using System.CodeDom.Compiler;

namespace Tiledriver.FormatModels.MapInfo;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record AutoMap(
	string Background,
	string DoorColor,
	string FloorColor,
	string FontColor,
	string WallColor,
	string YourColor
);
