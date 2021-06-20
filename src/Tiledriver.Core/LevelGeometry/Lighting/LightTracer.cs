// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Utils;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightTracer
    {
        public static (LightMap FloorLight, LightMap CeilingLight) Trace(
            MapData map,
            LightRange lightRange,
            IEnumerable<LightDefinition> lights)
        {
            var floorLight = new LightMap(lightRange, map.Dimensions).Blackout();
            var ceilingLight = new LightMap(lightRange, map.Dimensions).Blackout();

            var board = map.GetBoard();

            foreach (var light in lights)
            {
                // Check a big square around the light. This is very brute force but it doesn't appear to cause any
                // problems.
                for (int y = 0; y < light.LengthAffected; y++)
                {
                    for (int x = 0; x < light.LengthAffected; x++)
                    {
                        var delta = new PositionDelta(x, y) - light.Radius;

                        var location = light.Location + delta;

                        if (!map.Dimensions.Contains(location))
                            continue;

                        // check for line of sight
                        var obscured =
                            DrawingUtil.BresenhamLine(
                                    start: light.Location,
                                    end: location)
                                .Any(p => board[p].HasTile);

                        if (!obscured)
                        {
                            var d2 = GetDistanceSquared(light.Location, location);
                            var lightIncrement = PickLightLevelIncrement(light.Radius, d2);
                            floorLight.Lighten(location, lightIncrement);
                            ceilingLight.Lighten(location, lightIncrement);
                        }
                    }
                }
            }

            return (floorLight, ceilingLight);
        }


        public const int LightLevels = 30;
        public const int Overbrights = 15;
        public const int NormalLightLevels = LightLevels - Overbrights;


        public static void AddRandomLightsToMap(
            MapData map,
            Room room,
            int lightRadius = 4,
            double percentAreaToCoverWithLights = 0.015)
        {
            map.Sectors.Clear();
            map.Sectors.AddRange(Enumerable.Range(0, LightTracer.LightLevels).Select(level => new Sector
            (
                TextureCeiling: $"bf{level}",
                TextureFloor: $"bf{level}"
            )));

            int diameter = 2 * lightRadius + 1;

            var lightMap = new LightMap(new LightRange(15, 15), map.Dimensions);
            var lightSpots = FindValidSpotsForLights(map, room, percentageAreaToCover: percentAreaToCoverWithLights);

            var bounds = new Size(map.Width, map.Height);
            foreach (var lightSpot in lightSpots)
            {
                map.Things.Add(
                    new Thing(
                        Type: "Candle",
                        X: lightSpot.X + 0.5,
                        Y: lightSpot.Y + 0.5,
                        Z: 0,
                        Angle: 0,
                        Skill1: true,
                        Skill2: true,
                        Skill3: true,
                        Skill4: true));

                for (int y = 0; y < diameter; y++)
                {
                    for (int x = 0; x < diameter; x++)
                    {
                        var tileSpot = new Position(
                            lightSpot.X - lightRadius + x,
                            lightSpot.Y - lightRadius + y);

                        throw new NotImplementedException();
                        //if (!bounds.Contains(tileSpot))
                        //    continue;

                        // check for line of sight
                        var obscured = false;
                        foreach (var pointInBetween in DrawingUtil.BresenhamLine(start: lightSpot,
                            end: tileSpot))
                        {
                            throw new NotImplementedException();

                            // if (map.TileSpaceAt(pointInBetween).HasTile && pointInBetween != tileSpot)
                            {
                                obscured = true;
                                break; // TODO: This is wrong - the order is not defined
                            }
                        }

                        if (!obscured)
                        {
                            var d2 = GetDistanceSquared(lightSpot, tileSpot);
                            var lightIncrement = PickLightLevelIncrement(lightRadius, d2);
                            lightMap.Lighten(tileSpot, lightIncrement);
                        }
                    }
                }
            }

            foreach (var (level, ts) in lightMap.GetLightLevels().Zip(map.PlaneMaps[0], (level, ts) => (level, ts)))
            {
                throw new NotImplementedException();
                //ts.Sector = level;
            }

            int GetNeighborLevel(Position point)
            {
                throw new NotImplementedException();
                //if (!bounds.Contains(point))
                //    return 0;
                return lightMap[point];
            }

            var tileSequence = new TileSequence();
            for (var row = 0; row < map.Height; row++)
            {
                for (var col = 0; col < map.Width; col++)
                {
                    var tileSpot = new Position(col, row);
                    throw new NotImplementedException();
                    //var ts = map.TileSpaceAt(tileSpot);
                    //if (!ts.HasTile)
                    //    continue;

                    //var neighbors = new NeighborLevels(
                    //    north: GetNeighborLevel(tileSpot.Above()),
                    //    east: GetNeighborLevel(tileSpot.Right()),
                    //    south: GetNeighborLevel(tileSpot.Below()),
                    //    west: GetNeighborLevel(tileSpot.Left())
                    //);

                    //ts.Tile = tileSequence.GetTileIndex(neighbors);
                }
            }

            map.Tiles.Clear();
            map.Tiles.AddRange(tileSequence.GetTileDefinitions());
        }

        struct NeighborLevels
        {
            public readonly int North;
            public readonly int East;
            public readonly int South;
            public readonly int West;

            public NeighborLevels(int north, int east, int south, int west)
            {
                North = north;
                East = east;
                South = south;
                West = west;
            }
        }

        sealed class TileSequence
        {
            private readonly Dictionary<NeighborLevels, int> _levelComboToIndex = new Dictionary<NeighborLevels, int>();

            public int GetTileIndex(NeighborLevels neighbors)
            {
                if (_levelComboToIndex.TryGetValue(neighbors, out int index))
                {
                    return index;
                }

                var nextIndex = _levelComboToIndex.Count;
                _levelComboToIndex.Add(neighbors, nextIndex);
                return nextIndex;
            }

            public IEnumerable<Tile> GetTileDefinitions() =>
                _levelComboToIndex.OrderBy(pair => pair.Value).Select(pair =>
                    new Tile(
                        TextureNorth: $"bwa{pair.Key.North}",
                        TextureSouth: $"bwa{pair.Key.South}",
                        TextureEast: $"bwb{pair.Key.East}",
                        TextureWest: $"bwb{pair.Key.West}"));
        }

        private static double GetDistanceSquared(Position p1, Position p2) =>
            (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

        private static int PickLightLevelIncrement(int radius, double distanceSquared)
        {
            var distanceStep = (double) radius / NormalLightLevels;
            for (int level = 0; level < NormalLightLevels; level++)
            {
                var minDistance = radius - level * distanceStep;

                if (distanceSquared >= minDistance * minDistance)
                {
                    return level;
                }
            }

            return NormalLightLevels;
        }

        public static HashSet<Position> FindValidSpotsForLights(
            MapData map,
            Room room,
            double percentageAreaToCover = 0.015)
        {
            var lightsToPlace = (int) (room.Area * percentageAreaToCover);

            var existingThingSpots = map.Things.Select(t => new Position((int) t.X, (int) t.Y)).ToImmutableHashSet();

            var spots = new HashSet<Position>();
            var random = new Random(0);

            while (spots.Count < lightsToPlace)
            {
                var possibleSpot = room.ElementAt(random.Next(room.Area));

                // HACK: The light doesn't block, so this isn't needed
                var surroundedVertically = false; //IsSurroundedVertically(map, possibleSpot);
                var surroundedHorizontally = false; //IsSurroundedHorizontally(map, possibleSpot);

                if ((surroundedVertically ^ surroundedHorizontally) ||
                    existingThingSpots.Contains(possibleSpot))
                {
                    continue;
                }

                spots.Add(possibleSpot);
            }

            return spots;
        }
    }
}