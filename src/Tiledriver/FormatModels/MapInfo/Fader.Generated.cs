using System.CodeDom.Compiler;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Fader(
	Identifier FadeType,
	IntermissionBackground Background,
	IntermissionDraw Draw,
	string Music,
	double Time
) : BaseIntermissionAction(
	Background,
	Draw,
	Music,
	Time
);
