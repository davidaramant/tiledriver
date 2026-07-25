using SkiaSharp;
using Tiledriver.Rendering;

namespace Tiledriver.Utils.Images;

public interface IFastImage : IPixelBuffer, IDisposable { }

public sealed class FastImage : IFastImage
{
	private readonly SKBitmap _bitmap;

	public SKSizeI Dimensions { get; }
	public int Width => Dimensions.Width;
	public int Height => Dimensions.Height;

	public SKColor this[SKPointI p] => this[p.X, p.Y];
	public SKColor this[int x, int y] => _bitmap.GetPixel(x, y);

	public FastImage(SKSizeI resolution)
	{
		Dimensions = resolution;
		_bitmap = new SKBitmap(resolution.Width, resolution.Height);
	}

	public FastImage(int width, int height)
		: this(new SKSizeI(width, height)) { }

	private FastImage(SKBitmap bitmap)
	{
		Dimensions = new SKSizeI(bitmap.Width, bitmap.Height);
		_bitmap = bitmap;
	}

	public static FastImage WrapSKBitmap(SKBitmap bitmap) => new(bitmap);

	public void Fill(SKColor color)
	{
		using var canvas = new SKCanvas(_bitmap);
		canvas.Clear(color);
	}

	public void Fill(SKColor color, SKRectI area)
	{
		using var canvas = new SKCanvas(_bitmap);
		using var paint = new SKPaint();
		paint.Color = color;
		canvas.DrawRect(area, paint);
	}

	public void SetColor(SKPointI p, SKColor color) => SetColor(p.X, p.Y, color);

	public void SetColor(int x, int y, SKColor color) => _bitmap.SetPixel(x, y, color);

	public void SetColor(int pixelIndex, SKColor color)
	{
		var x = pixelIndex % Width;
		var y = pixelIndex / Width;

		SetColor(x, y, color);
	}

	public void AddColor(SKPointI p, SKColor color) => AddColor(p.X, p.Y, color);

	public void AddColor(int x, int y, SKColor color)
	{
		var current = this[x, y];

		SetColor(
			x,
			y,
			new SKColor(
				(byte)Math.Min(current.Red + color.Red, byte.MaxValue),
				(byte)Math.Min(current.Green + color.Green, byte.MaxValue),
				(byte)Math.Min(current.Blue + color.Blue, byte.MaxValue),
				current.Alpha
			)
		);
	}

	public void AddColor(int pixelIndex, SKColor color)
	{
		var x = pixelIndex % Width;
		var y = pixelIndex / Width;

		AddColor(x, y, color);
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
