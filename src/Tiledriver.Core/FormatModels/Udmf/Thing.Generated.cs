// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.Udmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Thing(
	double X,
	double Y,
	int Angle,
	int Type,
	int Id = 0,
	double Height = 0,
	bool Skill1 = false,
	bool Skill2 = false,
	bool Skill3 = false,
	bool Skill4 = false,
	bool Skill5 = false,
	bool Single = false,
	bool Coop = false,
	bool Dm = false,
	bool Ambush = false,
	string Comment = ""
);
