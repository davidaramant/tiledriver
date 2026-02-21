using SkiaSharp;
using Tiledriver.Core.Utils.Images;
using Tiledriver.Core.Utils.Noise;

namespace Tiledriver.Core.ManualTests;

[TestFixture]
public class NoiseTerrainVisualization
{
	private readonly DirectoryInfo _dirInfo = OutputLocation.CreateDirectory("Noise Terrain");

	void SaveImage(IFastImage image, string description) =>
		image.Save(Path.Combine(_dirInfo.FullName, $"{description}.png"));

	enum Biome
	{
		Water,
		Beach,
		Forest,
		Jungle,
		Savannah,
		Desert,
		BareRock,
		Snow,
	}

	static Biome ElevationToBiome(float elevation) =>
		elevation switch
		{
			< 0.2f => Biome.Water,
			< 0.3f => Biome.Beach,
			< 0.4f => Biome.Forest,
			< 0.5f => Biome.Jungle,
			< 0.7f => Biome.Savannah,
			< 0.8f => Biome.Desert,
			_ => Biome.Snow,
		};

	static SKColor BiomeToColor(Biome biome) =>
		biome switch
		{
			Biome.Water => SKColors.Blue,
			Biome.Beach => SKColors.Yellow,
			Biome.Forest => SKColors.Green,
			Biome.Jungle => SKColors.DarkGreen,
			Biome.Savannah => SKColors.Khaki,
			Biome.Desert => SKColors.SandyBrown,
			Biome.Snow => SKColors.White,
			Biome.BareRock => SKColors.Gray,
			_ => throw new ArgumentOutOfRangeException(nameof(biome), biome, null),
		};

	static Biome GetBiome(float elevation, float moisture)
	{
		if (elevation < 0.2)
			return Biome.Water;
		if (elevation < 0.3)
			return Biome.Beach;

		if (elevation > 0.8)
		{
			return moisture > 0.5 ? Biome.Snow : Biome.BareRock;
		}

		return moisture switch
		{
			< 0.2f => Biome.Desert,
			< 0.4f => Biome.Savannah,
			< 0.8f => Biome.Forest,
			_ => Biome.Jungle,
		};
	}

	[Test, Explicit]
	public void BasicBiome()
	{
		const int width = 512;
		const int height = 512;
		const string prefix = "biome - basic";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		const double power = 2;

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

					var biome = ElevationToBiome(elevation);
					var color = BiomeToColor(biome);

					// h: 0 - 360
					// s: 0 - 100
					// v: 0 - 100
					image.SetPixel(x, y, color);
				}
			}

			SaveImage(image, $"{prefix}.png");
		}
	}

	[Test, Explicit]
	public void BiomeWithMoisture()
	{
		const int width = 512;
		const int height = 512;
		const string prefix = "biome - moisture";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		const double power = 2;

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

					var moisture =
						octaves
							.Select(
								(o, i) =>
									o.Amplitude
									* (NoiseGenerator.Generate2D(o.Frequency * nx, o.Frequency * ny, seed: i + 1) + 1)
									/ 2f
							)
							.Sum() / amSum;

					var elevation = (float)Math.Pow(e, power);

					var biome = GetBiome(elevation, (float)moisture);
					var color = BiomeToColor(biome);

					// h: 0 - 360
					// s: 0 - 100
					// v: 0 - 100
					image.SetPixel(x, y, color);
				}
			}

			SaveImage(image, $"{prefix}.png");
		}
	}
}
