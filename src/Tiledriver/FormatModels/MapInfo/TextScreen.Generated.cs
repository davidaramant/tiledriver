using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record TextScreen(
	ImmutableArray<string> Texts,
	Identifier TextAlignment,
	Identifier TextAnchor,
	string TextColor,
	double TextDelay,
	int TextSpeed,
	TextScreenPosition Position,
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
