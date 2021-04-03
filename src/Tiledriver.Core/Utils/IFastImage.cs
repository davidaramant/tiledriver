// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.Utils
{
    public interface IFastImage
    {
        int Height { get; }
        int PixelCount { get; }
        int Width { get; }

        void Fill(Color color);
        void Save(string filePath);
        void SetPixel(int pixelIndex, Color color);
        void SetPixel(int x, int y, Color color);
        void SetPixel(Point p, Color color);
    }
}