// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions;

public static class ConnectedAreaExtensions
{
	public static int CountAdjacentWalls(this ConnectedArea area, Position p) =>
		(area.Contains(p + CoordinateSystem.TopLeft.Up) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Down) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Left) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Right) ? 0 : 1);
}
