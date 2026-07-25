using SkiaSharp;

namespace Tiledriver.Extensions.Skia;

public static class SkiaExtensions
{
	public static int Area(this SKSizeI s) => s.Height * s.Width;
}
