using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Xlat;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record TriggerTemplate(
	ushort OldNum,
	string Action,
	int Arg0 = 0,
	int Arg1 = 0,
	int Arg2 = 0,
	int Arg3 = 0,
	int Arg4 = 0,
	bool ActivateEast = true,
	bool ActivateNorth = true,
	bool ActivateWest = true,
	bool ActivateSouth = true,
	bool PlayerCross = false,
	bool PlayerUse = false,
	bool MonsterUse = false,
	bool Repeatable = false,
	bool Secret = false,
	string Comment = ""
);
