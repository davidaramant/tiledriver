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
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightTracer
    {
        private sealed class LightMap
        {
            private readonly int[,] _map;
            public LightMap(int width, int height) => _map = new int[width, height];
            public void Lighten(Point point, int amount)
            {
                var current = _map[point.Y, point.X];
                _map[point.Y, point.X] = Math.Min(current + amount, 3);
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

        public static void AddRandomLightsToMap(MapData map, Room room)
        {
            const int radius = 4;
            var radius2 = radius * radius;

            const int diameter = 2 * radius + 1;

            var lightMap = new LightMap(map.Height, map.Width);
            var lightSpots = FindValidSpotsForLights(map, room);

            var bounds = new Size(map.Width, map.Height);
            foreach (var lightSpot in lightSpots)
            {
                map.Things.Add(
                    new Thing(
                        type: "Candle",
                        x: lightSpot.X + 0.5,
                        y: lightSpot.Y + 0.5,
                        z: 0,
                        angle: 0,
                        skill1: true,
                        skill2: true,
                        skill3: true,
                        skill4: true));

                for (int y = 0; y < diameter; y++)
                {
                    for (int x = 0; x < diameter; x++)
                    {
                        var tileSpot = new Point(
                            lightSpot.X - radius + x,
                            lightSpot.Y - radius + y);
                        if (!bounds.Contains(tileSpot))
                            continue;

                        // check for line of sight
                        var pointsBetween = BresenhamLine(lightSpot, tileSpot);

                        if (pointsBetween.Any(p => map.TileSpaceAt(p).HasTile))
                        {
                            if (pointsBetween.First(p => map.TileSpaceAt(p).HasTile) != tileSpot)
                            {
                                continue;
                            }
                        }

                        var d2 = GetDistanceSquared(lightSpot, tileSpot);
                        var lightIncrement = PickLightLevelIncrement(radius2, d2);
                        lightMap.Lighten(tileSpot, lightIncrement);
                    }
                }
            }

            foreach (var (level, ts) in lightMap.GetLightLevels().Zip(map.PlaneMaps[0].TileSpaces, (level, ts) => (level, ts)))
            {
                ts.Sector = level;
                if (ts.HasTile)
                {
                    ts.Tile = level;
                }
            }
        }

        // Swap the values of A and B
        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        // Returns the list of points from origin to destination 
        private static List<Point> BresenhamLine(Point origin, Point destination)
        {
            return BresenhamLine(origin.X, origin.Y, destination.X, destination.Y);
        }

        // Returns the list of points from (x0, y0) to (x1, y1)
        private static List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            // Optimization: it would be preferable to calculate in
            // advance the size of "result" and to use a fixed-size array
            // instead of a list.
            List<Point> result = new List<Point>();

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
                if (steep) result.Add(new Point(y, x));
                else result.Add(new Point(x, y));
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }


        private static double GetDistanceSquared(Point p1, Point p2) =>
            (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);

        private static byte PickLightLevelIncrement(int lightRadiusSquared, double distanceSquared)
        {
            if (distanceSquared > lightRadiusSquared)
            {
                return 0;
            }

            if (distanceSquared >= (lightRadiusSquared / 4.0))
            {
                return 1;
            }

            return 2;
        }

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

                var surroundedVertically = IsSurroundedVertically(map, possibleSpot);
                var surroundedHorizontally = IsSurroundedHorizontally(map, possibleSpot);

                if ((surroundedVertically ^ surroundedHorizontally) ||
                    existingThingSpots.Contains(possibleSpot))
                {
                    continue;
                }

                spots.Add(possibleSpot);
            }

            return spots;
        }

        private static bool IsSurroundedVertically(MapData map, Point possibleSpot) =>
            map.TileSpaceAt(possibleSpot.Left()).HasTile && map.TileSpaceAt(possibleSpot.Right()).HasTile;

        private static bool IsSurroundedHorizontally(MapData map, Point possibleSpot) =>
            map.TileSpaceAt(possibleSpot.Above()).HasTile && map.TileSpaceAt(possibleSpot.Below()).HasTile;
    }
}