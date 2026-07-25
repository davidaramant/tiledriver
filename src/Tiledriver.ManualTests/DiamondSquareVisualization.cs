using SkiaSharp;
using Tiledriver.Utils.Images;

namespace Tiledriver.ManualTests;

public sealed record InitialCorners
{
	public double TopLeft { get; }
	public double TopRight { get; }
	public double BottomLeft { get; }
	public double BottomRight { get; }

	public InitialCorners(double topLeft, double topRight, double bottomLeft, double bottomRight)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(topLeft, 0.0);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(topLeft, 1.0);
		ArgumentOutOfRangeException.ThrowIfLessThan(topRight, 0.0);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(topRight, 1.0);
		ArgumentOutOfRangeException.ThrowIfLessThan(bottomLeft, 0.0);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(bottomLeft, 1.0);
		ArgumentOutOfRangeException.ThrowIfLessThan(bottomRight, 0.0);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(bottomRight, 1.0);

		TopLeft = topLeft;
		TopRight = topRight;
		BottomLeft = bottomLeft;
		BottomRight = bottomRight;
	}

	public static InitialCorners MakeRandom(Random? random = null)
	{
		random ??= Random.Shared;
		return new(random.NextDouble(), random.NextDouble(), random.NextDouble(), random.NextDouble());
	}
}

[TestFixture]
public sealed class DiamondSquareVisualization() : BaseVisualization("Diamond Square")
{
	[Test, Explicit]
	public void BlackAndWhite()
	{
		const string prefix = "bw";
		DeleteImages(prefix);

		Parallel.ForEach(
			[0.1, 0.3, 0.5, 0.7, 0.9],
			h =>
			{
				var map = GenerateDiamondSquareMap(
					n: 9,
					h: h,
					initialCorners: new InitialCorners(topLeft: 0, topRight: 1, bottomLeft: 1, bottomRight: 0),
					random: new Random(0)
				);

				using var image = new FastImage(map.GetLength(0), map.GetLength(1));
				for (int y = 0; y < map.GetLength(0); y++)
				{
					for (int x = 0; x < map.GetLength(1); x++)
					{
						var value = map[y, x];
						if (value is < 0 or > 1)
							throw new InvalidOperationException("Map value out of range [0, 1]");
						image.SetColor(x, y, SKColor.FromHsv(0, 0, (float)value * 100));
					}
				}

				SaveImage(image, $"{prefix} - h {h:N1}.png");
			}
		);
	}

	private static double[,] GenerateDiamondSquareMap(
		int n,
		double h,
		InitialCorners? initialCorners = null,
		Random? random = null
	)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(h, 0);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(h, 1);

		random ??= Random.Shared;
		var corners = initialCorners ?? InitialCorners.MakeRandom(random);
		var size = (1 << n) + 1;
		var map = new double[size, size];

		map[0, 0] = corners.TopLeft;
		map[0, size - 1] = corners.TopRight;
		map[size - 1, 0] = corners.BottomLeft;
		map[size - 1, size - 1] = corners.BottomRight;

		var scale = 1.0;
		var step = size - 1;

		for (int i = 0; i < n; i++)
		{
			scale *= Math.Pow(2.0, -h);
			var half = step / 2;

			// Diamond step: set the center of each square to the average of its 4 corners
			for (int y = 0; y < size - 1; y += step)
			{
				for (int x = 0; x < size - 1; x += step)
				{
					var avg = (map[y, x] + map[y, x + step] + map[y + step, x] + map[y + step, x + step]) / 4.0;
					map[y + half, x + half] = avg + (random.NextDouble() * 2.0 - 1.0) * scale;
				}
			}

			// Square step: set the midpoint of each diamond edge to the average of up to 4 neighbors
			for (int y = 0; y <= size - 1; y += half)
			{
				var xOffset = (y % step == 0) ? half : 0;
				for (int x = xOffset; x <= size - 1; x += step)
				{
					double sum = 0;
					int count = 0;

					if (y - half >= 0)
					{
						sum += map[y - half, x];
						count++;
					}
					if (y + half < size)
					{
						sum += map[y + half, x];
						count++;
					}
					if (x - half >= 0)
					{
						sum += map[y, x - half];
						count++;
					}
					if (x + half < size)
					{
						sum += map[y, x + half];
						count++;
					}

					map[y, x] = sum / count + (random.NextDouble() * 2.0 - 1.0) * scale;
				}
			}

			step = half;
		}

		// Normalize to [0, 1]
		var min = double.MaxValue;
		var max = double.MinValue;
		foreach (var value in map)
		{
			if (value < min)
				min = value;
			if (value > max)
				max = value;
		}
		var range = max - min;
		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				map[y, x] = (map[y, x] - min) / range;
			}
		}

		return map;
	}
}
