using SkiaSharp;

namespace Tiledriver.Utils.Images;

public interface IFastImage : IDisposable
{
	int Height { get; }
	int PixelCount { get; }
	int Width { get; }

	void Fill(SKColor color);
	void Save(string filePath, int scale = 1);
	void SetPixel(int pixelIndex, SKColor color);
	void SetPixel(int x, int y, SKColor color);
	void SetPixel(SKPointI p, SKColor color);
}

public sealed class FastImage : IFastImage
{
	private readonly SKBitmap _bitmap;

	public int Width { get; }
	public int Height { get; }
	public int PixelCount => Width * Height;

	public FastImage(SKSizeI resolution)
		: this(resolution.Width, resolution.Height) { }

	public FastImage(int width, int height)
	{
		Width = width;
		Height = height;
		_bitmap = new SKBitmap(width, height);
	}

	private FastImage(SKBitmap bitmap)
	{
		Width = bitmap.Width;
		Height = bitmap.Height;
		_bitmap = bitmap;
	}

	public static FastImage WrapSKBitmap(SKBitmap bitmap) => new(bitmap);

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
	/// <param name="scale">The integer scale of the image.</param>
	public void Save(string filePath, int scale = 1)
	{
		using var stream = File.Open(filePath, FileMode.Create);

		if (scale != 1)
		{
			var resizedWidth = scale * Width;
			var resizedHeight = scale * Height;

			using var surface = SKSurface.Create(
				new SKImageInfo
				{
					Width = resizedWidth,
					Height = resizedHeight,
					ColorType = SKImageInfo.PlatformColorType,
					AlphaType = SKAlphaType.Premul,
				}
			);
			using var img = SKImage.FromBitmap(_bitmap);

			surface.Canvas.DrawImage(img, new SKRectI(0, 0, resizedWidth, resizedHeight), SKSamplingOptions.Default);
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
