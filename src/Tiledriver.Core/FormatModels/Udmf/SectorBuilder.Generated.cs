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
	int? HeightFloor { get; set; }
	int? HeightCeiling { get; set; }
	Texture? TextureFloor { get; set; }
	Texture? TextureCeiling { get; set; }
	int? LightLevel { get; set; }
	int Special { get; set; } = 0;
	int Id { get; set; } = 0;
	bool DropActors { get; set; } = false;
	string Comment { get; set; } = "";

	public Sector Build() =>
		new(
			HeightFloor: HeightFloor ?? throw new ArgumentNullException("HeightFloor must have a value assigned."),
			HeightCeiling: HeightCeiling ?? throw new ArgumentNullException("HeightCeiling must have a value assigned."),
			TextureFloor: TextureFloor ?? throw new ArgumentNullException("TextureFloor must have a value assigned."),
			TextureCeiling: TextureCeiling ?? throw new ArgumentNullException("TextureCeiling must have a value assigned."),
			LightLevel: LightLevel ?? throw new ArgumentNullException("LightLevel must have a value assigned."),
			Special: Special,
			Id: Id,
			DropActors: DropActors,
			Comment: Comment
		);
}
