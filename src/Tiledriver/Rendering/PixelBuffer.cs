using SkiaSharp;
using Tiledriver.Extensions.Skia;

namespace Tiledriver.Rendering;

public interface IPixelBuffer
{
	int Height { get; }
	int Width { get; }
	SKSizeI Dimensions { get; }

	SKColor this[SKPointI p] { get; }
	SKColor this[int x, int y] { get; }

	void Fill(SKColor color);
	void Fill(SKColor color, SKRectI area);

	void SetColor(int pixelIndex, SKColor color);
	void SetColor(int x, int y, SKColor c);
	void SetColor(SKPointI p, SKColor color);

	void AddColor(int pixelIndex, SKColor color);
	void AddColor(int x, int y, SKColor color);
	void AddColor(SKPointI p, SKColor color);

	void Save(string filePath, int scale = 1);
}

public sealed class PixelBuffer : IPixelBuffer
{
	readonly SKColor[] _buffer;

	public PixelBuffer(int width, int height)
		: this(new SKSizeI(width, height)) { }

	public PixelBuffer(SKSizeI size)
		: this(size, new SKColor[size.Area()]) { }

	private PixelBuffer(SKSizeI size, SKColor[] buffer)
	{
		Dimensions = size;
		_buffer = buffer;
	}

	public SKSizeI Dimensions { get; }
	public int Width => Dimensions.Width;
	public int Height => Dimensions.Height;

	// Not on the interface. This is needed to copy the pixels directly to the output texture.
	public SKColor[] Pixels => _buffer;

	public SKColor this[SKPointI p] => _buffer[p.Y * Width + p.X];
	public SKColor this[int x, int y] => _buffer[y * Width + x];

	public void SetColor(int pixelIndex, SKColor color)
	{
		var x = pixelIndex % Width;
		var y = pixelIndex / Width;

		SetColor(x, y, color);
	}

	public void SetColor(SKPointI p, SKColor color) => SetColor(p.X, p.Y, color);

	public void SetColor(int x, int y, SKColor c)
	{
		if (x >= 0 && x < Width && y >= 0 && y < Height)
		{
			_buffer[y * Width + x] = c;
		}
	}

	public void AddColor(int pixelIndex, SKColor color)
	{
		var x = pixelIndex % Width;
		var y = pixelIndex / Width;

		AddColor(x, y, color);
	}

	public void AddColor(SKPointI p, SKColor color) => AddColor(p.X, p.Y, color);

	public void AddColor(int x, int y, SKColor c)
	{
		if (x >= 0 && x < Width && y >= 0 && y < Height)
		{
			ref SKColor current = ref _buffer[y * Width + x];

			current = new SKColor(
				(byte)Math.Min(current.Red + c.Red, 255),
				(byte)Math.Min(current.Green + c.Green, 255),
				(byte)Math.Min(current.Blue + c.Blue, 255)
			);
		}
	}

	public void Fill(SKColor color) => Array.Fill(_buffer, color);

	public void Fill(SKColor color, SKRectI area)
	{
		for (int row = 0; row < area.Height; row++)
		{
			Array.Fill(_buffer, color, (row + area.Top) * Width + area.Left, area.Width);
		}
	}

	public void Save(string filePath, int scale = 1)
	{
		using var bitmap = new SKBitmap(Width, Height);
		bitmap.Pixels = _buffer;
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
			using var image = SKImage.FromBitmap(bitmap);

			surface.Canvas.DrawImage(image, new SKRectI(0, 0, resizedWidth, resizedHeight), SKSamplingOptions.Default);
			surface.Canvas.Flush();

			using var resizedImage = surface.Snapshot();
			using var data = Path.GetExtension(filePath).ToLowerInvariant() switch
			{
				".jpg" => resizedImage.Encode(SKEncodedImageFormat.Jpeg, quality: 85),
				".png" => resizedImage.Encode(SKEncodedImageFormat.Png, quality: 100),
				_ => throw new ArgumentException("Unsupported file format."),
			};

			data.SaveTo(stream);
		}
		else
		{
			switch (Path.GetExtension(filePath))
			{
				case ".jpg":
					bitmap.Encode(stream, SKEncodedImageFormat.Jpeg, quality: 85);
					break;
				case ".png":
					bitmap.Encode(stream, SKEncodedImageFormat.Png, quality: 100);
					break;
				default:
					throw new ArgumentException("Unsupported file format.");
			}
		}
	}

	public void CopyFrom(SKColor[] texture, SKSizeI textureSize, SKPointI destination)
	{
		var xMargin = Width - destination.X;
		var xToCopy = Math.Min(xMargin, textureSize.Width);

		var yMargin = Height - destination.Y;
		var yToCopy = Math.Min(yMargin, textureSize.Height);

		for (int y = 0; y < yToCopy; y++)
		{
			Array.Copy(
				sourceArray: texture,
				sourceIndex: y * textureSize.Width,
				destinationArray: _buffer,
				destinationIndex: (destination.Y + y) * Width + destination.X,
				length: xToCopy
			);
		}
	}
}
