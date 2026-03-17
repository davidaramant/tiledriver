using System.Collections.Immutable;
using Tiledriver.FormatModels.Uwmf;

namespace Tiledriver.LevelGeometry;

public interface IBoard
{
	Size Dimensions { get; }
	MapSquare this[Position pos] { get; }
	MapSquare this[int x, int y] { get; }
	ImmutableArray<MapSquare> ToPlaneMap();
	ICanvas ToCanvas();
}
