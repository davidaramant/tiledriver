// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Extensions.Directions
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
        public static IEnumerable<(Direction direction, Position point)> GetAdjacentPoints(
            this Position origin,
            Size bounds,
            bool clockWise = true,
            Direction start = Direction.North)
        {
            foreach (var direction in GetDirections(start, clockWise))
            {
                switch (direction)
                {
                    case Direction.East:
                        if (origin.X < bounds.Width - 1)
                            yield return (Direction.East, new Position(origin.X + 1, origin.Y));
                        break;

                    case Direction.North:
                        if (origin.Y > 0)
                            yield return (Direction.North, new Position(origin.X, origin.Y - 1));
                        break;

                    case Direction.West:
                        if (origin.X > 0)
                            yield return (Direction.West, new Position(origin.X - 1, origin.Y));
                        break;

                    case Direction.South:
                        if (origin.Y < bounds.Height - 1)
                            yield return (Direction.South, new Position(origin.X, origin.Y + 1));
                        break;

                    default:
                        throw new InvalidProgramException("Unknown direction");
                }
            }
        }

        public static Direction Reverse(this Direction direction) => 
            direction switch
            {
                Direction.East => Direction.West,
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                _ => throw new ArgumentException("Unknown direction: " + direction)
            };

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
