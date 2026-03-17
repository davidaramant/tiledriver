using Tiledriver.LevelGeometry;

namespace Tiledriver.FormatModels.Uwmf;

public sealed partial record Thing
{
	public Position TilePosition() => new((int)X, (int)Y);
}
