using System.CodeDom.Compiler;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record SideDef(
	int Sector,
	Texture TextureTop,
	Texture TextureBottom,
	Texture TextureMiddle,
	int OffsetX = 0,
	int OffsetY = 0,
	string Comment = ""
);
