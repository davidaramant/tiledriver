using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tiledriver.Generator.SimpleGeometry
{
    public enum RoomType
    {
        Room,
        Hallway,
    }

    public sealed class RoomNode
    {
        public Rectangle Bounds { get; }
        public RoomType Type { get; }

        public RoomNode(Rectangle bounds, RoomType type)
        {
            Bounds = bounds;
            Type = type;
        }

        public Point GetStartingPointFacing(Direction direction, Random random)
        {
            switch (direction)
            {
                case Direction.East:
                    return new Point(x: Bounds.Right, y: random.Next(Bounds.Top, Bounds.Bottom + 1));

                case Direction.North:
                    return new Point(x: random.Next(Bounds.Left, Bounds.Right + 1), y: Bounds.Top);

                case Direction.West:
                    return new Point(x: Bounds.Left, y: random.Next(Bounds.Top, Bounds.Bottom + 1));

                case Direction.South:
                    return new Point(x: random.Next(Bounds.Left, Bounds.Right + 1), y: Bounds.Bottom);

                default:
                    throw new InvalidOperationException("Can't handle that direction.");
            }
        }
    }
}
