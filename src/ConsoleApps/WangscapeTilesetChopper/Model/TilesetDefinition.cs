using System.Collections.Immutable;

namespace WangscapeTilesetChopper.Model;

internal sealed record TilesetDefinition(
	ImmutableArray<int> Resolution,
	string Filename,
	int X,
	int Y,
	ImmutableArray<string> Terrains
)
{
	public int Width => Resolution[0];
	public int Height => Resolution[1];
}
