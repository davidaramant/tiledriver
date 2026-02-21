using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry;

public interface IBoard
{
	Size Dimensions { get; }
	MapSquare this[Position pos] { get; }
	MapSquare this[int x, int y] { get; }
	ImmutableArray<MapSquare> ToPlaneMap();
	ICanvas ToCanvas();
}
