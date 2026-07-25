using SkiaSharp;
using Tiledriver.Extensions.Colors;
using Tiledriver.Rendering;

namespace Tiledriver.ManualTests;

[TestFixture]
public sealed class DrawingPrimitiveVisualizations() : BaseVisualization("Drawing Primitives")
{
	[Test, Explicit]
	public void Lines([Values] bool smooth)
	{
		const int size = 256;
		const int stepSize = size / 32;

		var buffer = new PixelBuffer(size, size);
		buffer.Fill(SKColors.Black);

		for (int i = 0; i < size; i += stepSize)
		{
			buffer.DrawLine(
				0,
				i / 2,
				i,
				0,
				SKColors.Red.Multiply(i / (double)size),
				smooth ? LineMode.Smooth : LineMode.Exact
			);
			buffer.DrawLine(
				0,
				size - i / 2,
				i,
				size - 1,
				SKColors.Red.Multiply(i / (double)size),
				smooth ? LineMode.Smooth : LineMode.Exact
			);
		}

		SaveImage(buffer, smooth ? "Lines - Smooth (Wu)" : "Lines - Exact (Bresenham)", scale: 4);
	}

	[Test, Explicit]
	public void Circle()
	{
		const int size = 256;
		const int half = size / 2;
		const int stepSize = size / 32;

		var buffer = new PixelBuffer(size, size);
		buffer.Fill(SKColors.Black);

		for (int i = 0; i < half; i += stepSize)
		{
			buffer.DrawCircle(size / 2, size / 2, i, SKColors.Red.Multiply(i / (double)half));
		}

		SaveImage(buffer, "Circles - Exact (Bresenham)", scale: 4);
	}
}
