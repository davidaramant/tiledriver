// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.MapMetadata;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightTracer
    {
        public const int LightLevels = 30;
        public const int Overbrights = 15;
        public const int NormalLightLevels = LightLevels - Overbrights;

        private sealed class LightMap
        {
            private readonly int[,] _map;
            public LightMap(int width, int height) => _map = new int[width, height];
            public int this[int row, int col] => _map[row, col];
            public int this[Position p] => _map[p.Y, p.X];
            public void Lighten(Position point, int amount)
            {
                var current = _map[point.Y, point.X];
                _map[point.Y, point.X] = Math.Min(current + amount, LightLevels - 1);
            }

            public IEnumerable<int> GetLightLevels()
            {
                for (int y = 0; y < _map.GetLength(0); y++)
                {
                    for (int x = 0; x < _map.GetLength(1); x++)
                    {
                        yield return _map[y, x];
                    }
                }
            }
        }

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

            var lightMap = new LightMap(map.Height, map.Width);
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
                        foreach (var pointInBetween in BresenhamLine(origin: lightSpot, destination: tileSpot))
                        {
                            throw new NotImplementedException();

                            // if (map.TileSpaceAt(pointInBetween).HasTile && pointInBetween != tileSpot)
                            {
                                obscured = true;
                                break;
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

        // Swap the values of A and B
        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        // Returns the list of points from origin to destination
        private static IEnumerable<Position> BresenhamLine(Position origin, Position destination) =>
            BresenhamLine(origin.X, origin.Y, destination.X, destination.Y);

        // Returns the list of points from (x0, y0) to (x1, y1)
        private static IEnumerable<Position> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                    yield return new Position(y, x);
                else
                    yield return new Position(x, y);

                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }
        }


        private static double GetDistanceSquared(Position p1, Position p2) =>
            (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

        private static int PickLightLevelIncrement(int radius, double distanceSquared)
        {
            var distanceStep = (double)radius / NormalLightLevels;
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
            var lightsToPlace = (int)(room.Area * percentageAreaToCover);

            var existingThingSpots = map.Things.Select(t => new Position((int)t.X, (int)t.Y)).ToImmutableHashSet();

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