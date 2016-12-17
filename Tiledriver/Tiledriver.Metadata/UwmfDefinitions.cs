// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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
                HasOptionalBoolean("blockingEast", true).
                HasOptionalBoolean("blockingNorth", true).
                HasOptionalBoolean("blockingWest", true).
                HasOptionalBoolean("blockingSouth", true).
                HasOptionalBoolean("offsetVertical", false).
                HasOptionalBoolean("offsetHorizontal", false).
                HasOptionalBoolean("dontOverlay", false).
                HasOptionalIntegerNumber("mapped", 0).
                HasOptionalString("soundSequence", string.Empty).
                HasOptionalString("textureOverhead", string.Empty).
                HasOptionalString("comment", string.Empty),

            new BlockData("sector").
                HasRequiredString("textureCeiling").
                HasRequiredString("textureFloor").
                HasOptionalString("comment", string.Empty),

            new BlockData("zone").
                HasOptionalString("comment", string.Empty),

            new BlockData("plane").
                HasRequiredIntegerNumber("depth").
                HasOptionalString("comment", string.Empty),

            new BlockData("tileSpace").
                DisableNormalWriting().
                DisableNormalReading().
                HasRequiredIntegerNumber("tile").
                HasRequiredIntegerNumber("sector").
                HasRequiredIntegerNumber("zone").
                HasOptionalIntegerNumber("tag", 0).
                CannotHaveUnknownProperties(),

            new BlockData("planeMap").
                DisableNormalReading().
                HasSubBlocks("tileSpace").
                CannotHaveUnknownProperties(),

            new BlockData("thing").
                HasRequiredString("type").
                HasRequiredFloatingPointNumber("x").
                HasRequiredFloatingPointNumber("y").
                HasRequiredFloatingPointNumber("z").
                HasRequiredIntegerNumber("angle").
                HasOptionalBoolean("ambush", false).
                HasOptionalBoolean("patrol", false).
                HasOptionalBoolean("skill1", false).
                HasOptionalBoolean("skill2", false).
                HasOptionalBoolean("skill3", false).
                HasOptionalBoolean("skill4", false).
                HasOptionalBoolean("skill5", false).
                HasOptionalString("comment", string.Empty),

            new BlockData("trigger").
                HasRequiredIntegerNumber("x").
                HasRequiredIntegerNumber("y").
                HasRequiredIntegerNumber("z").
                HasRequiredString("action").
                HasOptionalIntegerNumber("arg0",0).
                HasOptionalIntegerNumber("arg1",0).
                HasOptionalIntegerNumber("arg2",0).
                HasOptionalIntegerNumber("arg3",0).
                HasOptionalIntegerNumber("arg4",0).
                HasOptionalBoolean("activateEast", true).
                HasOptionalBoolean("activateNorth", true).
                HasOptionalBoolean("activateWest", true).
                HasOptionalBoolean("activateSouth", true).
                HasOptionalBoolean("playerCross", false).
                HasOptionalBoolean("playerUse", false).
                HasOptionalBoolean("monsterUse", false).
                HasOptionalBoolean("repeatable", false).
                HasOptionalBoolean("secret", false).
                HasOptionalString("comment", string.Empty),

            new BlockData("map").
                DisableNormalReading().
                HasRequiredString("nameSpace", uwmfName:"namespace").
                HasRequiredIntegerNumber("tileSize").
                HasRequiredString("name").
                HasRequiredIntegerNumber("width").
                HasRequiredIntegerNumber("height").
                HasOptionalString("comment", string.Empty).
                HasSubBlocks("tile", "sector", "zone", "plane", "planeMap", "thing", "trigger").
                IsTopLevel(),
        };
    }
}