using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Sector(
	int HeightFloor,
	int HeightCeiling,
	Texture TextureFloor,
	Texture TextureCeiling,
	int LightLevel,
	int Special = 0,
	int Id = 0,
	bool DropActors = false,
	string Comment = ""
);
