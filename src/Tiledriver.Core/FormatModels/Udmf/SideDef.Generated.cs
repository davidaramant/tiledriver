using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf;
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
