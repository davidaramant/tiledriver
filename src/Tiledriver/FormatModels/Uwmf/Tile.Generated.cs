using System.CodeDom.Compiler;
using Tiledriver.FormatModels.Common;

namespace Tiledriver.FormatModels.Uwmf;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public sealed partial record Tile(
	Texture TextureEast,
	Texture TextureNorth,
	Texture TextureWest,
	Texture TextureSouth,
	bool BlockingEast = true,
	bool BlockingNorth = true,
	bool BlockingWest = true,
	bool BlockingSouth = true,
	bool OffsetVertical = false,
	bool OffsetHorizontal = false,
	bool DontOverlay = false,
	int Mapped = 0,
	string SoundSequence = "",
	string TextureOverhead = "",
	string Comment = ""
);
