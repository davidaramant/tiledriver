// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;
using System;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.Images
{
    public static class GenericVisualizer
    {
        public static IFastImage RenderBinary(
                Size dimensions,
                Func<Position, bool> isTrue,
                SKColor trueColor,
                SKColor falseColor,
                int scale = 1) =>
            RenderPalette(dimensions, getColor: p => isTrue(p) ? trueColor : falseColor, scale);

        public static IFastImage RenderPalette(
            Size dimensions,
            Func<Position, SKColor> getColor,
            int scale = 1)
        {
            var image = new FastImage(dimensions.Width, dimensions.Height, scale);

            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    image.SetPixel(x, y, getColor(new Position(x, y)));
                }
            }

            return image;
        }
    }
}
