using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Xlat;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record ThingTemplate(
	ushort OldNum,
	string Type,
	int Angles,
	bool Holowall,
	bool Pathing,
	bool Ambush,
	int Minskill
);
