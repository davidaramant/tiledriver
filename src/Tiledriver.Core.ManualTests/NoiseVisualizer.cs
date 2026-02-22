using System.Collections;
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

	[Test, Explicit]
	public void RedistributedOctavesBinary()
	{
		const int width = 512;
		const int height = width;
		const string prefix = "binary";

		foreach (var imagePath in Directory.GetFiles(_dirInfo.FullName, "*.png"))
		{
			if (Path.GetFileName(imagePath).StartsWith(prefix))
				File.Delete(imagePath);
		}

		IReadOnlyCollection<Octave> octaves = [new(5, 1), new(10, 0.5), new(20, 0.25), new(40, 0.125)];
		const double power = 2;

		IReadOnlyList<double> cutOffs = [0.1, 0.15, 0.2, 0.4, 0.6];

		var amSum = octaves.Select(o => o.Amplitude).Sum();

		using var images = new DisposableList<FastImage>(cutOffs.Select(_ => new FastImage(width, height, scale: 4)));

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

				e = double.Lerp(e, 1 - d, 0.55);
				var elevation = (float)Math.Pow(e, power);

				for (int i = 0; i < cutOffs.Count; i++)
				{
					var cutOff = cutOffs[i];
					var image = images[i];

					var isLand = elevation > cutOff;
					var color = isLand ? SKColors.White : SKColors.Black;

					// h: 0 - 360
					// s: 0 - 100
					// v: 0 - 100
					image.SetPixel(x, y, color);
				}
			}
		}

		for (int i = 0; i < cutOffs.Count; i++)
		{
			var cutOff = cutOffs[i];
			var image = images[i];
			SaveImage(image, $"{prefix} - cutoff {cutOff:N2}.png");
		}
	}

	private sealed class DisposableList<T> : IReadOnlyList<T>, IDisposable
		where T : IDisposable
	{
		private readonly List<T> _items;

		public DisposableList(IEnumerable<T> items) => _items = items.ToList();

		public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

		public int Count => _items.Count;

		public T this[int index] => _items[index];

		public void Dispose()
		{
			foreach (var item in _items)
				item.Dispose();
		}
	}
}
