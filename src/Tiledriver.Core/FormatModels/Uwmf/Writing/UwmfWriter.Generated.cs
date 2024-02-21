// Copyright (c) 2024, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.IO;

namespace Tiledriver.Core.FormatModels.Uwmf.Writing;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class UwmfWriter
{
	private static void Write(StreamWriter writer, Tile tile)
	{
		writer.WriteLine("tile");
		writer.WriteLine("{");
		WriteProperty(writer, "textureEast", tile.TextureEast);
		WriteProperty(writer, "textureNorth", tile.TextureNorth);
		WriteProperty(writer, "textureWest", tile.TextureWest);
		WriteProperty(writer, "textureSouth", tile.TextureSouth);
		WriteProperty(writer, "blockingEast", tile.BlockingEast, true);
		WriteProperty(writer, "blockingNorth", tile.BlockingNorth, true);
		WriteProperty(writer, "blockingWest", tile.BlockingWest, true);
		WriteProperty(writer, "blockingSouth", tile.BlockingSouth, true);
		WriteProperty(writer, "offsetVertical", tile.OffsetVertical, false);
		WriteProperty(writer, "offsetHorizontal", tile.OffsetHorizontal, false);
		WriteProperty(writer, "dontOverlay", tile.DontOverlay, false);
		WriteProperty(writer, "mapped", tile.Mapped, 0);
		WriteProperty(writer, "soundSequence", tile.SoundSequence, "");
		WriteProperty(writer, "textureOverhead", tile.TextureOverhead, "");
		WriteProperty(writer, "comment", tile.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, Sector sector)
	{
		writer.WriteLine("sector");
		writer.WriteLine("{");
		WriteProperty(writer, "textureCeiling", sector.TextureCeiling);
		WriteProperty(writer, "textureFloor", sector.TextureFloor);
		WriteProperty(writer, "comment", sector.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, Zone zone)
	{
		writer.WriteLine("zone");
		writer.WriteLine("{");
		WriteProperty(writer, "comment", zone.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, Plane plane)
	{
		writer.WriteLine("plane");
		writer.WriteLine("{");
		WriteProperty(writer, "depth", plane.Depth);
		WriteProperty(writer, "comment", plane.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, Thing thing)
	{
		writer.WriteLine("thing");
		writer.WriteLine("{");
		WriteProperty(writer, "type", thing.Type);
		WriteProperty(writer, "x", thing.X);
		WriteProperty(writer, "y", thing.Y);
		WriteProperty(writer, "z", thing.Z);
		WriteProperty(writer, "angle", thing.Angle);
		WriteProperty(writer, "ambush", thing.Ambush, false);
		WriteProperty(writer, "patrol", thing.Patrol, false);
		WriteProperty(writer, "skill1", thing.Skill1, false);
		WriteProperty(writer, "skill2", thing.Skill2, false);
		WriteProperty(writer, "skill3", thing.Skill3, false);
		WriteProperty(writer, "skill4", thing.Skill4, false);
		WriteProperty(writer, "comment", thing.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, Trigger trigger)
	{
		writer.WriteLine("trigger");
		writer.WriteLine("{");
		WriteProperty(writer, "x", trigger.X);
		WriteProperty(writer, "y", trigger.Y);
		WriteProperty(writer, "z", trigger.Z);
		WriteProperty(writer, "action", trigger.Action);
		WriteProperty(writer, "arg0", trigger.Arg0, 0);
		WriteProperty(writer, "arg1", trigger.Arg1, 0);
		WriteProperty(writer, "arg2", trigger.Arg2, 0);
		WriteProperty(writer, "arg3", trigger.Arg3, 0);
		WriteProperty(writer, "arg4", trigger.Arg4, 0);
		WriteProperty(writer, "activateEast", trigger.ActivateEast, true);
		WriteProperty(writer, "activateNorth", trigger.ActivateNorth, true);
		WriteProperty(writer, "activateWest", trigger.ActivateWest, true);
		WriteProperty(writer, "activateSouth", trigger.ActivateSouth, true);
		WriteProperty(writer, "playerCross", trigger.PlayerCross, false);
		WriteProperty(writer, "playerUse", trigger.PlayerUse, false);
		WriteProperty(writer, "monsterUse", trigger.MonsterUse, false);
		WriteProperty(writer, "repeatable", trigger.Repeatable, false);
		WriteProperty(writer, "secret", trigger.Secret, false);
		WriteProperty(writer, "comment", trigger.Comment, "");
		writer.WriteLine("}");
	}
	private static void Write(StreamWriter writer, MapData mapData)
	{
		WriteProperty(writer, "namespace", mapData.NameSpace, indent:false);
		WriteProperty(writer, "tileSize", mapData.TileSize, indent:false);
		WriteProperty(writer, "name", mapData.Name, indent:false);
		WriteProperty(writer, "width", mapData.Width, indent:false);
		WriteProperty(writer, "height", mapData.Height, indent:false);
		WriteProperty(writer, "comment", mapData.Comment, "", indent:false);
		foreach(var block in mapData.Tiles)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.Sectors)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.Zones)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.Planes)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.PlaneMaps)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.Things)
		{
			Write(writer, block);
		}
		foreach(var block in mapData.Triggers)
		{
			Write(writer, block);
		}
	}
}
