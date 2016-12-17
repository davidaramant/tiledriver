// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public sealed class XlatDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("ambushModzone").
                DisableNormalReading().
                HasOptionalBoolean("fillzone",false),

            new BlockData("changeTriggerModzone").
                DisableNormalReading().
                HasOptionalBoolean("fillzone",false).
                HasRequiredString("action").
                HasSubBlocks("trigger"),

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

            new BlockData("trigger").
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

            new BlockData("zone").
                HasOptionalString("comment", string.Empty),

            new BlockData("elevator"),

            new BlockData("thingDefinition").
                DisableNormalReading().
                HasRequiredIntegerNumber("oldnum").
                HasRequiredString("actor").
                HasRequiredIntegerNumber("angles").
                HasRequiredBoolean("holowall").
                HasRequiredBoolean("pathing").
                HasRequiredIntegerNumber("minskill"),

            new BlockData("ceiling").
                DisableNormalReading().
                HasRequiredString("flat"),

            new BlockData("floor").
                DisableNormalReading().
                HasRequiredString("flat"),

            new BlockData("tiles").
                HasSubBlocks("tile", "trigger", "zone" ).
                IsTopLevel(),

            new BlockData("things").
                HasSubBlocks("elevator", "trigger", "thingDefinition").
                IsTopLevel(),

            new BlockData("flats").
                DisableNormalReading().
                HasSubBlocks("ceiling", "floor").
                IsTopLevel(),
        };
    }
}