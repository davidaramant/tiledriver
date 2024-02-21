// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using HsluvS;
using SkiaSharp;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.Extensions.Colors
{
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
	}
}
