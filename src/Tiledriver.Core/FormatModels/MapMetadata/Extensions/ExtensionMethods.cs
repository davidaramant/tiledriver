// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.FormatModels.MapMetadata.Extensions;

public static class ExtensionMethods
{
	public static Position GetPosition(this Thing thing) => new((int)thing.X, (int)thing.Y);

	public static void AddAllSurrounding(this Queue<Position> positions, Position p)
	{
		foreach (var neighbor in p.GetVonNeumannNeighbors())
		{
			positions.Enqueue(neighbor);
		}
	}

	public static bool Contains(this Size bounds, Position position)
	{
		return position.X >= 0 && position.X < bounds.Width && position.Y >= 0 && position.Y < bounds.Height;
	}
}
