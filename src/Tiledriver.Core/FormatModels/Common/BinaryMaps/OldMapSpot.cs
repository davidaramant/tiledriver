using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Common.BinaryMaps;

public sealed record OldMapSpot(ushort OldNum, int Index, int X, int Y)
{
	public Position Location => new(X, Y);
}
