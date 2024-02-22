// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.LevelGeometry.Lighting;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Wolf;

public static class CaveThingPlacement
{
	public static IEnumerable<LightDefinition> RandomlyPlaceLights(
		IReadOnlyList<Position> validPositions,
		Random random,
		LightRange lightRange,
		double percentAreaToCover,
		bool varyHeight = false
	)
	{
		var numLights = (int)(validPositions.Count * percentAreaToCover);

		var positions = new List<Position>();

		return Enumerable
			.Range(0, numLights)
			.Select(_ =>
			{
				while (true)
				{
					var posIndex = random.Next(0, validPositions.Count);
					var position = validPositions[posIndex];

					var positionIsInvalid = positions.Any(p => p.Touches(position));

					if (positionIsInvalid)
						continue;

					positions.Add(position);

					return new LightDefinition(
						position,
						Brightness: lightRange.DarkLevels,
						Radius: 10,
						Height: varyHeight
							? random.Next(2) == 0
								? LightHeight.Ceiling
								: LightHeight.Floor
							: LightHeight.Middle
					);
				}
			});
	}

	public static IEnumerable<(Position Location, TreasureType Type)> RandomlyPlaceTreasure(
		ConnectedArea area,
		IEnumerable<Position> edge,
		LightMap floorLighting,
		LightRange lightRange,
		Random random
	)
	{
		var maxScore = 3 * lightRange.DarkLevels;
		var cutOff = (int)(0.5 * maxScore);

		var scoredLocations = edge.Select(p => new
			{
				Location = p,
				Score = area.CountAdjacentWalls(p) * -floorLighting[p]
			})
			.OrderBy(scoredLocation => scoredLocation.Score)
			.Where(scoredLocation => scoredLocation.Score >= cutOff)
			.OrderBy(sl => random.Next())
			.ToArray();

		var numberToTake = scoredLocations.Length / 2;

		return scoredLocations.Take(numberToTake).Select(sl => (sl.Location, TreasureType.Medium));
	}
}
