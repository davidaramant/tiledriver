// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling
{
    public static class ConnectedComponentAnalyzer
    {
        public static IEnumerable<ConnectedArea> FindEmptyAreas(Size dimensions, Func<Position, bool> isEmpty)
        {
            var map = new int[dimensions.Height, dimensions.Width];
            int roomCount = 0;

            var emptySpaces = GetAllEmptySpaces(dimensions, isEmpty).ToArray();

            foreach (var emptySpot in emptySpaces)
            {
                map[emptySpot.Y, emptySpot.X] = ++roomCount;
            }

            void ReplaceAll(int oldId, int newId)
            {
                foreach (var p in GetAllSpacesWithId(dimensions, map, oldId))
                {
                    map[p.Y, p.X] = newId;
                }
            }

            foreach (var spot in emptySpaces)
            {
                var currentComponentId = map[spot.Y, spot.X];

                int JoinAreas(Position p1, Position p2)
                {
                    var id1 = map[p1.Y, p1.X];
                    var id2 = map[p2.Y, p2.X];

                    ReplaceAll(Math.Max(id1, id2), Math.Min(id1, id2));

                    return Math.Min(id1, id2);
                }

                // left
                if (spot.X > 0 && map[spot.Y, spot.X - 1] != currentComponentId && map[spot.Y, spot.X - 1] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Left());
                }

                // right
                if (spot.X < dimensions.Width - 1 && map[spot.Y, spot.X + 1] != currentComponentId &&
                    map[spot.Y, spot.X + 1] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Right());
                }

                // top
                if (spot.Y > 0 && map[spot.Y - 1, spot.X] != currentComponentId && map[spot.Y - 1, spot.X] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Above());
                }

                // bottom
                if (spot.Y < dimensions.Height - 1 && map[spot.Y + 1, spot.X] != currentComponentId &&
                    map[spot.Y + 1, spot.X] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Below());
                }
            }

            var componentIds =
                dimensions
                    .GetAllPositions()
                    .Select(p => map[p.Y, p.X])
                    .Where(id => id > 0)
                    .Distinct();

            return componentIds.Select(id => new ConnectedArea(GetAllSpacesWithId(dimensions, map, id)));
        }

        private static IEnumerable<Position> GetAllEmptySpaces(Size dimensions, Func<Position, bool> isEmpty) =>
            dimensions.GetAllPositions().Where(isEmpty);

        private static IEnumerable<Position> GetAllSpacesWithId(Size dimensions, int[,] map, int id) =>
            dimensions.GetAllPositions().Where(p => map[p.Y, p.X] == id);

        public static IEnumerable<ConnectedArea> FindEmptyAreas2(Size dimensions, Func<Position, bool> isEmpty)
        {
            int label = 0;
            Dictionary<Position, int> covered = new();
            Queue<(Position P, int Label)> queue = new();

            IEnumerable<Position> GetNeighbors(Position p)
            {
                if (p.X > 0)
                {
                    yield return p.Left();
                }

                if (p.X < dimensions.Width - 1)
                {
                    yield return p.Right();
                }

                if (p.Y > 0)
                {
                    yield return p.Above();
                }

                if (p.Y < dimensions.Height - 1)
                {
                    yield return p.Below();
                }
            }

            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    var p = new Position(x, y);

                    if (!isEmpty(p) || covered.ContainsKey(p))
                        continue;

                    queue.Enqueue((p, label));

                    while (queue.Any())
                    {
                        var (qP, currentLabel) = queue.Dequeue();
                        foreach (var neighbor in GetNeighbors(qP).Where(n => isEmpty(n) && !covered.ContainsKey(n)))
                        {
                            covered.Add(neighbor, currentLabel);
                            queue.Enqueue((neighbor, currentLabel));
                        }
                    }

                    label++;
                }
            }

            var rawAreas = new List<Position>[label];
            for (int i = 0; i < label; i++)
            {
                rawAreas[i] = new List<Position>();
            }

            foreach (var pair in covered)
            {
                rawAreas[pair.Value].Add(pair.Key);
            }

            return rawAreas.Select(points => new ConnectedArea(points));
        }
    }
}