using Tiledriver.LevelGeometry;

namespace Tiledriver.FormatModels.Common.BinaryMaps;

public sealed record OldMapSpot(ushort OldNum, int Index, int X, int Y)
{
	public Position Location => new(X, Y);
}
