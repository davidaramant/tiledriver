// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling
{
    public static class ConnectedAreaAnalyzer
    {
        public static IEnumerable<ConnectedArea> FindForegroundAreas(Size dimensions, Func<Position, bool> isForeground)
        {
            int label = 0;
            Dictionary<Position, int> covered = new();
            Queue<(Position P, int Label)> queue = new();

            IEnumerable<Position> GetNeighbors(Position p)
            {
                if (p.X > 0)
                {
                    yield return p + CoordinateSystem.TopLeft.Left;
                }

                if (p.X < dimensions.Width - 1)
                {
                    yield return p + CoordinateSystem.TopLeft.Right;
                }

                if (p.Y > 0)
                {
                    yield return p + CoordinateSystem.TopLeft.Up;
                }

                if (p.Y < dimensions.Height - 1)
                {
                    yield return p + CoordinateSystem.TopLeft.Down;
                }
            }

            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    var p = new Position(x, y);

                    if (!isForeground(p) || covered.ContainsKey(p))
                        continue;

                    queue.Enqueue((p, label));

                    while (queue.Any())
                    {
                        var (qP, currentLabel) = queue.Dequeue();
                        foreach (
                            var neighbor in GetNeighbors(qP).Where(n => isForeground(n) && !covered.ContainsKey(n))
                        )
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

        /// <summary>
        /// Finds the interior contours of the area.
        /// </summary>
        /// <param name="area">The area to analyze.</param>
        /// <param name="neighborhood">What type of neighborhood to segerate contours by.</param>
        /// <returns>
        /// A list of the contours. Each index corresponds to the distance from the outer edge (0 is right next to it, 1 is futher inside, etc).
        /// </returns>
        public static IReadOnlyList<IReadOnlySet<Position>> DetermineInteriorContours(
            this ConnectedArea area,
            Neighborhood neighborhood
        )
        {
            Func<Position, IEnumerable<Position>> getNeighborhood =
                neighborhood == Neighborhood.Moore
                    ? PositionExtensions.GetMooreNeighbors
                    : PositionExtensions.GetVonNeumannNeighbors;

            HashSet<Position> remainingPositions = new(area);
            List<IReadOnlySet<Position>> edges = new();

            // Find outer edge

            var outerEdge = remainingPositions.Where(p => getNeighborhood(p).Any(p => !area.Contains(p))).ToHashSet();
            edges.Add(outerEdge);
            remainingPositions.ExceptWith(outerEdge);

            // Find remaining boundaries

            while (remainingPositions.Any())
            {
                var lastEdge = edges.Last();

                var edge = remainingPositions.Where(p => getNeighborhood(p).Any(lastEdge.Contains)).ToHashSet();
                edges.Add(edge);
                remainingPositions.ExceptWith(edge);
            }

            return edges;
        }

        public static IReadOnlyDictionary<Position, int> DetermineInteriorEdgeDistance(
            this ConnectedArea area,
            Neighborhood neighborhood
        )
        {
            var edges = DetermineInteriorContours(area, neighborhood);

            var output = new Dictionary<Position, int>();
            for (int distance = 0; distance < edges.Count; distance++)
            {
                foreach (var p in edges[distance])
                {
                    output.Add(p, distance);
                }
            }
            return output;
        }
    }
}
