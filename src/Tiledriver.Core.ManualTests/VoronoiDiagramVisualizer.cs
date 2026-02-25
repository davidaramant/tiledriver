using SharpVoronoiLib;
using SkiaSharp;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class VoronoiDiagramVisualizer() : BaseVisualization("Voronoi Diagrams")
{
	[Test, Explicit]
	public void Basics()
	{
		const int width = 4096;
		const int height = width;
		const string prefix = "basics";

		DeleteImages(prefix);

		const int numberOfSites = 1000;

		VoronoiPlane plane = new VoronoiPlane(0, 0, width, height);
		plane.GenerateRandomSites(numberOfSites, random: new SeededRandomNumberGenerator(0));
		plane.Tessellate();

		var bitmap = new SKBitmap(width, height);
		using var canvas = new SKCanvas(bitmap);
		canvas.Clear(SKColors.White);

		foreach (var site in plane.Sites)
		{
			canvas.DrawPoint(
				new SKPoint((float)site.X, (float)site.Y),
				new SKPaint
				{
					Color = SKColors.Red,
					Style = SKPaintStyle.Fill,
					StrokeWidth = 10,
				}
			);
		}
		using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);
		SaveImage(FastImage.WrapSKBitmap(bitmap, scale: 1), $"{prefix} - 1 sites");

		foreach (var edge in plane.Edges)
		{
			if (edge.Right is not null && edge.Left is not null)
			{
				canvas.DrawLine(
					(float)edge.Right.X,
					(float)edge.Right.Y,
					(float)edge.Left.X,
					(float)edge.Left.Y,
					new SKPaint
					{
						Color = SKColors.Red,
						StrokeWidth = 1,
						IsAntialias = true,
					}
				);
			}
		}
		SaveImage(image, $"{prefix} - 2 sites delaunay");

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

		SaveImage(image, $"{prefix} - 3 sites delaunay edges");

		var justEdgesBitmap = new SKBitmap(width, height);
		using var justEdgesCanvas = new SKCanvas(justEdgesBitmap);
		justEdgesCanvas.Clear(SKColors.White);

		foreach (var edge in plane.Edges)
		{
			justEdgesCanvas.DrawLine(
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

		using var justEdgesImage = FastImage.WrapSKBitmap(justEdgesBitmap, scale: 1);
		SaveImage(justEdgesImage, $"{prefix} - 4 voronoi");
	}

	[Test, Explicit]
	public void RelaxingTriangulation()
	{
		const int width = 2048;
		const int height = width;
		const string prefix = "relaxing";

		DeleteImages(prefix);

		const int numberOfSites = 1000;

		IReadOnlyCollection<int> relaxIterations = [1, 2, 3];
		IReadOnlyCollection<float> relaxStrengths = [0.25f, 0.5f, 0.7f, 0.9f];

		var combinations = (
			from iterations in relaxIterations
			from strength in relaxStrengths
			select (Iterations: iterations, Strength: strength)
		).ToList();

		Parallel.ForEach(
			combinations,
			relaxArgs =>
			{
				VoronoiPlane plane = new VoronoiPlane(0, 0, width, height);

				plane.GenerateRandomSites(numberOfSites, random: new SeededRandomNumberGenerator(0));

				plane.Tessellate();

				List<VoronoiEdge> edges = plane.Relax(relaxArgs.Iterations, relaxArgs.Strength);

				var bitmap = new SKBitmap(width, height);
				using var canvas = new SKCanvas(bitmap);
				canvas.Clear(SKColors.White);

				foreach (var edge in edges)
				{
					canvas.DrawLine(
						(float)edge.Start.X,
						(float)edge.Start.Y,
						(float)edge.End.X,
						(float)edge.End.Y,
						new SKPaint { Color = SKColors.Black, StrokeWidth = 1 }
					);
				}

				using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);

				SaveImage(
					image,
					$"{prefix} - delaunay {false} - iterations {relaxArgs.Iterations}  - factor {relaxArgs.Strength:N2}"
				);

				foreach (var edge in edges)
				{
					if (edge.Right is not null && edge.Left is not null)
					{
						canvas.DrawLine(
							(float)edge.Right.X,
							(float)edge.Right.Y,
							(float)edge.Left.X,
							(float)edge.Left.Y,
							new SKPaint { Color = SKColors.Red, StrokeWidth = 1 }
						);
					}
				}

				SaveImage(
					image,
					$"{prefix} - delaunay {true} - iterations {relaxArgs.Iterations}  - factor {relaxArgs.Strength:N2}"
				);
			}
		);
	}

	[Test, Explicit]
	public void DrawVoronoiDiagramMesh()
	{
		const int width = 2048;
		const int height = width;
		const string prefix = "mesh";

		DeleteImages(prefix);

		const int numberOfSites = 1000;
		const int relaxIterations = 3;
		const float relaxStrength = 0.25f;

		Parallel.ForEach(
			[PointGenerationMethod.Uniform, PointGenerationMethod.Gaussian],
			pointGenerationMethod =>
			{
				VoronoiPlane plane = new VoronoiPlane(0, 0, width, height);

				plane.GenerateRandomSites(
					numberOfSites,
					pointGenerationMethod,
					random: new SeededRandomNumberGenerator(0)
				);

				plane.Tessellate();

				plane.Relax(relaxIterations, relaxStrength);

				var sites = plane.Sites.Where(s => !s.SkippedAsDuplicate).ToList();

				var meshBitmap = new SKBitmap(width, height);
				using var meshCanvas = new SKCanvas(meshBitmap);

				for (int index = 0; index < sites.Count; index++)
				{
					var site = sites[index];
					var points = site.ClockwisePoints.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
					using var path = new SKPath();
					path.AddPoly(points, close: true);
					meshCanvas.DrawPath(
						path,
						new SKPaint
						{
							// Golden angle spacing ensures maximally distinct, evenly distributed hues
							Color = SKColor.FromHsv(
								(index * 137.508f) % 360f,
								70f + (index % 4) * 10f,
								75f + (index % 3) * 8f
							),
							Style = SKPaintStyle.Fill,
						}
					);

					meshCanvas.DrawPoint(
						new SKPoint((float)site.X, (float)site.Y),
						new SKPaint
						{
							Color = SKColors.Black,
							Style = SKPaintStyle.Fill,
							StrokeWidth = 3,
						}
					);
				}

				using var meshImage = FastImage.WrapSKBitmap(meshBitmap, scale: 1);

				SaveImage(meshImage, $"{prefix} - {pointGenerationMethod}");
			}
		);
	}
}
