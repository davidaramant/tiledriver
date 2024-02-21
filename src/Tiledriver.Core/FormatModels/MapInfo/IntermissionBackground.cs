// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.MapInfo;

public sealed class IntermissionBackground
{
	public string Texture { get; }
	public bool ShouldTile { get; }
	public string? Palette { get; }

	public IntermissionBackground(string texture, bool shouldTile = false, string? palette = null)
	{
		Texture = texture;
		ShouldTile = shouldTile;
		Palette = palette;
	}
}
