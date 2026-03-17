using Tiledriver.LevelGeometry;
using Tiledriver.LevelGeometry.CoordinateSystems;

namespace Tiledriver.Utils.ConnectedComponentLabeling.Extensions;

public static class ConnectedAreaExtensions
{
	public static int CountAdjacentWalls(this ConnectedArea area, Position p) =>
		(area.Contains(p + CoordinateSystem.TopLeft.Up) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Down) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Left) ? 0 : 1)
		+ (area.Contains(p + CoordinateSystem.TopLeft.Right) ? 0 : 1);
}
