using SkiaSharp;
using Tiledriver.LevelGeometry;

namespace Tiledriver.Utils.Images;

public static class GenericVisualizer
{
	public static IFastImage RenderBinary(
		Size dimensions,
		Func<Position, bool> isTrue,
		SKColor trueColor,
		SKColor falseColor
	) => RenderPalette(dimensions, getColor: p => isTrue(p) ? trueColor : falseColor);

	public static IFastImage RenderPalette(Size dimensions, Func<Position, SKColor> getColor)
	{
		var image = new FastImage(dimensions.Width, dimensions.Height);

		for (int y = 0; y < dimensions.Height; y++)
		{
			for (int x = 0; x < dimensions.Width; x++)
			{
				image.SetColor(x, y, getColor(new Position(x, y)));
			}
		}

		return image;
	}
}
