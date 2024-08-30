// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.IO;
using SkiaSharp;

namespace Tiledriver.Core.Utils.Images;

public sealed class FastImage : IFastImage
{
	private readonly int _scale;
	private readonly SKBitmap _bitmap;

	public int Width { get; }
	public int Height { get; }
	public int PixelCount => Width * Height;

	public FastImage(SKSizeI resolution, int scale = 1)
		: this(resolution.Width, resolution.Height, scale) { }

	public FastImage(int width, int height, int scale = 1)
	{
		_scale = scale;
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

		if (_scale != 1)
		{
			var resizedWidth = _scale * Width;
			var resizedHeight = _scale * Height;

			using var surface = SKSurface.Create(
				new SKImageInfo
				{
					Width = resizedWidth,
					Height = resizedHeight,
					ColorType = SKImageInfo.PlatformColorType,
					AlphaType = SKAlphaType.Premul,
				}
			);
			using var paint = new SKPaint { IsAntialias = false, FilterQuality = SKFilterQuality.None };

			using var img = SKImage.FromBitmap(_bitmap);

			surface.Canvas.DrawImage(img, new SKRectI(0, 0, resizedWidth, resizedHeight), paint);
			surface.Canvas.Flush();

			using var newImg = surface.Snapshot();
			using var data = Path.GetExtension(filePath).ToLowerInvariant() switch
			{
				".jpg" => newImg.Encode(SKEncodedImageFormat.Jpeg, quality: 85),
				".png" => newImg.Encode(SKEncodedImageFormat.Png, quality: 100),
				_ => throw new ArgumentException("Unsupported file format."),
			};

			data.SaveTo(stream);
		}
		else
		{
			switch (Path.GetExtension(filePath))
			{
				case ".jpg":
					_bitmap.Encode(stream, SKEncodedImageFormat.Jpeg, quality: 85);
					break;
				case ".png":
					_bitmap.Encode(stream, SKEncodedImageFormat.Png, quality: 100);
					break;
				default:
					throw new ArgumentException("Unsupported file format.");
			}
		}
	}

	public void Dispose() => _bitmap.Dispose();
}
