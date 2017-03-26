// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Drawing;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.Extensions
{
    public static class DirectionExtensions
    {
        /// <summary>
        /// Returns valid adjacent points.
        /// </summary>
        /// <param name="origin">The starting point.</param>
        /// <param name="bounds">The maximum size to clip things to.</param>
        /// <param name="clockWise">The direction to return the points in.</param>
        /// <param name="start">Which direction to start with.</param>
        /// <returns>The direction relative to <paramref name="origin"/> and the adjacent location.</returns>
        public static IEnumerable<(Direction direction, Point point)> GetAdjacentPoints(
            this Point origin,
            Size bounds,
            bool clockWise,
            Direction start)
        {
            foreach (var direction in GetDirections(start, clockWise))
            {
                switch (direction)
                {
                    case Direction.East:
                        if (origin.X < bounds.Width - 1)
                            yield return (Direction.East, new Point(origin.X + 1, origin.Y));
                        break;

                    case Direction.North:
                        if (origin.Y > 0)
                            yield return (Direction.North, new Point(origin.X, origin.Y - 1));
                        break;

                    case Direction.West:
                        if (origin.X > 0)
                            yield return (Direction.West, new Point(origin.X - 1, origin.Y));
                        break;

                    case Direction.South:
                        if (origin.Y < bounds.Height - 1)
                            yield return (Direction.South, new Point(origin.X, origin.Y + 1));
                        break;
                }
            }
        }

        public static Direction Reverse(this Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    return Direction.West;
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    throw new ArgumentException("Unknown direction: " + direction);
            }
        }

        public static IEnumerable<Direction> GetDirections(Direction start, bool clockWise)
        {
            var directions = new[] { Direction.East, Direction.North, Direction.West, Direction.South, };

            for (int i = 0; i < 4; i++)
            {
                var d = directions[((int)start + i) % 4];
                if (i % 2 == 1 && clockWise)
                    yield return d.Reverse();
                else
                    yield return d;
            }
        }
    }
}
