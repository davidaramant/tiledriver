// Copyright (c) 2026, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.IO;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf.Writing;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class UdmfWriter
{
	private static void Write(StreamWriter writer, Thing thing, int index = 0)
	{
		writer.WriteLine($"thing // {index}");
		writer.WriteLine("{");
		WriteProperty(writer, "id", thing.Id, 0);
		WriteProperty(writer, "x", thing.X);
		WriteProperty(writer, "y", thing.Y);
		WriteProperty(writer, "height", thing.Height, 0);
		WriteProperty(writer, "angle", thing.Angle);
		WriteProperty(writer, "type", thing.Type);
		WriteProperty(writer, "skill1", thing.Skill1, false);
		WriteProperty(writer, "skill2", thing.Skill2, false);
		WriteProperty(writer, "skill3", thing.Skill3, false);
		WriteProperty(writer, "skill4", thing.Skill4, false);
		WriteProperty(writer, "skill5", thing.Skill5, false);
		WriteProperty(writer, "single", thing.Single, false);
		WriteProperty(writer, "coop", thing.Coop, false);
		WriteProperty(writer, "dm", thing.Dm, false);
		WriteProperty(writer, "ambush", thing.Ambush, false);
		WriteProperty(writer, "comment", thing.Comment, "");
		writer.WriteLine("}");
		writer.WriteLine();
	}
	private static void Write(StreamWriter writer, Vertex vertex, int index = 0)
	{
		writer.WriteLine($"vertex // {index}");
		writer.WriteLine("{");
		WriteProperty(writer, "x", vertex.X);
		WriteProperty(writer, "y", vertex.Y);
		WriteProperty(writer, "comment", vertex.Comment, "");
		writer.WriteLine("}");
		writer.WriteLine();
	}
	private static void Write(StreamWriter writer, LineDef linedef, int index = 0)
	{
		writer.WriteLine($"linedef // {index}");
		writer.WriteLine("{");
		WriteProperty(writer, "id", linedef.Id, -1);
		WriteProperty(writer, "v1", linedef.V1);
		WriteProperty(writer, "v2", linedef.V2);
		WriteProperty(writer, "sideFront", linedef.SideFront);
		WriteProperty(writer, "sideBack", linedef.SideBack, -1);
		WriteProperty(writer, "special", linedef.Special, 0);
		WriteProperty(writer, "arg0", linedef.Arg0, 0);
		WriteProperty(writer, "arg1", linedef.Arg1, 0);
		WriteProperty(writer, "arg2", linedef.Arg2, 0);
		WriteProperty(writer, "arg3", linedef.Arg3, 0);
		WriteProperty(writer, "arg4", linedef.Arg4, 0);
		WriteProperty(writer, "twoSided", linedef.TwoSided, false);
		WriteProperty(writer, "dontPegTop", linedef.DontPegTop, false);
		WriteProperty(writer, "dontPegBottom", linedef.DontPegBottom, false);
		WriteProperty(writer, "blockMonsters", linedef.BlockMonsters, false);
		WriteProperty(writer, "blockSound", linedef.BlockSound, false);
		WriteProperty(writer, "secret", linedef.Secret, false);
		WriteProperty(writer, "monsterActivate", linedef.MonsterActivate, false);
		WriteProperty(writer, "playerUse", linedef.PlayerUse, false);
		WriteProperty(writer, "blocking", linedef.Blocking, false);
		WriteProperty(writer, "repeatSpecial", linedef.RepeatSpecial, false);
		WriteProperty(writer, "playerCross", linedef.PlayerCross, false);
		WriteProperty(writer, "dontDraw", linedef.DontDraw, false);
		WriteProperty(writer, "mapped", linedef.Mapped, false);
		WriteProperty(writer, "comment", linedef.Comment, "");
		writer.WriteLine("}");
		writer.WriteLine();
	}
	private static void Write(StreamWriter writer, SideDef sidedef, int index = 0)
	{
		writer.WriteLine($"sidedef // {index}");
		writer.WriteLine("{");
		WriteProperty(writer, "sector", sidedef.Sector);
		WriteProperty(writer, "offsetX", sidedef.OffsetX, 0);
		WriteProperty(writer, "offsetY", sidedef.OffsetY, 0);
		WriteProperty(writer, "textureTop", sidedef.TextureTop, Texture.None);
		WriteProperty(writer, "textureBottom", sidedef.TextureBottom, Texture.None);
		WriteProperty(writer, "textureMiddle", sidedef.TextureMiddle, Texture.None);
		WriteProperty(writer, "comment", sidedef.Comment, "");
		writer.WriteLine("}");
		writer.WriteLine();
	}
	private static void Write(StreamWriter writer, Sector sector, int index = 0)
	{
		writer.WriteLine($"sector // {index}");
		writer.WriteLine("{");
		WriteProperty(writer, "heightFloor", sector.HeightFloor);
		WriteProperty(writer, "heightCeiling", sector.HeightCeiling);
		WriteProperty(writer, "textureFloor", sector.TextureFloor);
		WriteProperty(writer, "textureCeiling", sector.TextureCeiling);
		WriteProperty(writer, "lightLevel", sector.LightLevel);
		WriteProperty(writer, "special", sector.Special, 0);
		WriteProperty(writer, "id", sector.Id, 0);
		WriteProperty(writer, "dropActors", sector.DropActors, false);
		WriteProperty(writer, "comment", sector.Comment, "");
		writer.WriteLine("}");
		writer.WriteLine();
	}
	private static void Write(StreamWriter writer, MapData mapData, int index = 0)
	{
		WriteProperty(writer, "namespace", mapData.NameSpace);
		WriteProperty(writer, "comment", mapData.Comment, "");
		for(int i = 0; i < mapData.Things.Length; i++)
		{
			Write(writer, mapData.Things[i], i);
		}
		for(int i = 0; i < mapData.Vertices.Length; i++)
		{
			Write(writer, mapData.Vertices[i], i);
		}
		for(int i = 0; i < mapData.LineDefs.Length; i++)
		{
			Write(writer, mapData.LineDefs[i], i);
		}
		for(int i = 0; i < mapData.SideDefs.Length; i++)
		{
			Write(writer, mapData.SideDefs[i], i);
		}
		for(int i = 0; i < mapData.Sectors.Length; i++)
		{
			Write(writer, mapData.Sectors[i], i);
		}
	}
}
