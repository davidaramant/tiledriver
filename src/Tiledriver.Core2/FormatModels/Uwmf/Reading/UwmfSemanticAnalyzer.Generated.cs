// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public static partial class UwmfSemanticAnalyzer
    {
        private static Tile ReadTile(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Tile(
                TextureEast: GetRequiredFieldValue<string>(assignments, block.Name, "textureEast"),
                TextureNorth: GetRequiredFieldValue<string>(assignments, block.Name, "textureNorth"),
                TextureWest: GetRequiredFieldValue<string>(assignments, block.Name, "textureWest"),
                TextureSouth: GetRequiredFieldValue<string>(assignments, block.Name, "textureSouth"),
                BlockingEast: GetOptionalFieldValue<bool>(assignments, "blockingEast"),
                BlockingNorth: GetOptionalFieldValue<bool>(assignments, "blockingNorth"),
                BlockingWest: GetOptionalFieldValue<bool>(assignments, "blockingWest"),
                BlockingSouth: GetOptionalFieldValue<bool>(assignments, "blockingSouth"),
                OffsetVertical: GetOptionalFieldValue<bool>(assignments, "offsetVertical"),
                OffsetHorizontal: GetOptionalFieldValue<bool>(assignments, "offsetHorizontal"),
                DontOverlay: GetOptionalFieldValue<bool>(assignments, "dontOverlay"),
                Mapped: GetOptionalFieldValue<int>(assignments, "mapped"),
                SoundSequence: GetOptionalFieldValue<string>(assignments, "soundSequence"),
                TextureOverhead: GetOptionalFieldValue<string>(assignments, "textureOverhead"),
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
        private static Sector ReadSector(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Sector(
                TextureCeiling: GetRequiredFieldValue<string>(assignments, block.Name, "textureCeiling"),
                TextureFloor: GetRequiredFieldValue<string>(assignments, block.Name, "textureFloor"),
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
        private static Zone ReadZone(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Zone(
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
        private static Plane ReadPlane(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Plane(
                Depth: GetRequiredFieldValue<int>(assignments, block.Name, "depth"),
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
        private static Thing ReadThing(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Thing(
                Type: GetRequiredFieldValue<string>(assignments, block.Name, "type"),
                X: GetRequiredDoubleFieldValue(assignments, block.Name, "x"),
                Y: GetRequiredDoubleFieldValue(assignments, block.Name, "y"),
                Z: GetRequiredDoubleFieldValue(assignments, block.Name, "z"),
                Angle: GetRequiredFieldValue<int>(assignments, block.Name, "angle"),
                Ambush: GetOptionalFieldValue<bool>(assignments, "ambush"),
                Patrol: GetOptionalFieldValue<bool>(assignments, "patrol"),
                Skill1: GetOptionalFieldValue<bool>(assignments, "skill1"),
                Skill2: GetOptionalFieldValue<bool>(assignments, "skill2"),
                Skill3: GetOptionalFieldValue<bool>(assignments, "skill3"),
                Skill4: GetOptionalFieldValue<bool>(assignments, "skill4"),
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
        private static Trigger ReadTrigger(Block block)
        {
            var assignments = block.GetFieldAssignments();

            return new Trigger(
                X: GetRequiredFieldValue<int>(assignments, block.Name, "x"),
                Y: GetRequiredFieldValue<int>(assignments, block.Name, "y"),
                Z: GetRequiredFieldValue<int>(assignments, block.Name, "z"),
                Action: GetRequiredFieldValue<string>(assignments, block.Name, "action"),
                Arg0: GetOptionalFieldValue<int>(assignments, "arg0"),
                Arg1: GetOptionalFieldValue<int>(assignments, "arg1"),
                Arg2: GetOptionalFieldValue<int>(assignments, "arg2"),
                Arg3: GetOptionalFieldValue<int>(assignments, "arg3"),
                Arg4: GetOptionalFieldValue<int>(assignments, "arg4"),
                ActivateEast: GetOptionalFieldValue<bool>(assignments, "activateEast"),
                ActivateNorth: GetOptionalFieldValue<bool>(assignments, "activateNorth"),
                ActivateWest: GetOptionalFieldValue<bool>(assignments, "activateWest"),
                ActivateSouth: GetOptionalFieldValue<bool>(assignments, "activateSouth"),
                PlayerCross: GetOptionalFieldValue<bool>(assignments, "playerCross"),
                PlayerUse: GetOptionalFieldValue<bool>(assignments, "playerUse"),
                MonsterUse: GetOptionalFieldValue<bool>(assignments, "monsterUse"),
                Repeatable: GetOptionalFieldValue<bool>(assignments, "repeatable"),
                Secret: GetOptionalFieldValue<bool>(assignments, "secret"),
                Comment: GetOptionalFieldValue<string>(assignments, "comment")
            );
        }
    }
}
