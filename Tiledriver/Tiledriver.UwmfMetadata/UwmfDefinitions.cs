// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.UwmfMetadata
{
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<UwmfBlock> Blocks = new[]
        {
            new UwmfBlock("tile").
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
                HasOptionalString("soundSequence", String.Empty).
                HasOptionalString("textureOverhead", String.Empty).
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("sector").
                HasRequiredString("textureCeiling").
                HasRequiredString("textureFloor").
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("zone").
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("plane").
                HasRequiredIntegerNumber("depth").
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("tileSpace").
                HasRequiredIntegerNumber("tile").
                HasRequiredIntegerNumber("sector").
                HasRequiredIntegerNumber("zone").
                HasOptionalIntegerNumber("tag", 0).
                DisableNormalWriting().
                DisableNormalReading(),
            new UwmfBlock("planeMap").
                HasSubBlocks("tileSpace").
                DisableNormalReading(),
            new UwmfBlock("thing").
                HasRequiredIntegerNumber("type").
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
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("trigger").
                HasRequiredIntegerNumber("x").
                HasRequiredIntegerNumber("y").
                HasRequiredIntegerNumber("z").
                HasRequiredIntegerNumber("action").
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
                HasOptionalString("comment", String.Empty),
            new UwmfBlock("map").
                HasRequiredString("namespace").
                HasRequiredIntegerNumber("tileSize").
                HasRequiredString("name").
                HasRequiredIntegerNumber("width").
                HasRequiredIntegerNumber("height").
                HasSubBlocks("tile", "sector", "zone", "plane", "planeMap", "thing", "trigger").
                IsTopLevel(),
        };
    }
}