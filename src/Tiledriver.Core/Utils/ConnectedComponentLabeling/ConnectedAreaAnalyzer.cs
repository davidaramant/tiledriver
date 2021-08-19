// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling
{
    public static class ConnectedAreaAnalyzer
    {
        public static IReadOnlyDictionary<Position, int> DetermineDistanceToEdges(this ConnectedArea area, Neighborhood neighborhood)
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
                p.GetNeighbors(neighborhood).Select(GetDistance).Any(d => d == distance);

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