using System.CodeDom.Compiler;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.MapInfo;
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
