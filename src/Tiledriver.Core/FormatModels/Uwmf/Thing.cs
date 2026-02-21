using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Uwmf;

public sealed partial record Thing
{
	public Position TilePosition() => new((int)X, (int)Y);
}
