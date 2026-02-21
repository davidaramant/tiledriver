using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record LineDef(
	int V1,
	int V2,
	int SideFront,
	int Id = -1,
	int SideBack = -1,
	int Special = 0,
	int Arg0 = 0,
	int Arg1 = 0,
	int Arg2 = 0,
	int Arg3 = 0,
	int Arg4 = 0,
	bool TwoSided = false,
	bool DontPegTop = false,
	bool DontPegBottom = false,
	bool BlockMonsters = false,
	bool BlockSound = false,
	bool Secret = false,
	bool MonsterActivate = false,
	bool PlayerUse = false,
	bool Blocking = false,
	bool RepeatSpecial = false,
	bool PlayerCross = false,
	bool DontDraw = false,
	bool Mapped = false,
	string Comment = ""
);
