// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using BenchmarkDotNet.Attributes;
using SkiaSharp;
using Tiledriver.Core.Utils.Images;

namespace Benchmarks
{
    public class FastImageVsScaledImageBenchmarks
    {
        private const int Size = 10;

        [Benchmark]
        public IFastImage CreateNormal()
        {
            var image = new FastImage(Size);
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    image.SetPixel(x, y, new SKColor((uint)(x * y)));
                }
            }

            return image;
        }
        [Benchmark]
        public IFastImage CreateScaled()
        {
            var image = new ScaledFastImage(Size, Size, scale: 1);
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    image.SetPixel(x, y, new SKColor((uint)(x * y)));
                }
            }

            return image;
        }
    }
}
