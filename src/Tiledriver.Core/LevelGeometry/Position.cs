// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry
{
    public readonly record struct Position(int X, int Y)
    {
        public static readonly Position Origin = new(0, 0);

        public static Position operator +(Position p, PositionDelta d) => new(p.X + d.X, p.Y + d.Y);
        public static Position operator -(Position p, PositionDelta d) => new(p.X - d.X, p.Y - d.Y);
        public static PositionDelta operator -(Position p1, Position p2) => new(p1.X - p2.X, p1.Y - p2.Y);

        public static Position operator +(Position p, int scalar) => new(p.X + scalar, p.Y + scalar);
        public static Position operator -(Position p, int scalar) => new(p.X - scalar, p.Y - scalar);

        public int DistanceSquared(Position other) => (this - other).MagnitudeSquared();
    }
}