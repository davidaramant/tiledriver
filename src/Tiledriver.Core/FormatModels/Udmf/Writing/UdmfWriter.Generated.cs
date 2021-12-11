// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.IO;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Udmf.Writing;
[GeneratedCode("DataModelGenerator", "1.0.0.0")]
public static partial class UdmfWriter
{
    private static void Write(StreamWriter writer, LineDef linedef)
    {
        writer.WriteLine("linedef");
        writer.WriteLine("{");
        WriteProperty(writer, "id", linedef.Id, -1);
        WriteProperty(writer, "v1", linedef.V1);
        WriteProperty(writer, "v2", linedef.V2);
        WriteProperty(writer, "blocking", linedef.Blocking, false);
        WriteProperty(writer, "blockMonsters", linedef.BlockMonsters, false);
        WriteProperty(writer, "twoSided", linedef.TwoSided, false);
        WriteProperty(writer, "dontPegTop", linedef.DontPegTop, false);
        WriteProperty(writer, "dontPegBottom", linedef.DontPegBottom, false);
        WriteProperty(writer, "secret", linedef.Secret, false);
        WriteProperty(writer, "blockSound", linedef.BlockSound, false);
        WriteProperty(writer, "dontDraw", linedef.DontDraw, false);
        WriteProperty(writer, "mapped", linedef.Mapped, false);
        WriteProperty(writer, "special", linedef.Special, 0);
        WriteProperty(writer, "arg0", linedef.Arg0, 0);
        WriteProperty(writer, "arg1", linedef.Arg1, 0);
        WriteProperty(writer, "arg2", linedef.Arg2, 0);
        WriteProperty(writer, "arg3", linedef.Arg3, 0);
        WriteProperty(writer, "arg4", linedef.Arg4, 0);
        WriteProperty(writer, "sideFront", linedef.SideFront);
        WriteProperty(writer, "sideBack", linedef.SideBack, -1);
        WriteProperty(writer, "comment", linedef.Comment, "");
        writer.WriteLine("}");
    }
    private static void Write(StreamWriter writer, SideDef sidedef)
    {
        writer.WriteLine("sidedef");
        writer.WriteLine("{");
        WriteProperty(writer, "offsetX", sidedef.OffsetX, 0);
        WriteProperty(writer, "offsetY", sidedef.OffsetY, 0);
        WriteProperty(writer, "textureTop", sidedef.TextureTop, Texture.None);
        WriteProperty(writer, "textureBottom", sidedef.TextureBottom, Texture.None);
        WriteProperty(writer, "textureMiddle", sidedef.TextureMiddle, Texture.None);
        WriteProperty(writer, "sector", sidedef.Sector);
        WriteProperty(writer, "comment", sidedef.Comment, "");
        writer.WriteLine("}");
    }
    private static void Write(StreamWriter writer, Vertex vertex)
    {
        writer.WriteLine("vertex");
        writer.WriteLine("{");
        WriteProperty(writer, "x", vertex.X);
        WriteProperty(writer, "y", vertex.Y);
        WriteProperty(writer, "comment", vertex.Comment, "");
        writer.WriteLine("}");
    }
    private static void Write(StreamWriter writer, Sector sector)
    {
        writer.WriteLine("sector");
        writer.WriteLine("{");
        WriteProperty(writer, "heightFloor", sector.HeightFloor, 0);
        WriteProperty(writer, "heightCeiling", sector.HeightCeiling, 0);
        WriteProperty(writer, "textureFloor", sector.TextureFloor);
        WriteProperty(writer, "textureCeiling", sector.TextureCeiling);
        WriteProperty(writer, "lightLevel", sector.LightLevel, 160);
        WriteProperty(writer, "special", sector.Special, 0);
        WriteProperty(writer, "id", sector.Id, 0);
        WriteProperty(writer, "comment", sector.Comment, "");
        writer.WriteLine("}");
    }
    private static void Write(StreamWriter writer, Thing thing)
    {
        writer.WriteLine("thing");
        writer.WriteLine("{");
        WriteProperty(writer, "id", thing.Id, 0);
        WriteProperty(writer, "x", thing.X);
        WriteProperty(writer, "y", thing.Y);
        WriteProperty(writer, "height", thing.Height, 0);
        WriteProperty(writer, "angle", thing.Angle, 0);
        WriteProperty(writer, "type", thing.Type);
        WriteProperty(writer, "skill1", thing.Skill1, false);
        WriteProperty(writer, "skill2", thing.Skill2, false);
        WriteProperty(writer, "skill3", thing.Skill3, false);
        WriteProperty(writer, "skill4", thing.Skill4, false);
        WriteProperty(writer, "skill5", thing.Skill5, false);
        WriteProperty(writer, "ambush", thing.Ambush, false);
        WriteProperty(writer, "single", thing.Single, false);
        WriteProperty(writer, "dm", thing.Dm, false);
        WriteProperty(writer, "coop", thing.Coop, false);
        WriteProperty(writer, "comment", thing.Comment, "");
        writer.WriteLine("}");
    }
    private static void Write(StreamWriter writer, MapData mapData)
    {
        WriteProperty(writer, "namespace", mapData.NameSpace, indent:false);
        WriteProperty(writer, "comment", mapData.Comment, "", indent:false);
        foreach(var block in mapData.LineDefs)
        {
            Write(writer, block);
        }
        foreach(var block in mapData.SideDefs)
        {
            Write(writer, block);
        }
        foreach(var block in mapData.Vertices)
        {
            Write(writer, block);
        }
        foreach(var block in mapData.Sectors)
        {
            Write(writer, block);
        }
        foreach(var block in mapData.Things)
        {
            Write(writer, block);
        }
    }
}
