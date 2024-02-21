// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf;

public sealed partial record SideDef
{
	public SideDef(
		int sector,
		Texture? textureTop = null,
		Texture? textureBottom = null,
		Texture? textureMiddle = null,
		int offsetX = 0,
		int offsetY = 0,
		string comment = ""
	)
		: this(
			TextureTop: textureTop ?? Texture.None,
			TextureBottom: textureBottom ?? Texture.None,
			TextureMiddle: textureMiddle ?? Texture.None,
			Sector: sector,
			OffsetX: offsetX,
			OffsetY: offsetY,
			Comment: comment
		) { }
}
