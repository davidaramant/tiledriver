using System.Numerics;
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
	public void Islands()
	{
		const int width = 1024;
		const int height = width;
		const string prefix = "islands - basic";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		const double power = 2;

		IReadOnlyCollection<double> mixes = [0.5, 0.55, 0.6];

		Parallel.ForEach(
			mixes,
			mix =>
			{
				var amSum = octaves.Select(o => o.Amplitude).Sum();

				using var image = new FastImage(width, height, scale: 2);

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = 2d * x / width - 1;
						double ny = 2d * y / height - 1;

						// Square bump
						var d = 1 - (1 - nx * nx) * (1 - ny * ny);

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
										* (
											NoiseGenerator.Generate2D(o.Frequency * nx, o.Frequency * ny, seed: i + 1)
											+ 1
										)
										/ 2f
								)
								.Sum() / amSum;

						e = double.Lerp(e, 1 - d, mix);
						var elevation = (float)Math.Pow(e, power);

						var biome = GetBiome(elevation, (float)moisture);
						var color = BiomeToColor(biome);

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, color);
					}
				}

				SaveImage(image, $"{prefix} - {mix:N2}.png");
			}
		);
	}

	[Test, Explicit]
	public void TerracedIslands()
	{
		const int width = 1024;
		const int height = width;
		const string prefix = "islands - terraced";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		const double power = 2;
		const double mix = 0.6;
		var numBiomes = Enum.GetValues<Biome>().Length;
		IReadOnlyCollection<int> terraceOptions = [2 * numBiomes, 3 * numBiomes, 4 * numBiomes];

		Parallel.ForEach(
			terraceOptions,
			numTerraces =>
			{
				var amSum = octaves.Select(o => o.Amplitude).Sum();

				using var image = new FastImage(width, height, scale: 2);

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						double nx = 2d * x / width - 1;
						double ny = 2d * y / height - 1;

						// Square bump
						var d = 1 - (1 - nx * nx) * (1 - ny * ny);

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
										* (
											NoiseGenerator.Generate2D(o.Frequency * nx, o.Frequency * ny, seed: i + 1)
											+ 1
										)
										/ 2f
								)
								.Sum() / amSum;

						e = double.Lerp(e, 1 - d, mix);
						var elevation = (float)Math.Pow(e, power);

						var step = (int)MathF.Round(elevation * numTerraces);
						elevation = step / (float)numTerraces;

						var biome = GetBiome(elevation, (float)moisture);
						var color = BiomeToColor(biome);

						// Adjust even steps to create visible terrace banding
						if (step % 2 == 0)
						{
							color.ToHsv(out float h, out float s, out float v);
							color = SKColor.FromHsv(h, Math.Max(0, s - 10), Math.Max(0, v - 10));
						}

						// h: 0 - 360
						// s: 0 - 100
						// v: 0 - 100
						image.SetPixel(x, y, color);
					}
				}

				SaveImage(image, $"{prefix} - terraces {numTerraces:D2}.png");
			}
		);
	}
}
