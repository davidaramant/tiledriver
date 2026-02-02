// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

#nullable enable
namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class SideDefBuilder
{
	int? Sector { get; set; }
	Texture? TextureTop { get; set; }
	Texture? TextureBottom { get; set; }
	Texture? TextureMiddle { get; set; }
	int OffsetX { get; set; } = 0;
	int OffsetY { get; set; } = 0;
	string Comment { get; set; } = "";

	public SideDef Build() =>
		new(
			Sector: Sector ?? throw new ArgumentNullException("Sector must have a value assigned."),
			TextureTop: TextureTop ?? Texture.None,
			TextureBottom: TextureBottom ?? Texture.None,
			TextureMiddle: TextureMiddle ?? Texture.None,
			OffsetX: OffsetX,
			OffsetY: OffsetY,
			Comment: Comment
		);
}
