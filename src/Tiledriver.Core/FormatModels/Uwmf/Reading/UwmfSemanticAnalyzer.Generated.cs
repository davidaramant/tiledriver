// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class UwmfSemanticAnalyzer
{
	private static Tile ReadTile(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Tile(
			TextureEast: fields.GetRequiredTextureFieldValue(block.Name, "textureEast"),
			TextureNorth: fields.GetRequiredTextureFieldValue(block.Name, "textureNorth"),
			TextureWest: fields.GetRequiredTextureFieldValue(block.Name, "textureWest"),
			TextureSouth: fields.GetRequiredTextureFieldValue(block.Name, "textureSouth"),
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
	private static Sector ReadSector(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Sector(
			TextureCeiling: fields.GetRequiredTextureFieldValue(block.Name, "textureCeiling"),
			TextureFloor: fields.GetRequiredTextureFieldValue(block.Name, "textureFloor"),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
	private static Zone ReadZone(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Zone(
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
	private static Plane ReadPlane(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Plane(
			Depth: fields.GetRequiredFieldValue<int>(block.Name, "depth"),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
	private static Thing ReadThing(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Thing(
			Type: fields.GetRequiredFieldValue<string>(block.Name, "type"),
			X: fields.GetRequiredDoubleFieldValue(block.Name, "x"),
			Y: fields.GetRequiredDoubleFieldValue(block.Name, "y"),
			Z: fields.GetRequiredDoubleFieldValue(block.Name, "z"),
			Angle: fields.GetRequiredFieldValue<int>(block.Name, "angle"),
			Ambush: fields.GetOptionalFieldValue<bool>("ambush", false),
			Patrol: fields.GetOptionalFieldValue<bool>("patrol", false),
			Skill1: fields.GetOptionalFieldValue<bool>("skill1", false),
			Skill2: fields.GetOptionalFieldValue<bool>("skill2", false),
			Skill3: fields.GetOptionalFieldValue<bool>("skill3", false),
			Skill4: fields.GetOptionalFieldValue<bool>("skill4", false),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
	private static Trigger ReadTrigger(Block block)
	{
		var fields = block.GetFieldAssignments();

		return new Trigger(
			X: fields.GetRequiredFieldValue<int>(block.Name, "x"),
			Y: fields.GetRequiredFieldValue<int>(block.Name, "y"),
			Z: fields.GetRequiredFieldValue<int>(block.Name, "z"),
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
	public static MapData ReadMapData(IEnumerable<IExpression> ast)
	{
		Dictionary<Identifier, Token> fields = new();
		var block = new IdentifierToken(FilePosition.StartOfFile, "MapData");
		var tileBuilder = ImmutableArray.CreateBuilder<Tile>();
		var sectorBuilder = ImmutableArray.CreateBuilder<Sector>();
		var zoneBuilder = ImmutableArray.CreateBuilder<Zone>();
		var planeBuilder = ImmutableArray.CreateBuilder<Plane>();
		var planeMapBuilder = ImmutableArray.CreateBuilder<ImmutableArray<MapSquare>>();
		var thingBuilder = ImmutableArray.CreateBuilder<Thing>();
		var triggerBuilder = ImmutableArray.CreateBuilder<Trigger>();

		foreach(var expression in ast)
		{
			switch (expression)
			{
				case Assignment a:
					fields.Add(a.Name.Id, a.Value);
					break;

				case Block b:
					switch (b.Name.Id.ToLower())
					{
						case "tile":
							tileBuilder.Add(ReadTile(b));
							break;
						case "sector":
							sectorBuilder.Add(ReadSector(b));
							break;
						case "zone":
							zoneBuilder.Add(ReadZone(b));
							break;
						case "plane":
							planeBuilder.Add(ReadPlane(b));
							break;
						case "thing":
							thingBuilder.Add(ReadThing(b));
							break;
						case "trigger":
							triggerBuilder.Add(ReadTrigger(b));
							break;
						default:
							throw new ParsingException($"Unknown block: {b.Name}");
					}
					break;

				case IntTupleBlock itb:
					if (itb.Name.Id.ToLower() != "planemap")
					{
						throw new ParsingException("Unknown int tuple block");
					}
					planeMapBuilder.Add(ReadPlaneMap(itb));
					break;

				default:
					throw new ParsingException("Unknown expression type");
			}
		}

		return new MapData(
			NameSpace: fields.GetRequiredFieldValue<string>(block, "namespace"),
			TileSize: fields.GetRequiredFieldValue<int>(block, "tileSize"),
			Name: fields.GetRequiredFieldValue<string>(block, "name"),
			Width: fields.GetRequiredFieldValue<int>(block, "width"),
			Height: fields.GetRequiredFieldValue<int>(block, "height"),
			Tiles: tileBuilder.ToImmutable(),
			Sectors: sectorBuilder.ToImmutable(),
			Zones: zoneBuilder.ToImmutable(),
			Planes: planeBuilder.ToImmutable(),
			PlaneMaps: planeMapBuilder.ToImmutable(),
			Things: thingBuilder.ToImmutable(),
			Triggers: triggerBuilder.ToImmutable(),
			Comment: fields.GetOptionalFieldValue<string>("comment", "")
		);
	}
}
