// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.Extensions
{
    public static class PositionExtensions
    {
        public static Position Right(this Position p) => new(p.X + 1, p.Y);
        public static Position Left(this Position p) => new(p.X - 1, p.Y);
        public static Position Below(this Position p) => new(p.X, p.Y + 1);
        public static Position Above(this Position p) => new(p.X, p.Y - 1);

        public static bool Touches(this Position p, Position other) =>
            (p.X == other.X && (p.Y == other.Y - 1 || p.Y == other.Y + 1)) ||
            (p.Y == other.Y && (p.X == other.X - 1 || p.X == other.X + 1));
    }
}
