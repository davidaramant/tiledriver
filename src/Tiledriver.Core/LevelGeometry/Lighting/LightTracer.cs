// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;
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

                        var location = light.Center + delta;

                        if (!map.Dimensions.Contains(location))
                            continue;

                        // check for line of sight
                        var obscured =
                            DrawingUtil.BresenhamLine(
                                    start: light.Center,
                                    end: location)
                                .Any(p => board[p].HasTile);

                        if (!obscured)
                        {
                            var increment = light.GetBrightness(location);

                            floorLight.Lighten(location, increment.Floor);
                            ceilingLight.Lighten(location, increment.Ceiling);
                        }
                    }
                }
            }

            return (floorLight, ceilingLight);
        }
    }
}