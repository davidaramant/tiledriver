using System.CodeDom.Compiler;

namespace Tiledriver.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record MapSquare(
	int Tile,
	int Sector,
	int Zone,
	int Tag = 0
);
