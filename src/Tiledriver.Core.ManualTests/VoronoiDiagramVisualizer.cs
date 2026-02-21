using SharpVoronoiLib;
using SkiaSharp;
using Tiledriver.Core.Utils.Images;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class VoronoiDiagramVisualizer
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Voronoi Diagrams");

	void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	[Test, Explicit]
	public void DrawVoronoiDiagrams()
	{
		const int width = 2048;
		const int height = 2048;

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith("Voronoi"))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<int> relaxIterations = [1, 2, 3];
		IReadOnlyCollection<float> relaxStrengths = [0.5f, 0.7f, 0.9f, 1];

		var combinations = relaxIterations
			.SelectMany(iterations => relaxStrengths.Select(strength => (iterations, strength)))
			.ToList();

		Parallel.ForEach(
			combinations,
			relaxArgs =>
			{
				var bitmap = new SKBitmap(width, height);
				using var canvas = new SKCanvas(bitmap);
				canvas.Clear(SKColors.White);

				const int numberOfSites = 500;

				List<VoronoiSite> sites = Enumerable
					.Range(1, numberOfSites)
					.Select(_ => new VoronoiSite(Random.Shared.Next(width), Random.Shared.Next(height)))
					.ToList();

				VoronoiPlane plane = new VoronoiPlane(0, 0, width, height);

				plane.SetSites(sites);

				plane.Tessellate();

				List<VoronoiEdge> edges = plane.Relax(relaxArgs.iterations, relaxArgs.strength);

				foreach (var edge in edges)
				{
					canvas.DrawLine(
						(float)edge.Start.X,
						(float)edge.Start.Y,
						(float)edge.End.X,
						(float)edge.End.Y,
						new SKPaint { Color = SKColors.Black, StrokeWidth = 1 }
					);

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

				using var image = FastImage.WrapSKBitmap(bitmap, scale: 1);

				SaveImage(image, $"Voronoi - iterations {relaxArgs.iterations}  - factor {relaxArgs.strength}");
			}
		);
	}
}
