﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Tiledriver.Generator.SimpleGeometry
{
    public enum RoomType
    {
        Room,
        Hallway,
    }

    [DebuggerDisplay("Bounds: {Bounds}, Type: {Type}")]
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
                    return new Point(x: Bounds.RightEdge(), y: random.Next(Bounds.TopEdge() + 1, Bounds.BottomEdge()));

                case Direction.North:
                    return new Point(x: random.Next(Bounds.LeftEdge() + 1, Bounds.RightEdge()), y: Bounds.TopEdge());

                case Direction.West:
                    return new Point(x: Bounds.LeftEdge(), y: random.Next(Bounds.TopEdge(), Bounds.BottomEdge()));

                case Direction.South:
                    return new Point(x: random.Next(Bounds.LeftEdge() + 1, Bounds.RightEdge()), y: Bounds.BottomEdge());

                default:
                    throw new InvalidOperationException("Can't handle that direction.");
            }
        }
    }
}