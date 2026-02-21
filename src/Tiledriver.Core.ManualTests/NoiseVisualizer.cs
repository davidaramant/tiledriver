using SkiaSharp;
using Tiledriver.Core.Utils.Images;
using Tiledriver.Core.Utils.Noise;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public sealed class NoiseVisualizer
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Noise");

	void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	[Test, Explicit]
	public void BasicNoise()
	{
		const int width = 1024;
		const int height = 1024;

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith("basic"))
				File.Delete(imagePath);
		}

		double[] scales = [5, 10, 50, 100];

		Parallel.ForEach(
			scales,
			scale =>
			{
				using var image = new FastImage(width, height);

				float min = 0;
				float max = 0;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						var noise = (NoiseGenerator.Generate2D(scale * x / width, scale * y / height) + 1) / 2f;

						min = Math.Min(min, noise);
						max = Math.Max(max, noise);

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, SKColor.FromHsv(0, 0, noise * 100));
					}
				}

				Console.Out.WriteLine($"Min: {min}, Max: {max}");
				SaveImage(image, $"basic - scale {scale}.png");
			}
		);
	}
}
