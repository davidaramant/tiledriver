// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry
{
    public sealed record Size(int Width, int Height)
    {
        public int Area => Width * Height;

        public Rectangle ToRectangle() => new(new Position(0, 0), this);
        public bool Contains(Position p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

        public static Size operator *(Size size, int scale) => new(size.Width * scale, size.Height * scale);

        public IEnumerable<Position> GetAllPositions()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return new Position(x, y);
                }
            }
        }
    }
}