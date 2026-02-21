using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Image(
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
