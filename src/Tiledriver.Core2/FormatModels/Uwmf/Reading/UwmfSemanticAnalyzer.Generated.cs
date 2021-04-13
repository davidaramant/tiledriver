// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public static partial class UwmfSemanticAnalyzer
    {
        private static Tile ReadTile(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Tile(
                TextureEast: GetRequiredFieldValue<string>(fields, block.Name, "textureEast"),
                TextureNorth: GetRequiredFieldValue<string>(fields, block.Name, "textureNorth"),
                TextureWest: GetRequiredFieldValue<string>(fields, block.Name, "textureWest"),
                TextureSouth: GetRequiredFieldValue<string>(fields, block.Name, "textureSouth"),
                BlockingEast: GetOptionalFieldValue<bool>(fields, "blockingEast"),
                BlockingNorth: GetOptionalFieldValue<bool>(fields, "blockingNorth"),
                BlockingWest: GetOptionalFieldValue<bool>(fields, "blockingWest"),
                BlockingSouth: GetOptionalFieldValue<bool>(fields, "blockingSouth"),
                OffsetVertical: GetOptionalFieldValue<bool>(fields, "offsetVertical"),
                OffsetHorizontal: GetOptionalFieldValue<bool>(fields, "offsetHorizontal"),
                DontOverlay: GetOptionalFieldValue<bool>(fields, "dontOverlay"),
                Mapped: GetOptionalFieldValue<int>(fields, "mapped"),
                SoundSequence: GetOptionalFieldValue<string>(fields, "soundSequence"),
                TextureOverhead: GetOptionalFieldValue<string>(fields, "textureOverhead"),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        private static Sector ReadSector(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Sector(
                TextureCeiling: GetRequiredFieldValue<string>(fields, block.Name, "textureCeiling"),
                TextureFloor: GetRequiredFieldValue<string>(fields, block.Name, "textureFloor"),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        private static Zone ReadZone(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Zone(
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        private static Plane ReadPlane(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Plane(
                Depth: GetRequiredFieldValue<int>(fields, block.Name, "depth"),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        private static Thing ReadThing(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Thing(
                Type: GetRequiredFieldValue<string>(fields, block.Name, "type"),
                X: GetRequiredDoubleFieldValue(fields, block.Name, "x"),
                Y: GetRequiredDoubleFieldValue(fields, block.Name, "y"),
                Z: GetRequiredDoubleFieldValue(fields, block.Name, "z"),
                Angle: GetRequiredFieldValue<int>(fields, block.Name, "angle"),
                Ambush: GetOptionalFieldValue<bool>(fields, "ambush"),
                Patrol: GetOptionalFieldValue<bool>(fields, "patrol"),
                Skill1: GetOptionalFieldValue<bool>(fields, "skill1"),
                Skill2: GetOptionalFieldValue<bool>(fields, "skill2"),
                Skill3: GetOptionalFieldValue<bool>(fields, "skill3"),
                Skill4: GetOptionalFieldValue<bool>(fields, "skill4"),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        private static Trigger ReadTrigger(Block block)
        {
            var fields = block.GetFieldAssignments();

            return new Trigger(
                X: GetRequiredFieldValue<int>(fields, block.Name, "x"),
                Y: GetRequiredFieldValue<int>(fields, block.Name, "y"),
                Z: GetRequiredFieldValue<int>(fields, block.Name, "z"),
                Action: GetRequiredFieldValue<string>(fields, block.Name, "action"),
                Arg0: GetOptionalFieldValue<int>(fields, "arg0"),
                Arg1: GetOptionalFieldValue<int>(fields, "arg1"),
                Arg2: GetOptionalFieldValue<int>(fields, "arg2"),
                Arg3: GetOptionalFieldValue<int>(fields, "arg3"),
                Arg4: GetOptionalFieldValue<int>(fields, "arg4"),
                ActivateEast: GetOptionalFieldValue<bool>(fields, "activateEast"),
                ActivateNorth: GetOptionalFieldValue<bool>(fields, "activateNorth"),
                ActivateWest: GetOptionalFieldValue<bool>(fields, "activateWest"),
                ActivateSouth: GetOptionalFieldValue<bool>(fields, "activateSouth"),
                PlayerCross: GetOptionalFieldValue<bool>(fields, "playerCross"),
                PlayerUse: GetOptionalFieldValue<bool>(fields, "playerUse"),
                MonsterUse: GetOptionalFieldValue<bool>(fields, "monsterUse"),
                Repeatable: GetOptionalFieldValue<bool>(fields, "repeatable"),
                Secret: GetOptionalFieldValue<bool>(fields, "secret"),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
        public static MapData ReadMapData(IEnumerable<IGlobalExpression> ast)
        {
            Dictionary<Identifier, Token> fields = new();
            var block = new IdentifierToken(FilePosition.StartOfFile, "MapData");
            var tileBuilder = ImmutableList.CreateBuilder<Tile>();
            var sectorBuilder = ImmutableList.CreateBuilder<Sector>();
            var zoneBuilder = ImmutableList.CreateBuilder<Zone>();
            var planeBuilder = ImmutableList.CreateBuilder<Plane>();
            var planeMapBuilder = ImmutableList.CreateBuilder<PlaneMap>();
            var thingBuilder = ImmutableList.CreateBuilder<Thing>();
            var triggerBuilder = ImmutableList.CreateBuilder<Trigger>();

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
                NameSpace: GetRequiredFieldValue<string>(fields, block, "namespace"),
                TileSize: GetRequiredFieldValue<int>(fields, block, "tileSize"),
                Name: GetRequiredFieldValue<string>(fields, block, "name"),
                Width: GetRequiredFieldValue<int>(fields, block, "width"),
                Height: GetRequiredFieldValue<int>(fields, block, "height"),
                Tiles: tileBuilder.ToImmutable(),
                Sectors: sectorBuilder.ToImmutable(),
                Zones: zoneBuilder.ToImmutable(),
                Planes: planeBuilder.ToImmutable(),
                PlaneMaps: planeMapBuilder.ToImmutable(),
                Things: thingBuilder.ToImmutable(),
                Triggers: triggerBuilder.ToImmutable(),
                Comment: GetOptionalFieldValue<string>(fields, "comment")
            );
        }
    }
}
