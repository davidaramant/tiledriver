// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry
{
    public sealed record PositionDelta(int X, int Y)
    {
        public static PositionDelta operator +(PositionDelta d1, PositionDelta d2) => new(d1.X + d2.X, d1.Y + d2.Y);
        public static PositionDelta operator -(PositionDelta d1, PositionDelta d2) => new(d1.X - d2.X, d1.Y - d2.Y);
        
        public static PositionDelta operator +(PositionDelta d, int scalar) => new(d.X + scalar, d.Y + scalar);
        public static PositionDelta operator -(PositionDelta d, int scalar) => new(d.X - scalar, d.Y - scalar);

        public static readonly PositionDelta North = new(0, -1);
        public static readonly PositionDelta South = new(0, 1);
        public static readonly PositionDelta West = new(-1, 0);
        public static readonly PositionDelta East = new(1, 0);

        public static PositionDelta Up => North;
        public static PositionDelta Down => South;
        public static PositionDelta Left => West;
        public static PositionDelta Right => East;
    }
}