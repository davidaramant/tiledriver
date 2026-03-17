using System.CodeDom.Compiler;

namespace Tiledriver.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public abstract partial record BaseIntermissionAction(
	IntermissionBackground Background,
	IntermissionDraw Draw,
	string Music,
	double Time
);
