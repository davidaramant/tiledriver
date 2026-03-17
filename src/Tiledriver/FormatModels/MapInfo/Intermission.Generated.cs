using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Intermission(
	string Name,
	ImmutableArray<IIntermissionAction> IntermissionActions
);
