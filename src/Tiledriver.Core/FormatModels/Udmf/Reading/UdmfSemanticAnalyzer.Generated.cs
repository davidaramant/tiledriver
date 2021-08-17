// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Udmf.Reading
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public static partial class UdmfSemanticAnalyzer
    {
        private static LineDef ReadLineDef(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new LineDef(
                V1: fields.GetRequiredFieldValue<int>(block.Name, "v1"),
                V2: fields.GetRequiredFieldValue<int>(block.Name, "v2"),
                SideFront: fields.GetRequiredFieldValue<int>(block.Name, "sideFront"),
                Id: fields.GetOptionalFieldValue<int>("id", -1),
                Blocking: fields.GetOptionalFieldValue<bool>("blocking", false),
                BlockMonsters: fields.GetOptionalFieldValue<bool>("blockMonsters", false),
                TwoSided: fields.GetOptionalFieldValue<bool>("twoSided", false),
                DontPegTop: fields.GetOptionalFieldValue<bool>("dontPegTop", false),
                DontPegBottom: fields.GetOptionalFieldValue<bool>("dontPegBottom", false),
                Secret: fields.GetOptionalFieldValue<bool>("secret", false),
                BlockSound: fields.GetOptionalFieldValue<bool>("blockSound", false),
                DontDraw: fields.GetOptionalFieldValue<bool>("dontDraw", false),
                Mapped: fields.GetOptionalFieldValue<bool>("mapped", false),
                Special: fields.GetOptionalFieldValue<int>("special", 0),
                Arg0: fields.GetOptionalFieldValue<int>("arg0", 0),
                Arg1: fields.GetOptionalFieldValue<int>("arg1", 0),
                Arg2: fields.GetOptionalFieldValue<int>("arg2", 0),
                Arg3: fields.GetOptionalFieldValue<int>("arg3", 0),
                Arg4: fields.GetOptionalFieldValue<int>("arg4", 0),
                SideBack: fields.GetOptionalFieldValue<int>("sideBack", -1),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
        private static SideDef ReadSideDef(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new SideDef(
                TextureTop: fields.GetOptionalTextureFieldValue("textureTop"),
                TextureBottom: fields.GetOptionalTextureFieldValue("textureBottom"),
                TextureMiddle: fields.GetOptionalTextureFieldValue("textureMiddle"),
                Sector: fields.GetRequiredFieldValue<int>(block.Name, "sector"),
                OffsetX: fields.GetOptionalFieldValue<int>("offsetX", 0),
                OffsetY: fields.GetOptionalFieldValue<int>("offsetY", 0),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
        private static Vertex ReadVertex(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Vertex(
                X: fields.GetRequiredDoubleFieldValue(block.Name, "x"),
                Y: fields.GetRequiredDoubleFieldValue(block.Name, "y"),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
        private static Sector ReadSector(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Sector(
                TextureFloor: fields.GetRequiredTextureFieldValue(block.Name, "textureFloor"),
                TextureCeiling: fields.GetRequiredTextureFieldValue(block.Name, "textureCeiling"),
                HeightFloor: fields.GetOptionalFieldValue<int>("heightFloor", 0),
                HeightCeiling: fields.GetOptionalFieldValue<int>("heightCeiling", 0),
                LightLevel: fields.GetOptionalFieldValue<int>("lightLevel", 160),
                Special: fields.GetOptionalFieldValue<int>("special", 0),
                Id: fields.GetOptionalFieldValue<int>("id", 0),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
        private static Thing ReadThing(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Thing(
                X: fields.GetRequiredDoubleFieldValue(block.Name, "x"),
                Y: fields.GetRequiredDoubleFieldValue(block.Name, "y"),
                Type: fields.GetRequiredFieldValue<int>(block.Name, "type"),
                Id: fields.GetOptionalFieldValue<int>("id", 0),
                Height: fields.GetRequiredDoubleFieldValue(block.Name, "height"),
                Angle: fields.GetOptionalFieldValue<int>("angle", 0),
                Skill1: fields.GetOptionalFieldValue<bool>("skill1", false),
                Skill2: fields.GetOptionalFieldValue<bool>("skill2", false),
                Skill3: fields.GetOptionalFieldValue<bool>("skill3", false),
                Skill4: fields.GetOptionalFieldValue<bool>("skill4", false),
                Skill5: fields.GetOptionalFieldValue<bool>("skill5", false),
                Ambush: fields.GetOptionalFieldValue<bool>("ambush", false),
                Single: fields.GetOptionalFieldValue<bool>("single", false),
                Dm: fields.GetOptionalFieldValue<bool>("dm", false),
                Coop: fields.GetOptionalFieldValue<bool>("coop", false),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
        public static MapData ReadMapData(IEnumerable<IExpression> ast)
        {
            Dictionary<Identifier, Token> fields = new();
            var block = new IdentifierToken(FilePosition.StartOfFile, "MapData");
            var lineDefBuilder = ImmutableList.CreateBuilder<LineDef>();
            var sideDefBuilder = ImmutableList.CreateBuilder<SideDef>();
            var verticesBuilder = ImmutableList.CreateBuilder<Vertex>();
            var sectorBuilder = ImmutableList.CreateBuilder<Sector>();
            var thingBuilder = ImmutableList.CreateBuilder<Thing>();

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
                            case "lineDef":
                                lineDefBuilder.Add(ReadLineDef(b));
                                break;
                            case "sideDef":
                                sideDefBuilder.Add(ReadSideDef(b));
                                break;
                            case "vertices":
                                verticesBuilder.Add(ReadVertex(b));
                                break;
                            case "sector":
                                sectorBuilder.Add(ReadSector(b));
                                break;
                            case "thing":
                                thingBuilder.Add(ReadThing(b));
                                break;
                            default:
                                throw new ParsingException($"Unknown block: {b.Name}");
                        }
                        break;

                    default:
                        throw new ParsingException("Unknown expression type");
                }
            }

            return new MapData(
                NameSpace: fields.GetRequiredFieldValue<string>(block, "namespace"),
                LineDefs: lineDefBuilder.ToImmutable(),
                SideDefs: sideDefBuilder.ToImmutable(),
                Vertices: verticesBuilder.ToImmutable(),
                Sectors: sectorBuilder.ToImmutable(),
                Things: thingBuilder.ToImmutable(),
                Comment: fields.GetOptionalFieldValue<string>("comment", "")
            );
        }
    }
}
