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
		const int height = width;
		const string prefix = "basic";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		double[] frequencies = [5, 10, 50, 100];

		Parallel.ForEach(
			frequencies,
			frequency =>
			{
				using var image = new FastImage(width, height);

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = (double)x / width - 0.5;
						double ny = (double)y / height - 0.5;
						var noise = (NoiseGenerator.Generate2D(frequency * nx, frequency * ny) + 1) / 2f;

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, SKColor.FromHsv(0, 0, noise * 100));
					}
				}

				SaveImage(image, $"{prefix} - frequency {frequency}.png");
			}
		);
	}

	[Test, Explicit]
	public void Octaves()
	{
		const int width = 512;
		const int height = width;
		const string prefix = "octaves";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<IReadOnlyCollection<Octave>> octaveSets =
		[
			[new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)],
			[new(1, 1), new(2, 0.5), new(4, 0.25), new(8, 0.125)],
			[new(2, 1), new(4, 0.5), new(8, 0.25), new(16, 0.125)],
		];

		Parallel.ForEach(
			octaveSets,
			octaves =>
			{
				var amSum = octaves.Select(o => o.Amplitude).Sum();

				using var image = new FastImage(width, height, scale: 4);

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = (double)x / width - 0.5;
						double ny = (double)y / height - 0.5;

						float elevation = (float)(
							octaves
								.Select(
									(o, i) =>
										o.Amplitude
										* (NoiseGenerator.Generate2D(o.Frequency * nx, o.Frequency * ny, seed: i) + 1)
										/ 2f
								)
								.Sum() / amSum
						);

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, SKColor.FromHsv(0, 0, elevation * 100));
					}
				}

				var description = string.Join(" - ", octaves.Select(o => $"f{o.Frequency:N} a{o.Amplitude:N3}"));
				SaveImage(image, $"{prefix} - {description}.png");
			}
		);
	}

	[Test, Explicit]
	public void RedistributedOctaves()
	{
		const int width = 512;
		const int height = width;
		const string prefix = "redistributed";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		IReadOnlyCollection<double> redistributionPowers = [1, 2, 3];

		Parallel.ForEach(
			redistributionPowers,
			power =>
			{
				var amSum = octaves.Select(o => o.Amplitude).Sum();

				using var image = new FastImage(width, height, scale: 4);

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = (double)x / width - 0.5;
						double ny = (double)y / height - 0.5;

						var e =
							octaves
								.Select(
									(o, i) =>
										o.Amplitude
										* (NoiseGenerator.Generate2D(o.Frequency * nx, o.Frequency * ny, seed: i) + 1)
										/ 2f
								)
								.Sum() / amSum;

						var elevation = (float)Math.Pow(e, power);

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, SKColor.FromHsv(0, 0, elevation * 100));
					}
				}

				SaveImage(image, $"{prefix} - {power:N2}.png");
			}
		);
	}
}
