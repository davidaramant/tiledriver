// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Text.Uwmf.Parsing.Syntax;

namespace Tiledriver.Core.FormatModels.Text.Uwmf.Parsing
{
    public static partial class Parser
    {
        static partial void SetGlobalAssignments(Map map, UwmfSyntaxTree tree)
        {
            SetRequiredString(tree.GetValueFor("Namespace"), value => map.Namespace = value, "Map", "Namespace");
            SetRequiredIntegerNumber(tree.GetValueFor("TileSize"), value => map.TileSize = value, "Map", "TileSize");
            SetRequiredString(tree.GetValueFor("Name"), value => map.Name = value, "Map", "Name");
            SetRequiredIntegerNumber(tree.GetValueFor("Width"), value => map.Width = value, "Map", "Width");
            SetRequiredIntegerNumber(tree.GetValueFor("Height"), value => map.Height = value, "Map", "Height");
            SetOptionalString(tree.GetValueFor("Comment"), value => map.Comment = value, "Map", "Comment");
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
        private static Tile ParseTile(Block block)
        {
            var parsedBlock = new Tile();
            SetRequiredString(block.GetValueFor("TextureEast"), value => parsedBlock.TextureEast = value, "Tile", "TextureEast");
            SetRequiredString(block.GetValueFor("TextureNorth"), value => parsedBlock.TextureNorth = value, "Tile", "TextureNorth");
            SetRequiredString(block.GetValueFor("TextureWest"), value => parsedBlock.TextureWest = value, "Tile", "TextureWest");
            SetRequiredString(block.GetValueFor("TextureSouth"), value => parsedBlock.TextureSouth = value, "Tile", "TextureSouth");
            SetOptionalBoolean(block.GetValueFor("BlockingEast"), value => parsedBlock.BlockingEast = value, "Tile", "BlockingEast");
            SetOptionalBoolean(block.GetValueFor("BlockingNorth"), value => parsedBlock.BlockingNorth = value, "Tile", "BlockingNorth");
            SetOptionalBoolean(block.GetValueFor("BlockingWest"), value => parsedBlock.BlockingWest = value, "Tile", "BlockingWest");
            SetOptionalBoolean(block.GetValueFor("BlockingSouth"), value => parsedBlock.BlockingSouth = value, "Tile", "BlockingSouth");
            SetOptionalBoolean(block.GetValueFor("OffsetVertical"), value => parsedBlock.OffsetVertical = value, "Tile", "OffsetVertical");
            SetOptionalBoolean(block.GetValueFor("OffsetHorizontal"), value => parsedBlock.OffsetHorizontal = value, "Tile", "OffsetHorizontal");
            SetOptionalBoolean(block.GetValueFor("DontOverlay"), value => parsedBlock.DontOverlay = value, "Tile", "DontOverlay");
            SetOptionalIntegerNumber(block.GetValueFor("Mapped"), value => parsedBlock.Mapped = value, "Tile", "Mapped");
            SetOptionalString(block.GetValueFor("SoundSequence"), value => parsedBlock.SoundSequence = value, "Tile", "SoundSequence");
            SetOptionalString(block.GetValueFor("TextureOverhead"), value => parsedBlock.TextureOverhead = value, "Tile", "TextureOverhead");
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Tile", "Comment");
            return parsedBlock;
        }
        private static Sector ParseSector(Block block)
        {
            var parsedBlock = new Sector();
            SetRequiredString(block.GetValueFor("TextureCeiling"), value => parsedBlock.TextureCeiling = value, "Sector", "TextureCeiling");
            SetRequiredString(block.GetValueFor("TextureFloor"), value => parsedBlock.TextureFloor = value, "Sector", "TextureFloor");
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Sector", "Comment");
            return parsedBlock;
        }
        private static Zone ParseZone(Block block)
        {
            var parsedBlock = new Zone();
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Zone", "Comment");
            return parsedBlock;
        }
        private static Plane ParsePlane(Block block)
        {
            var parsedBlock = new Plane();
            SetRequiredIntegerNumber(block.GetValueFor("Depth"), value => parsedBlock.Depth = value, "Plane", "Depth");
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Plane", "Comment");
            return parsedBlock;
        }
        private static Thing ParseThing(Block block)
        {
            var parsedBlock = new Thing();
            SetRequiredString(block.GetValueFor("Type"), value => parsedBlock.Type = value, "Thing", "Type");
            SetRequiredFloatingPointNumber(block.GetValueFor("X"), value => parsedBlock.X = value, "Thing", "X");
            SetRequiredFloatingPointNumber(block.GetValueFor("Y"), value => parsedBlock.Y = value, "Thing", "Y");
            SetRequiredFloatingPointNumber(block.GetValueFor("Z"), value => parsedBlock.Z = value, "Thing", "Z");
            SetRequiredIntegerNumber(block.GetValueFor("Angle"), value => parsedBlock.Angle = value, "Thing", "Angle");
            SetOptionalBoolean(block.GetValueFor("Ambush"), value => parsedBlock.Ambush = value, "Thing", "Ambush");
            SetOptionalBoolean(block.GetValueFor("Patrol"), value => parsedBlock.Patrol = value, "Thing", "Patrol");
            SetOptionalBoolean(block.GetValueFor("Skill1"), value => parsedBlock.Skill1 = value, "Thing", "Skill1");
            SetOptionalBoolean(block.GetValueFor("Skill2"), value => parsedBlock.Skill2 = value, "Thing", "Skill2");
            SetOptionalBoolean(block.GetValueFor("Skill3"), value => parsedBlock.Skill3 = value, "Thing", "Skill3");
            SetOptionalBoolean(block.GetValueFor("Skill4"), value => parsedBlock.Skill4 = value, "Thing", "Skill4");
            SetOptionalBoolean(block.GetValueFor("Skill5"), value => parsedBlock.Skill5 = value, "Thing", "Skill5");
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Thing", "Comment");
            return parsedBlock;
        }
        private static Trigger ParseTrigger(Block block)
        {
            var parsedBlock = new Trigger();
            SetRequiredIntegerNumber(block.GetValueFor("X"), value => parsedBlock.X = value, "Trigger", "X");
            SetRequiredIntegerNumber(block.GetValueFor("Y"), value => parsedBlock.Y = value, "Trigger", "Y");
            SetRequiredIntegerNumber(block.GetValueFor("Z"), value => parsedBlock.Z = value, "Trigger", "Z");
            SetRequiredString(block.GetValueFor("Action"), value => parsedBlock.Action = value, "Trigger", "Action");
            SetOptionalIntegerNumber(block.GetValueFor("Arg0"), value => parsedBlock.Arg0 = value, "Trigger", "Arg0");
            SetOptionalIntegerNumber(block.GetValueFor("Arg1"), value => parsedBlock.Arg1 = value, "Trigger", "Arg1");
            SetOptionalIntegerNumber(block.GetValueFor("Arg2"), value => parsedBlock.Arg2 = value, "Trigger", "Arg2");
            SetOptionalIntegerNumber(block.GetValueFor("Arg3"), value => parsedBlock.Arg3 = value, "Trigger", "Arg3");
            SetOptionalIntegerNumber(block.GetValueFor("Arg4"), value => parsedBlock.Arg4 = value, "Trigger", "Arg4");
            SetOptionalBoolean(block.GetValueFor("ActivateEast"), value => parsedBlock.ActivateEast = value, "Trigger", "ActivateEast");
            SetOptionalBoolean(block.GetValueFor("ActivateNorth"), value => parsedBlock.ActivateNorth = value, "Trigger", "ActivateNorth");
            SetOptionalBoolean(block.GetValueFor("ActivateWest"), value => parsedBlock.ActivateWest = value, "Trigger", "ActivateWest");
            SetOptionalBoolean(block.GetValueFor("ActivateSouth"), value => parsedBlock.ActivateSouth = value, "Trigger", "ActivateSouth");
            SetOptionalBoolean(block.GetValueFor("PlayerCross"), value => parsedBlock.PlayerCross = value, "Trigger", "PlayerCross");
            SetOptionalBoolean(block.GetValueFor("PlayerUse"), value => parsedBlock.PlayerUse = value, "Trigger", "PlayerUse");
            SetOptionalBoolean(block.GetValueFor("MonsterUse"), value => parsedBlock.MonsterUse = value, "Trigger", "MonsterUse");
            SetOptionalBoolean(block.GetValueFor("Repeatable"), value => parsedBlock.Repeatable = value, "Trigger", "Repeatable");
            SetOptionalBoolean(block.GetValueFor("Secret"), value => parsedBlock.Secret = value, "Trigger", "Secret");
            SetOptionalString(block.GetValueFor("Comment"), value => parsedBlock.Comment = value, "Trigger", "Comment");
            return parsedBlock;
        }
    }
}
