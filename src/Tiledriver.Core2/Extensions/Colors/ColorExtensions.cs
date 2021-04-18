// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using HsluvS;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.Extensions.Colors
{
    public static class ColorExtensions
    {
        public static (double h, double s, double l) ToHsl(this Color color) => 
            Hsluv.RgbToHsl((color.R / 255d, color.G / 255d, color.B / 255d));

        public static Color ToColor(this (double h, double s, double l) hsl)
        {
            var (r, g, b) = Hsluv.HslToRgb(hsl);
            return Color.FromArgb((int) (r * 255), (int) (g * 255), (int) (b * 255));
        }

        public static Color ToColor(this HslColor hsl) => hsl.ToTuple().ToColor();
    }
}