namespace Tiledriver.FormatModels.Uwmf;

public sealed partial record MapSquare
{
	public bool HasTile => Tile != -1;
}
