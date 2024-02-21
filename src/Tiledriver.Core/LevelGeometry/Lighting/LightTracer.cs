// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
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
			IEnumerable<LightDefinition> lights
		)
		{
			var board = map.GetBoard();
			return Trace(map.Dimensions, p => board[p].HasTile, lightRange, lights);
		}

		public static (LightMap FloorLight, LightMap CeilingLight) Trace(
			Size dimensions,
			Func<Position, bool> isPositionObscured,
			LightRange lightRange,
			IEnumerable<LightDefinition> lights
		)
		{
			var floorLight = new LightMap(lightRange, dimensions).Blackout();
			var ceilingLight = new LightMap(lightRange, dimensions).Blackout();

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

						if (!dimensions.Contains(location))
							continue;

						// check for line of sight
						var obscured = DrawingUtil
							.BresenhamLine(start: light.Center, end: location)
							.Any(isPositionObscured);

						if (!obscured)
						{
							var (floorIncrement, ceilingIncrement) = light.GetBrightness(location);

							floorLight.Lighten(location, floorIncrement);
							ceilingLight.Lighten(location, ceilingIncrement);
						}
					}
				}
			}

			return (floorLight, ceilingLight);
		}
	}
}
