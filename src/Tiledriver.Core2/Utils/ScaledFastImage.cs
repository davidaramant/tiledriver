// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.Utils
{
    public sealed class ScaledFastImage : IFastImage
    {
        private readonly uint _scale;
        private readonly FastImage _image;

        public ScaledFastImage(Size resolution, uint scale) : this(resolution.Width, resolution.Height, scale) { }
        public ScaledFastImage(int width, int height, uint scale)
        {
            Width = width;
            Height = height;
            _scale = scale;
            _image = new FastImage(width * (int)scale, height * (int)scale);
        }

        public int Height { get; }
        public int PixelCount => Height * Width;
        public int Width { get; }
        public void Fill(Color color) => _image.Fill(color);
        public void Save(string filePath) => _image.Save(filePath);
        public void SetPixel(int pixelIndex, Color color) => SetPixel(pixelIndex % Width, pixelIndex / Height, color);
        public void SetPixel(Point p, Color color) => SetPixel(p.X, p.Y, color);
        public void SetPixel(int x, int y, Color color)
        {
            for (int pixelY = y * (int)_scale; pixelY < (y + 1) * _scale; pixelY++)
            {
                for (int pixelX = x * (int)_scale; pixelX < (x + 1) * _scale; pixelX++)
                {
                    _image.SetPixel(pixelX, pixelY, color);
                }
            }
        }

    }
}