// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Trigger(
	int X,
	int Y,
	int Z,
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
