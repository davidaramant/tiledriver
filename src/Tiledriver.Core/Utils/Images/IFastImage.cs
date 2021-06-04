// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;

namespace Tiledriver.Core.Utils.Images
{
    public interface IFastImage
    {
        int Height { get; }
        int PixelCount { get; }
        int Width { get; }

        void Fill(SKColor color);
        void Save(string filePath);
        void SetPixel(int pixelIndex, SKColor color);
        void SetPixel(int x, int y, SKColor color);
        void SetPixel(SKPointI p, SKColor color);
    }
}