// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing
{
    public static partial class UwmfParser
    {
        static partial void SetGlobalAssignments(Map map, UwmfSyntaxTree tree)
        {
            tree.GetValueFor("NameSpace").SetRequiredString(value => map.NameSpace = value, "Map", "NameSpace");
            tree.GetValueFor("TileSize").SetRequiredInteger(value => map.TileSize = value, "Map", "TileSize");
            tree.GetValueFor("Name").SetRequiredString(value => map.Name = value, "Map", "Name");
            tree.GetValueFor("Width").SetRequiredInteger(value => map.Width = value, "Map", "Width");
            tree.GetValueFor("Height").SetRequiredInteger(value => map.Height = value, "Map", "Height");
            tree.GetValueFor("Comment").SetOptionalString(value => map.Comment = value, "Map", "Comment");
        }
        static partial void SetBlocks(Map map, UwmfSyntaxTree tree)
        {
            var tileName = new Identifier("tile");
            var sectorName = new Identifier("sector");
            var zoneName = new Identifier("zone");
            var planeName = new Identifier("plane");
            var thingName = new Identifier("thing");
            var triggerName = new Identifier("trigger");
            foreach (var block in tree.Blocks)
            {
                if (block.Name == tileName) map.Tiles.Add(ParseTile(block));
                if (block.Name == sectorName) map.Sectors.Add(ParseSector(block));
                if (block.Name == zoneName) map.Zones.Add(ParseZone(block));
                if (block.Name == planeName) map.Planes.Add(ParsePlane(block));
                if (block.Name == thingName) map.Things.Add(ParseThing(block));
                if (block.Name == triggerName) map.Triggers.Add(ParseTrigger(block));
            }
        }
        public static Tile ParseTile(IHaveAssignments block)
        {
            var parsedBlock = new Tile();
            block.GetValueFor("TextureEast").SetRequiredString(value => parsedBlock.TextureEast = value, "Tile", "TextureEast");
            block.GetValueFor("TextureNorth").SetRequiredString(value => parsedBlock.TextureNorth = value, "Tile", "TextureNorth");
            block.GetValueFor("TextureWest").SetRequiredString(value => parsedBlock.TextureWest = value, "Tile", "TextureWest");
            block.GetValueFor("TextureSouth").SetRequiredString(value => parsedBlock.TextureSouth = value, "Tile", "TextureSouth");
            block.GetValueFor("BlockingEast").SetOptionalBoolean(value => parsedBlock.BlockingEast = value, "Tile", "BlockingEast");
            block.GetValueFor("BlockingNorth").SetOptionalBoolean(value => parsedBlock.BlockingNorth = value, "Tile", "BlockingNorth");
            block.GetValueFor("BlockingWest").SetOptionalBoolean(value => parsedBlock.BlockingWest = value, "Tile", "BlockingWest");
            block.GetValueFor("BlockingSouth").SetOptionalBoolean(value => parsedBlock.BlockingSouth = value, "Tile", "BlockingSouth");
            block.GetValueFor("OffsetVertical").SetOptionalBoolean(value => parsedBlock.OffsetVertical = value, "Tile", "OffsetVertical");
            block.GetValueFor("OffsetHorizontal").SetOptionalBoolean(value => parsedBlock.OffsetHorizontal = value, "Tile", "OffsetHorizontal");
            block.GetValueFor("DontOverlay").SetOptionalBoolean(value => parsedBlock.DontOverlay = value, "Tile", "DontOverlay");
            block.GetValueFor("Mapped").SetOptionalInteger(value => parsedBlock.Mapped = value, "Tile", "Mapped");
            block.GetValueFor("SoundSequence").SetOptionalString(value => parsedBlock.SoundSequence = value, "Tile", "SoundSequence");
            block.GetValueFor("TextureOverhead").SetOptionalString(value => parsedBlock.TextureOverhead = value, "Tile", "TextureOverhead");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Tile", "Comment");
            return parsedBlock;
        }
        public static Sector ParseSector(IHaveAssignments block)
        {
            var parsedBlock = new Sector();
            block.GetValueFor("TextureCeiling").SetRequiredString(value => parsedBlock.TextureCeiling = value, "Sector", "TextureCeiling");
            block.GetValueFor("TextureFloor").SetRequiredString(value => parsedBlock.TextureFloor = value, "Sector", "TextureFloor");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Sector", "Comment");
            return parsedBlock;
        }
        public static Zone ParseZone(IHaveAssignments block)
        {
            var parsedBlock = new Zone();
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Zone", "Comment");
            return parsedBlock;
        }
        public static Plane ParsePlane(IHaveAssignments block)
        {
            var parsedBlock = new Plane();
            block.GetValueFor("Depth").SetRequiredInteger(value => parsedBlock.Depth = value, "Plane", "Depth");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Plane", "Comment");
            return parsedBlock;
        }
        public static Thing ParseThing(IHaveAssignments block)
        {
            var parsedBlock = new Thing();
            block.GetValueFor("Type").SetRequiredString(value => parsedBlock.Type = value, "Thing", "Type");
            block.GetValueFor("X").SetRequiredDouble(value => parsedBlock.X = value, "Thing", "X");
            block.GetValueFor("Y").SetRequiredDouble(value => parsedBlock.Y = value, "Thing", "Y");
            block.GetValueFor("Z").SetRequiredDouble(value => parsedBlock.Z = value, "Thing", "Z");
            block.GetValueFor("Angle").SetRequiredInteger(value => parsedBlock.Angle = value, "Thing", "Angle");
            block.GetValueFor("Ambush").SetOptionalBoolean(value => parsedBlock.Ambush = value, "Thing", "Ambush");
            block.GetValueFor("Patrol").SetOptionalBoolean(value => parsedBlock.Patrol = value, "Thing", "Patrol");
            block.GetValueFor("Skill1").SetOptionalBoolean(value => parsedBlock.Skill1 = value, "Thing", "Skill1");
            block.GetValueFor("Skill2").SetOptionalBoolean(value => parsedBlock.Skill2 = value, "Thing", "Skill2");
            block.GetValueFor("Skill3").SetOptionalBoolean(value => parsedBlock.Skill3 = value, "Thing", "Skill3");
            block.GetValueFor("Skill4").SetOptionalBoolean(value => parsedBlock.Skill4 = value, "Thing", "Skill4");
            block.GetValueFor("Skill5").SetOptionalBoolean(value => parsedBlock.Skill5 = value, "Thing", "Skill5");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Thing", "Comment");
            return parsedBlock;
        }
        public static Trigger ParseTrigger(IHaveAssignments block)
        {
            var parsedBlock = new Trigger();
            block.GetValueFor("X").SetRequiredInteger(value => parsedBlock.X = value, "Trigger", "X");
            block.GetValueFor("Y").SetRequiredInteger(value => parsedBlock.Y = value, "Trigger", "Y");
            block.GetValueFor("Z").SetRequiredInteger(value => parsedBlock.Z = value, "Trigger", "Z");
            block.GetValueFor("Action").SetRequiredString(value => parsedBlock.Action = value, "Trigger", "Action");
            block.GetValueFor("Arg0").SetOptionalInteger(value => parsedBlock.Arg0 = value, "Trigger", "Arg0");
            block.GetValueFor("Arg1").SetOptionalInteger(value => parsedBlock.Arg1 = value, "Trigger", "Arg1");
            block.GetValueFor("Arg2").SetOptionalInteger(value => parsedBlock.Arg2 = value, "Trigger", "Arg2");
            block.GetValueFor("Arg3").SetOptionalInteger(value => parsedBlock.Arg3 = value, "Trigger", "Arg3");
            block.GetValueFor("Arg4").SetOptionalInteger(value => parsedBlock.Arg4 = value, "Trigger", "Arg4");
            block.GetValueFor("ActivateEast").SetOptionalBoolean(value => parsedBlock.ActivateEast = value, "Trigger", "ActivateEast");
            block.GetValueFor("ActivateNorth").SetOptionalBoolean(value => parsedBlock.ActivateNorth = value, "Trigger", "ActivateNorth");
            block.GetValueFor("ActivateWest").SetOptionalBoolean(value => parsedBlock.ActivateWest = value, "Trigger", "ActivateWest");
            block.GetValueFor("ActivateSouth").SetOptionalBoolean(value => parsedBlock.ActivateSouth = value, "Trigger", "ActivateSouth");
            block.GetValueFor("PlayerCross").SetOptionalBoolean(value => parsedBlock.PlayerCross = value, "Trigger", "PlayerCross");
            block.GetValueFor("PlayerUse").SetOptionalBoolean(value => parsedBlock.PlayerUse = value, "Trigger", "PlayerUse");
            block.GetValueFor("MonsterUse").SetOptionalBoolean(value => parsedBlock.MonsterUse = value, "Trigger", "MonsterUse");
            block.GetValueFor("Repeatable").SetOptionalBoolean(value => parsedBlock.Repeatable = value, "Trigger", "Repeatable");
            block.GetValueFor("Secret").SetOptionalBoolean(value => parsedBlock.Secret = value, "Trigger", "Secret");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Trigger", "Comment");
            return parsedBlock;
        }
    }
}
