// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.MapMetadata.Extensions;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Extensions;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightTracer
    {
        public static HashSet<Point> FindValidSpotsForLights(
            MapData map,
            Room room,
            double percentageAreaToCover = 0.015)
        {
            var lightsToPlace = (int)(room.Area * percentageAreaToCover);

            var existingThingSpots = map.Things.Select(t => new Point((int)t.X, (int)t.Y)).ToImmutableHashSet();

            var spots = new HashSet<Point>();

            var random = new Random(0);

            while (spots.Count < lightsToPlace)
            {
                var possibleSpot = room.ElementAt(random.Next(room.Area));

                if (WouldBlockHorizontally(map, possibleSpot) ||
                    WouldBlockVertically(map, possibleSpot) ||
                    existingThingSpots.Contains(possibleSpot))
                {
                    continue;
                }

                spots.Add(possibleSpot);
            }

            return spots;
        }

        private static bool WouldBlockHorizontally(MapData map, Point possibleSpot) =>
            map.TileSpaceAt(possibleSpot.Left()).HasTile && map.TileSpaceAt(possibleSpot.Right()).HasTile;

        private static bool WouldBlockVertically(MapData map, Point possibleSpot) =>
            map.TileSpaceAt(possibleSpot.Above()).HasTile && map.TileSpaceAt(possibleSpot.Below()).HasTile;
    }
}