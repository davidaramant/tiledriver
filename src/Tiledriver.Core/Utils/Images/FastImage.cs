// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using SkiaSharp;

namespace Tiledriver.Core.Utils.Images
{
    public sealed class FastImage : IFastImage
    {
        private readonly SKBitmap _bitmap;

        public int Width { get; }
        public int Height { get; }
        public int PixelCount => Width * Height;

        public FastImage(int tileSize) : this(tileSize, tileSize)
        {
        }

        public FastImage(SKSizeI resolution) : this(resolution.Width, resolution.Height)
        {
        }

        public FastImage(int width, int height)
        {
            Width = width;
            Height = height;
            _bitmap = new SKBitmap(width, height);
        }

        public void Fill(SKColor color)
        {
            using var canvas = new SKCanvas(_bitmap);
            canvas.Clear(color);
        }

        public void SetPixel(SKPointI p, SKColor color) => SetPixel(p.X, p.Y, color);

        public void SetPixel(int x, int y, SKColor color)
        {
            _bitmap.SetPixel(x, y, color);
        }

        public void SetPixel(int pixelIndex, SKColor color)
        {
            var x = pixelIndex % Width;
            var y = pixelIndex / Width;

            SetPixel(x, y, color);
        }

        /// <summary>
        /// Saves the image to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Create);

            switch (Path.GetExtension(filePath))
            {
                case ".jpg":
                case ".jpeg":
                    _bitmap.Encode(stream, SKEncodedImageFormat.Jpeg, quality: 85);
                    break;
                case ".png":
                    _bitmap.Encode(stream, SKEncodedImageFormat.Png, quality: 100);
                    break;
                default:
                    throw new ArgumentException("Unsupported file format.");
            }
        }

        public void Dispose() => _bitmap.Dispose();
    }
}