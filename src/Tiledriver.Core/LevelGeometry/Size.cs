// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry;

public sealed record Size(int Width, int Height) : IBox
{
	public int Area => Width * Height;

	public Rectangle ToRectangle() => new(new Position(0, 0), this);

	public bool Contains(Position p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

	public static Size operator *(Size size, int scale) => new(size.Width * scale, size.Height * scale);
}
