using System.CodeDom.Compiler;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Sector(
	Texture TextureCeiling,
	Texture TextureFloor,
	string Comment = ""
);
