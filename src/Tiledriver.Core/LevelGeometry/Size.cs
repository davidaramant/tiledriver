// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry
{
    public sealed record Size(int Width, int Height)
    {
        public int Area => Width * Height;

        public Rectangle ToRectangle() => new(new Position(0, 0), this);
    }
}