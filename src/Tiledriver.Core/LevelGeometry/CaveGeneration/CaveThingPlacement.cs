// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public static class CaveThingPlacement
    {
        public static IEnumerable<LightDefinition> RandomlyPlaceLights(
            ConnectedArea area,
            Random random, LightRange lightRange,
            double percentAreaToCover)
        {
            var numLights = (int)(area.Area * percentAreaToCover);

            var positions = new List<Position>();

            return Enumerable
                .Range(0, numLights)
                .Select(_ =>
                {
                    while (true)
                    {
                        var posIndex = random.Next(0, area.Area);
                        var position = area.ElementAt(posIndex);

                        var positionIsInvalid =
                            positions.Any(p => p.Touches(position));

                        if (positionIsInvalid)
                            continue;

                        positions.Add(position);

                        return new LightDefinition(
                            position,
                            Brightness: (int)(lightRange.LightLevels * 1.25),
                            Radius: 15);
                    }
                });
        }

        public static IEnumerable<(Position Location, TreasureType Type)> RandomlyPlaceTreasure(
            ConnectedArea area,
            HashSet<Position> edge,
            LightMap floorLighting,
            LightRange lightRange,
            Random random)
        {
            var maxScore = 3 * lightRange.DarkLevels;
            var cutOff = (int)(0.5 * maxScore);

            var scoredLocations = edge
                    .Select(p => new { Location = p, Score = area.CountAdjacentWalls(p) * -floorLighting[p] })
                    .OrderBy(scoredLocation => scoredLocation.Score)
                    .Where(scoredLocation => scoredLocation.Score >= cutOff)
                    .OrderBy(sl => random.Next())
                    .ToArray();

            var numberToTake = scoredLocations.Length / 2;

            return scoredLocations.Take(numberToTake).Select(sl => (sl.Location, TreasureType.Medium));
        }
    }
}
