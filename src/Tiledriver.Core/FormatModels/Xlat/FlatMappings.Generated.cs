using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Xlat;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record FlatMappings(
	ImmutableArray<string> Ceilings,
	ImmutableArray<string> Floors
);
