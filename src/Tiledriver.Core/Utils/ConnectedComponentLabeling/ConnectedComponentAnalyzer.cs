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
                if (spot.X < dimensions.Width - 1 && map[spot.Y, spot.X + 1] != currentComponentId && map[spot.Y, spot.X + 1] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Right());
                }
                // top
                if (spot.Y > 0 && map[spot.Y - 1, spot.X] != currentComponentId && map[spot.Y - 1, spot.X] != 0)
                {
                    currentComponentId = JoinAreas(spot, spot.Above());
                }
                // bottom
                if (spot.Y < dimensions.Height - 1 && map[spot.Y + 1, spot.X] != currentComponentId && map[spot.Y + 1, spot.X] != 0)
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
    }
}
