// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;

namespace Tiledriver.Core.Utils.Images
{
    public sealed class ScaledFastImage : IFastImage
    {
        private readonly uint _scale;
        private readonly FastImage _image;

        public ScaledFastImage(SKSizeI resolution, uint scale) : this(resolution.Width, resolution.Height, scale) { }
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
        public void Fill(SKColor color) => _image.Fill(color);
        public void Save(string filePath) => _image.Save(filePath);
        public void SetPixel(int pixelIndex, SKColor color) => SetPixel(pixelIndex % Width, pixelIndex / Height, color);
        public void SetPixel(SKPointI p, SKColor color) => SetPixel(p.X, p.Y, color);
        public void SetPixel(int x, int y, SKColor color)
        {
            for (int pixelY = y * (int)_scale; pixelY < (y + 1) * _scale; pixelY++)
            {
                for (int pixelX = x * (int)_scale; pixelX < (x + 1) * _scale; pixelX++)
                {
                    _image.SetPixel(pixelX, pixelY, color);
                }
            }
        }
        public void Dispose() => _image.Dispose();
    }
}