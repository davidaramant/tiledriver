// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using SkiaSharp;
using System;
using System.Threading.Tasks;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.Images
{
    public static class BinaryVisualizer
    {
        public static IFastImage Render(
            Size dimensions,
            Func<Position, bool> isTrue,
            SKColor trueColor,
            SKColor falseColor,
            int scale = 10)
        {
            var image = new FastImage(dimensions.Width, dimensions.Height, scale);

            Parallel.For(0, dimensions.Height, y =>
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    image.SetPixel(x, y, isTrue(new Position(x,y)) ? trueColor : falseColor);
                }
            });

            return image;
        }
    }
}
