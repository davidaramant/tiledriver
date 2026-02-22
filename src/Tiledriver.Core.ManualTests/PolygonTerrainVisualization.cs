using SharpVoronoiLib;
using SkiaSharp;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.TerrainMaps;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class PolygonTerrainVisualization() : BaseVisualization("Polygon Terrain")
{
	[Test, Explicit]
	public void OverlayCellularAutomata()
	{
		const string prefix = "overlay";
		DeleteImages(prefix);

		const int size = 2048;

		var voronoiParams = new VoronoiParams(
			Width: size,
			Height: size,
			NumberOfSites: 4000,
			RelaxIterations: 3,
			RelaxStrength: 0.25f,
			PointMethod: PointGenerationMethod.Uniform
		);
		var caParams = new CellularAutomataParams(ProbabalityAlive: 0.48, Size: 32, RandomSeed: 0);

		var random = new Random(caParams.RandomSeed);

		var caBoard = new CellBoard(new Size(caParams.Size, caParams.Size))
			.Fill(random, probabilityAlive: caParams.ProbabalityAlive)
			.MakeBorderAlive(thickness: 1)
			.GenerateStandardCave();

		var caScale = (double)voronoiParams.Width / caBoard.Width;

		VoronoiPlane plane = new(0, 0, voronoiParams.Width, voronoiParams.Height);
		plane.GenerateRandomSites(voronoiParams.NumberOfSites, voronoiParams.PointMethod);
		plane.Tessellate();
		plane.Relax(voronoiParams.RelaxIterations, voronoiParams.RelaxStrength);

		var sites = plane.Sites.Where(s => !s.SkippedAsDuplicate).ToList();

		foreach (var markRegions in new[] { false, true })
		{
			var bitmap = new SKBitmap(voronoiParams.Width, voronoiParams.Height);
			using var canvas = new SKCanvas(bitmap);
			canvas.Clear(SKColors.White);

			// Draw sites
			foreach (var site in sites)
			{
				if (markRegions)
				{
					var caX = (int)(site.X / caScale);
					var caY = (int)(site.Y / caScale);
					var siteCenterInDeadCell = caBoard[new Position(caX, caY)] == CellType.Dead;

					var color = siteCenterInDeadCell ? SKColors.LightGreen : SKColors.RoyalBlue;

					var points = site.ClockwisePoints.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
					using var path = new SKPath();
					path.AddPoly(points, close: true);
					canvas.DrawPath(path, new SKPaint { Color = color, Style = SKPaintStyle.Fill });
				}

				canvas.DrawPoint(
					new SKPoint((float)site.X, (float)site.Y),
					new SKPaint
					{
						Color = SKColors.Red,
						Style = SKPaintStyle.Fill,
						StrokeWidth = 3,
					}
				);
			}

			// Draw region edges
			foreach (var edge in plane.Edges)
			{
				canvas.DrawLine(
					(float)edge.Start.X,
					(float)edge.Start.Y,
					(float)edge.End.X,
					(float)edge.End.Y,
					new SKPaint
					{
						Color = SKColors.Black,
						StrokeWidth = 2,
						IsAntialias = true,
					}
				);
			}

			// Draw CA board
			{
				for (int y = 0; y < caBoard.Height; y++)
				{
					for (int x = 0; x < caBoard.Width; x++)
					{
						if (caBoard[new Position(x, y)] == CellType.Dead)
						{
							canvas.DrawRect(
								(float)(x * caScale),
								(float)(y * caScale),
								(float)caScale,
								(float)caScale,
								new SKPaint { Color = SKColors.SlateGray.WithAlpha(128), Style = SKPaintStyle.Fill }
							);
						}
					}
				}
			}

			using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);
			SaveImage(image, $"{prefix} - mark regions {markRegions}");
		}
	}

	[Test, Explicit]
	public void DetermineDistanceFromWater()
	{
		const string prefix = "distance from water";
		DeleteImages(prefix);

		const int size = 2048;

		var vParams = new VoronoiParams(
			Width: size,
			Height: size,
			NumberOfSites: 4000,
			RelaxIterations: 3,
			RelaxStrength: 0.25f,
			PointMethod: PointGenerationMethod.Uniform
		);
		var caParams = new CellularAutomataParams(ProbabalityAlive: 0.48, Size: 32, RandomSeed: 0);

		Parallel.ForEach(
			[vParams, vParams with { PointMethod = PointGenerationMethod.Gaussian }],
			voronoiParams =>
			{
				var random = new Random(caParams.RandomSeed);

				var caBoard = new CellBoard(new Size(caParams.Size, caParams.Size))
					.Fill(random, probabilityAlive: caParams.ProbabalityAlive)
					.MakeBorderAlive(thickness: 1)
					.GenerateStandardCave();

				var caScale = (double)voronoiParams.Width / caBoard.Width;

				VoronoiPlane plane = new(0, 0, voronoiParams.Width, voronoiParams.Height);
				plane.GenerateRandomSites(voronoiParams.NumberOfSites, voronoiParams.PointMethod);
				plane.Tessellate();
				plane.Relax(voronoiParams.RelaxIterations, voronoiParams.RelaxStrength);

				var sites = plane.Sites.Where(s => !s.SkippedAsDuplicate).ToList();

				var isLand = sites.ToDictionary(
					s => s,
					s =>
						caBoard[new Position((int)(s.X / caScale), (int)(s.Y / caScale))] == CellType.Dead
						&& s.Edges.All(e => e.Right != null && e.Left != null)
				);

				// Multi-source BFS: water sites start at distance 0; land sites get
				// the shortest hop-count to the nearest water neighbour.
				var distances = new Dictionary<VoronoiSite, int>();
				var queue = new Queue<VoronoiSite>();

				foreach (var site in sites.Where(s => !isLand[s]))
				{
					distances[site] = 0;
					queue.Enqueue(site);
				}

				while (queue.Count > 0)
				{
					var current = queue.Dequeue();
					var nextDist = distances[current] + 1;

					foreach (var neighbour in current.Neighbours)
					{
						if (neighbour.SkippedAsDuplicate || distances.ContainsKey(neighbour))
							continue;

						distances[neighbour] = nextDist;
						queue.Enqueue(neighbour);
					}
				}

				var regions = sites
					.Select(s => new Region(
						s,
						IsLand: isLand[s],
						DistanceFromWater: distances.GetValueOrDefault(s, -1)
					))
					.ToList();

				var maxDistance = regions.Where(r => r.IsLand).Max(r => r.DistanceFromWater);

				var bitmap = new SKBitmap(voronoiParams.Width, voronoiParams.Height);
				using var canvas = new SKCanvas(bitmap);

				// Draw regions
				foreach (var region in regions)
				{
					// Site polygons

					SKColor color;
					if (!region.IsLand)
					{
						color = SKColors.RoyalBlue;
					}
					else
					{
						// Interpolate from LightGreen (distance 1) to DarkGreen (max distance).
						var t =
							maxDistance > 1
								? Math.Clamp((region.DistanceFromWater - 1.0) / (maxDistance - 1.0), 0.0, 1.0)
								: 0.0;
						color = Lerp(SKColors.LightGreen, SKColors.DarkGreen, t);
					}

					var points = region.Site.ClockwisePoints.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
					using var path = new SKPath();
					path.AddPoly(points, close: true);
					canvas.DrawPath(path, new SKPaint { Color = color, Style = SKPaintStyle.Fill });

					// Distance text
					if (region.IsLand)
					{
						var text = region.DistanceFromWater.ToString();

						var font = new SKFont();
						font.MeasureText(text, out var textBounds);
						var centeredY = (float)region.Site.Y - (textBounds.Top + textBounds.Bottom) / 2;
						canvas.DrawText(
							text,
							(float)region.Site.X,
							centeredY,
							SKTextAlign.Center,
							font,
							new SKPaint { Color = SKColors.Black }
						);
					}
				}

				// Draw region edges
				foreach (var edge in plane.Edges)
				{
					canvas.DrawLine(
						(float)edge.Start.X,
						(float)edge.Start.Y,
						(float)edge.End.X,
						(float)edge.End.Y,
						new SKPaint
						{
							Color = SKColors.Black,
							StrokeWidth = 2,
							IsAntialias = true,
						}
					);
				}

				using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);
				SaveImage(image, $"{prefix} - {voronoiParams.PointMethod}");
			}
		);
	}

	[Test, Explicit]
	public void MergeRegions()
	{
		const string prefix = "merged";
		DeleteImages(prefix);

		const int size = 2048;

		var vpParams = new VoronoiParams(
			Width: size,
			Height: size,
			NumberOfSites: 4000,
			RelaxIterations: 3,
			RelaxStrength: 0.25f,
			PointMethod: PointGenerationMethod.Uniform
		);
		var caParams = new CellularAutomataParams(ProbabalityAlive: 0.48, Size: 32, RandomSeed: 0);

		IReadOnlyCollection<VoronoiParams> voronoiTrials =
		[
			vpParams,
			vpParams with
			{
				RelaxStrength = 0.5f,
			},
			vpParams with
			{
				RelaxStrength = 0.7f,
			},
			vpParams with
			{
				RelaxIterations = 4,
			},
		];

		Parallel.ForEach(
			voronoiTrials,
			voronoiParams =>
			{
				var caBoard = new CellBoard(new Size(caParams.Size, caParams.Size))
					.Fill(new Random(caParams.RandomSeed), probabilityAlive: caParams.ProbabalityAlive)
					.MakeBorderAlive(thickness: 1)
					.GenerateStandardCave();

				var caScale = (double)voronoiParams.Width / caBoard.Width;

				VoronoiPlane plane = new(0, 0, voronoiParams.Width, voronoiParams.Height);
				plane.GenerateRandomSites(voronoiParams.NumberOfSites, voronoiParams.PointMethod);
				plane.Tessellate();
				plane.Relax(voronoiParams.RelaxIterations, voronoiParams.RelaxStrength);

				var sites = plane.Sites.Where(s => !s.SkippedAsDuplicate).ToList();

				var isLand = sites.ToDictionary(
					s => s,
					s => caBoard[new Position((int)(s.X / caScale), (int)(s.Y / caScale))] == CellType.Dead
				);

				// Multi-source BFS: water sites start at distance 0; land sites get
				// the shortest hop-count to the nearest water neighbor.
				var distances = new Dictionary<VoronoiSite, int>();
				var queue = new Queue<VoronoiSite>();

				foreach (var site in sites.Where(s => !isLand[s]))
				{
					distances[site] = 0;
					queue.Enqueue(site);
				}

				while (queue.Count > 0)
				{
					var current = queue.Dequeue();
					var nextDist = distances[current] + 1;

					foreach (var neighbour in current.Neighbours)
					{
						if (neighbour.SkippedAsDuplicate || distances.ContainsKey(neighbour))
							continue;

						distances[neighbour] = nextDist;
						queue.Enqueue(neighbour);
					}
				}

				var regions = sites
					.Select(s => new Region(
						s,
						IsLand: isLand[s],
						DistanceFromWater: distances.GetValueOrDefault(s, -1)
					))
					.ToDictionary(region => region.Site);

				var maxDistance = regions.Values.Where(r => r.IsLand).Max(r => r.DistanceFromWater);

				var bitmap = new SKBitmap(voronoiParams.Width, voronoiParams.Height);
				using var canvas = new SKCanvas(bitmap);

				// Draw regions
				foreach (var region in regions.Values)
				{
					// Site polygons

					SKColor color;
					if (!region.IsLand)
					{
						color = SKColors.RoyalBlue;
					}
					else
					{
						// Interpolate from LightGreen (distance 1) to DarkGreen (max distance).
						var t =
							maxDistance > 1
								? Math.Clamp((region.DistanceFromWater - 1.0) / (maxDistance - 1.0), 0.0, 1.0)
								: 0.0;
						color = Lerp(SKColors.LightGreen, SKColors.DarkGreen, t);
					}

					var points = region.Site.ClockwisePoints.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
					using var path = new SKPath();
					path.AddPoly(points, close: true);
					canvas.DrawPath(path, new SKPaint { Color = color, Style = SKPaintStyle.Fill });

					// Outline
					foreach (var edge in region.Site.Edges.Where(e => e.Right is not null && e.Left is not null))
					{
						var leftRegion = regions[edge.Left!];
						var rightRegion = regions[edge.Right!];

						if (leftRegion.DistanceFromWater != rightRegion.DistanceFromWater)
						{
							canvas.DrawLine(
								(float)edge.Start.X,
								(float)edge.Start.Y,
								(float)edge.End.X,
								(float)edge.End.Y,
								new SKPaint
								{
									Color = SKColors.Black,
									StrokeWidth = 2,
									IsAntialias = true,
								}
							);
						}
					}
				}

				using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);
				SaveImage(
					image,
					$"{prefix} - relax iterations {voronoiParams.RelaxIterations} - relax strength {voronoiParams.RelaxStrength:N2}"
				);
			}
		);
	}

	private sealed record CellularAutomataParams(double ProbabalityAlive, int Size, int RandomSeed);

	private sealed record VoronoiParams(
		int Width,
		int Height,
		int NumberOfSites,
		int RelaxIterations,
		float RelaxStrength,
		PointGenerationMethod PointMethod
	);

	private static SKColor Lerp(SKColor from, SKColor to, double t) =>
		new(
			(byte)(from.Red + (to.Red - from.Red) * t),
			(byte)(from.Green + (to.Green - from.Green) * t),
			(byte)(from.Blue + (to.Blue - from.Blue) * t)
		);

	private sealed record Region(VoronoiSite Site, bool IsLand, int DistanceFromWater);
}
