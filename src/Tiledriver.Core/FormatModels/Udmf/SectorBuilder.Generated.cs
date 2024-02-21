// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

#nullable enable
namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial class SectorBuilder
{
	Texture? TextureFloor { get; set; }
	Texture? TextureCeiling { get; set; }
	int HeightFloor { get; set; } = 0;
	int HeightCeiling { get; set; } = 0;
	int LightLevel { get; set; } = 160;
	int Special { get; set; } = 0;
	int Id { get; set; } = 0;
	string Comment { get; set; } = "";

	public Sector Build() =>
		new(
			TextureFloor: TextureFloor ?? throw new ArgumentNullException("TextureFloor must have a value assigned."),
			TextureCeiling: TextureCeiling ?? throw new ArgumentNullException("TextureCeiling must have a value assigned."),
			HeightFloor: HeightFloor,
			HeightCeiling: HeightCeiling,
			LightLevel: LightLevel,
			Special: Special,
			Id: Id,
			Comment: Comment
		);
}
