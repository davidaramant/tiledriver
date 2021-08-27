// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;
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

                    if (!isForeground(p) || covered.ContainsKey(p))
                        continue;

                    queue.Enqueue((p, label));

                    while (queue.Any())
                    {
                        var (qP, currentLabel) = queue.Dequeue();
                        foreach (var neighbor in GetNeighbors(qP)
                            .Where(n => isForeground(n) && !covered.ContainsKey(n)))
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

        public static IReadOnlyDictionary<Position, int> DetermineDistanceToEdges(
            this ConnectedArea area,
            Neighborhood neighborhood) =>
            DetermineDistanceToEdges(area,
                neighborhood == Neighborhood.Moore
                    ? PositionExtensions.GetMooreNeighbors
                    : PositionExtensions.GetVonNeumannNeighbors);

        private static IReadOnlyDictionary<Position, int> DetermineDistanceToEdges(
            ConnectedArea area,
            Func<Position, IEnumerable<Position>> getNeighborhood)
        {
            LinkedList<Position> remainingPositions = new(area);
            Dictionary<Position, int> posToDistance = new();

            int? GetDistance(Position p)
            {
                if (posToDistance.TryGetValue(p, out int distance))
                    return distance;

                return !area.Contains(p) ? -1 : null;
            }

            bool IsTouchingDistance(Position p, int distance) =>
                getNeighborhood(p).Select(GetDistance).Any(d => d == distance);

            int distance = -1;
            while (remainingPositions.Any())
            {
                var posNode = remainingPositions.First;

                while (posNode != null)
                {
                    if (IsTouchingDistance(posNode.Value, distance))
                    {
                        posToDistance.Add(posNode.Value, distance + 1);
                        var next = posNode.Next;
                        remainingPositions.Remove(posNode);
                        posNode = next;
                    }
                    else
                    {
                        posNode = posNode.Next;
                    }
                }

                distance++;
            }

            return posToDistance;
        }
    }
}