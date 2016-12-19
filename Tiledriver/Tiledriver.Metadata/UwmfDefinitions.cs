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
                HasOptionalInteger("mapped", 0).
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
                HasRequiredInteger("depth").
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("tileSpace").
                DisableNormalWriting().
                DisableNormalReading().
                HasRequiredInteger("tile").
                HasRequiredInteger("sector").
                HasRequiredInteger("zone").
                HasOptionalInteger("tag", 0),

            new BlockData("planeMap").
                DisableNormalReading().
                HasSubBlockLists("tileSpace"),

            new BlockData("thing").
                HasRequiredString("type").
                HasRequiredDouble("x").
                HasRequiredDouble("y").
                HasRequiredDouble("z").
                HasRequiredInteger("angle").
                HasOptionalBoolean("ambush", false).
                HasOptionalBoolean("patrol", false).
                HasOptionalBoolean("skill1", false).
                HasOptionalBoolean("skill2", false).
                HasOptionalBoolean("skill3", false).
                HasOptionalBoolean("skill4", false).
                HasOptionalBoolean("skill5", false).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("trigger").
                HasRequiredInteger("x").
                HasRequiredInteger("y").
                HasRequiredInteger("z").
                HasRequiredString("action").
                HasOptionalInteger("arg0",0).
                HasOptionalInteger("arg1",0).
                HasOptionalInteger("arg2",0).
                HasOptionalInteger("arg3",0).
                HasOptionalInteger("arg4",0).
                HasOptionalBoolean("activateEast", true).
                HasOptionalBoolean("activateNorth", true).
                HasOptionalBoolean("activateWest", true).
                HasOptionalBoolean("activateSouth", true).
                HasOptionalBoolean("playerCross", false).
                HasOptionalBoolean("playerUse", false).
                HasOptionalBoolean("monsterUse", false).
                HasOptionalBoolean("repeatable", false).
                HasOptionalBoolean("secret", false).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("map").
                DisableNormalReading().
                HasRequiredString("nameSpace", uwmfName:"namespace").
                HasRequiredInteger("tileSize").
                HasRequiredString("name").
                HasRequiredInteger("width").
                HasRequiredInteger("height").
                HasOptionalString("comment", string.Empty).
                HasSubBlockLists("tile", "sector", "zone", "plane", "planeMap", "thing", "trigger").
                CanHaveUnknownProperties().
                CanHaveUnknownBlocks().
                IsTopLevel(),
        };
    }
}