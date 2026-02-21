using System.CodeDom.Compiler;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Xlat.Reading;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class XlatParser
{
	private static TriggerTemplate ReadTriggerTemplate(ushort oldNum, Block block)
	{
		var fields = block.GetFieldAssignments();

		return new TriggerTemplate(
			oldNum,
			Action: fields.GetRequiredFieldValue<string>(block.Name, "action"),
			Arg0: fields.GetOptionalFieldValue<int>("arg0", 0),
			Arg1: fields.GetOptionalFieldValue<int>("arg1", 0),
			Arg2: fields.GetOptionalFieldValue<int>("arg2", 0),
			Arg3: fields.GetOptionalFieldValue<int>("arg3", 0),
			Arg4: fields.GetOptionalFieldValue<int>("arg4", 0),
			ActivateEast: fields.GetOptionalFieldValue<bool>("activateEast", true),
			ActivateNorth: fields.GetOptionalFieldValue<bool>("activateNorth", true),
			ActivateWest: fields.GetOptionalFieldValue<bool>("activateWest", true),
			ActivateSouth: fields.GetOptionalFieldValue<bool>("activateSouth", true),
			PlayerCross: fields.GetOptionalFieldValue<bool>("playerCross", false),
			PlayerUse: fields.GetOptionalFieldValue<bool>("playerUse", false),
			MonsterUse: fields.GetOptionalFieldValue<bool>("monsterUse", false),
			Repeatable: fields.GetOptionalFieldValue<bool>("repeatable", false),
			Secret: fields.GetOptionalFieldValue<bool>("secret", false),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
	private static TileTemplate ReadTileTemplate(ushort oldNum, Block block)
	{
		var fields = block.GetFieldAssignments();

		return new TileTemplate(
			oldNum,
			TextureEast: fields.GetRequiredFieldValue<string>(block.Name, "textureEast"),
			TextureNorth: fields.GetRequiredFieldValue<string>(block.Name, "textureNorth"),
			TextureWest: fields.GetRequiredFieldValue<string>(block.Name, "textureWest"),
			TextureSouth: fields.GetRequiredFieldValue<string>(block.Name, "textureSouth"),
			BlockingEast: fields.GetOptionalFieldValue<bool>("blockingEast", true),
			BlockingNorth: fields.GetOptionalFieldValue<bool>("blockingNorth", true),
			BlockingWest: fields.GetOptionalFieldValue<bool>("blockingWest", true),
			BlockingSouth: fields.GetOptionalFieldValue<bool>("blockingSouth", true),
			OffsetVertical: fields.GetOptionalFieldValue<bool>("offsetVertical", false),
			OffsetHorizontal: fields.GetOptionalFieldValue<bool>("offsetHorizontal", false),
			DontOverlay: fields.GetOptionalFieldValue<bool>("dontOverlay", false),
			Mapped: fields.GetOptionalFieldValue<int>("mapped", 0),
			SoundSequence: fields.GetOptionalFieldValue<string>("soundSequence", ""),
			TextureOverhead: fields.GetOptionalFieldValue<string>("textureOverhead", ""),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
}
