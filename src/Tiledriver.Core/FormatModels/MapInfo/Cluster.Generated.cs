using System.CodeDom.Compiler;

namespace Tiledriver.Core.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Cluster(
	int Id,
	ClusterExitText ExitText,
	bool ExitTextIsLump = true,
	bool ExitTextIsMessage = true
);
