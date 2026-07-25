using System.Drawing;
using Tiledriver.Extensions.Drawing;

namespace Tiledriver.Rendering;

public interface IPixelBuffer
{
	Color this[Point p] { get; }
	Color this[int x, int y] { get; }

	Size Dimensions { get; }
	int Height { get; }
	int Width { get; }

	void Clear();
	void Clear(Rectangle area);

	void SetColor(int x, int y, Color c);
	void AddColor(int x, int y, Color c);
}

public sealed class PixelBuffer : IPixelBuffer
{
	readonly Color[] _buffer;

	public PixelBuffer(Size size)
		: this(size, new Color[size.Area()]) { }

	private PixelBuffer(Size size, Color[] buffer)
	{
		Dimensions = size;
		_buffer = buffer;
	}

	public Size Dimensions { get; }
	public int Width => Dimensions.Width;
	public int Height => Dimensions.Height;

	public Color this[Point p] => _buffer[p.Y * Width + p.X];
	public Color this[int x, int y] => _buffer[y * Width + x];

	public void SetColor(int x, int y, Color c)
	{
		if (x >= 0 && x < Width && y >= 0 && y < Height)
		{
			_buffer[y * Width + x] = c;
		}
	}

	public void AddColor(int x, int y, Color c)
	{
		if (x >= 0 && x < Width && y >= 0 && y < Height)
		{
			ref Color current = ref _buffer[y * Width + x];

			current = Color.FromArgb(
				Math.Min(current.R + c.R, 255),
				Math.Min(current.G + c.G, 255),
				Math.Min(current.B + c.B, 255)
			);
		}
	}

	public void Clear() => Array.Clear(_buffer, 0, _buffer.Length);

	public void Clear(Rectangle area)
	{
		for (int row = 0; row < area.Height; row++)
		{
			Array.Clear(_buffer, (row + area.Y) * Width + area.X, area.Width);
		}
	}

	public void CopyFrom(Color[] texture, Point textureSize, Point destination)
	{
		var xMargin = Width - destination.X;
		var xToCopy = Math.Min(xMargin, textureSize.X);

		var yMargin = Height - destination.Y;
		var yToCopy = Math.Min(yMargin, textureSize.Y);

		for (int y = 0; y < yToCopy; y++)
		{
			Array.Copy(
				sourceArray: texture,
				sourceIndex: y * textureSize.X,
				destinationArray: _buffer,
				destinationIndex: (destination.Y + y) * Width + destination.X,
				length: xToCopy
			);
		}
	}
}
