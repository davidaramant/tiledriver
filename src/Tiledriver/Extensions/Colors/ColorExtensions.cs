using HsluvS;
using SkiaSharp;
using Tiledriver.Utils;

namespace Tiledriver.Extensions.Colors;

public static class ColorExtensions
{
	public static (double h, double s, double l) ToHsl(this SKColor color) =>
		Hsluv.RgbToHsl((color.Red / 255d, color.Green / 255d, color.Blue / 255d));

	public static SKColor ToSKColor(this (double h, double s, double l) hsl)
	{
		var (r, g, b) = Hsluv.HslToRgb(hsl);
		return new SKColor((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
	}

	public static SKColor ToColor(this HslColor hsl) => hsl.ToTuple().ToSKColor();

	public static SKColor Multiply(this SKColor c, double scale)
	{
		return new SKColor(Clamp((byte)(c.Red * scale)), Clamp((byte)(c.Green * scale)), Clamp((byte)(c.Blue * scale)));

		static byte Clamp(byte v) => Math.Clamp(v, byte.MinValue, byte.MaxValue);
	}
}
