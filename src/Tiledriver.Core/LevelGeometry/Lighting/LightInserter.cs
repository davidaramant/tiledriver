// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public static class LightInserter
    {
        public static IEnumerable<LightDefinition> RandomlyPlaceLights(
            LightRange lightRange, 
            double percentAreaToCover,
            ConnectedArea area,
            Random random)
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
                            Brightness: (int) (lightRange.LightLevels * 1.25),
                            Radius: 15);
                    }
                });
        }
    }
}
