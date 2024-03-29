// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Skill(
	Identifier Id,
	double DamageFactor,
	int Lives,
	int MapFilter,
	string MustConfirm,
	string Name,
	string PicName,
	double PlayerDamageFactor,
	bool QuizHints,
	double ScoreMultiplier,
	int SpawnFilter,
	bool FastMontsters = true
);
