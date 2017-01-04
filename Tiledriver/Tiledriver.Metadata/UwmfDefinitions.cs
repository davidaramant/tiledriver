// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("tile").
                HasRequiredString("textureEast").
                HasRequiredString("textureNorth").
                HasRequiredString("textureWest").
                HasRequiredString("textureSouth").
                HasOptionalBool("blockingEast", true).
                HasOptionalBool("blockingNorth", true).
                HasOptionalBool("blockingWest", true).
                HasOptionalBool("blockingSouth", true).
                HasOptionalBool("offsetVertical", false).
                HasOptionalBool("offsetHorizontal", false).
                HasOptionalBool("dontOverlay", false).
                HasOptionalInt("mapped", 0).
                HasOptionalString("soundSequence", string.Empty).
                HasOptionalString("textureOverhead", string.Empty).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("sector").
                HasRequiredString("textureCeiling").
                HasRequiredString("textureFloor").
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("zone").
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("plane").
                HasRequiredInt("depth").
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("tileSpace").
                DisableNormalWriting().
                DisableNormalReading().
                HasRequiredInt("tile").
                HasRequiredInt("sector").
                HasRequiredInt("zone").
                HasOptionalInt("tag", 0),

            new BlockData("planeMap").
                DisableNormalReading().
                HasSubBlockLists("tileSpace"),

            new BlockData("thing").
                HasRequiredString("type").
                HasRequiredDouble("x").
                HasRequiredDouble("y").
                HasRequiredDouble("z").
                HasRequiredInt("angle").
                HasOptionalBool("ambush", false).
                HasOptionalBool("patrol", false).
                HasOptionalBool("skill1", false).
                HasOptionalBool("skill2", false).
                HasOptionalBool("skill3", false).
                HasOptionalBool("skill4", false).
                HasOptionalBool("skill5", false).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("trigger").
                HasRequiredInt("x").
                HasRequiredInt("y").
                HasRequiredInt("z").
                HasRequiredString("action").
                HasOptionalInt("arg0",0).
                HasOptionalInt("arg1",0).
                HasOptionalInt("arg2",0).
                HasOptionalInt("arg3",0).
                HasOptionalInt("arg4",0).
                HasOptionalBool("activateEast", true).
                HasOptionalBool("activateNorth", true).
                HasOptionalBool("activateWest", true).
                HasOptionalBool("activateSouth", true).
                HasOptionalBool("playerCross", false).
                HasOptionalBool("playerUse", false).
                HasOptionalBool("monsterUse", false).
                HasOptionalBool("repeatable", false).
                HasOptionalBool("secret", false).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("map").
                DisableNormalReading().
                HasRequiredString("nameSpace", uwmfName:"namespace").
                HasRequiredInt("tileSize").
                HasRequiredString("name").
                HasRequiredInt("width").
                HasRequiredInt("height").
                HasOptionalString("comment", string.Empty).
                HasSubBlockLists("tile", "sector", "zone", "plane", "planeMap", "thing", "trigger").
                CanHaveUnknownProperties().
                CanHaveUnknownBlocks().
                IsTopLevel(),
        };
    }
}