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

				SaveImage(image, $"basic - frequency {frequency}.png");
			}
		);
	}

	[Test, Explicit]
	public void Octaves()
	{
		const int width = 512;
		const int height = 512;

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith("octaves"))
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
				SaveImage(image, $"octaves - {description}.png");
			}
		);
	}

	[Test, Explicit]
	public void RedistributedOctaves()
	{
		const int width = 512;
		const int height = 512;

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith("redistributed"))
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

				SaveImage(image, $"redistributed - {power:N2}.png");
			}
		);
	}

	[Test, Explicit]
	public void RadialNoise()
	{
		const int width = 2048;
		const int height = width;

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith("radial"))
				File.Delete(imagePath);
		}

		double[] frequencies = [2, 3.5, 5];
		float[] innerRadiusFactors = [0.05f, 0.1f, 0.15f, 0.25f, 0.5f];

		var combinations = frequencies
			.SelectMany(frequency =>
				innerRadiusFactors.Select(innerRadiusFactor =>
					(Frequency: frequency, InnerRadiusFactors: innerRadiusFactor)
				)
			)
			.ToList();

		float cx = width / 2f;
		float cy = height / 2f;
		float outerRadius = Math.Min(width, height) / 2f;

		Parallel.ForEach(
			combinations,
			renderParams =>
			{
				using var image = new FastImage(width, height);

				float innerRadius = outerRadius * renderParams.InnerRadiusFactors;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = (double)x / width - 0.5;
						double ny = (double)y / height - 0.5;
						var noise =
							(NoiseGenerator.Generate2D(renderParams.Frequency * nx, renderParams.Frequency * ny) + 1)
							/ 2f;

						float dx = x - cx;
						float dy = y - cy;
						float distance = MathF.Sqrt(dx * dx + dy * dy);

						float falloff = CalculateFalloff(distance, innerRadius, outerRadius);
						noise *= falloff;

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, SKColor.FromHsv(0, 0, noise * 100));
					}
				}

				SaveImage(
					image,
					$"radial - scale {renderParams.Frequency:N1} - inner radius {renderParams.InnerRadiusFactors:N2}.png"
				);
			}
		);

		static float CalculateFalloff(float distance, float innerRadius, float outerRadius)
		{
			if (distance >= outerRadius)
				return 0f;
			if (distance <= innerRadius)
				return 1f;

			var t = (distance - innerRadius) / (outerRadius - innerRadius);
			return 1f - t * t * t;
		}
	}
}
