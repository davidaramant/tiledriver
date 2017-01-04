// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class XlatDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("ambushModzone").
                DisableNormalReading().
                HasOptionalBool("fillzone",false),

            new BlockData("changeTriggerModzone").
                DisableNormalReading().
                HasOptionalBool("fillzone",false).
                HasRequiredString("action").
                HasSubBlock("positionlessTrigger"),

            new BlockData("thingDefinition").
                DisableNormalReading().
                HasRequiredString("actor").
                HasRequiredInt("angles").
                HasRequiredBool("holowall").
                HasRequiredBool("pathing").
                HasRequiredBool("ambush").
                HasRequiredInt("minskill"),

            new BlockData("positionlessTrigger").
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
                HasOptionalBoolean("monsterUse", false).
                HasOptionalBoolean("repeatable", false).
                HasOptionalBoolean("secret", false).
                HasOptionalString("comment", string.Empty).
                CanHaveUnknownProperties(),

            new BlockData("tiles",className:"TileMappings").
                DisableNormalReading().
                HasMappedSubBlocks("ambushModzone", "changeTriggerModzone", "tile", "positionlessTrigger", "zone" ).
                IsTopLevel(),

            new BlockData("things",className:"ThingMappings").
                DisableNormalReading().
                HasRequiredUshortSet("elevator").
                HasMappedSubBlocks("positionlessTrigger").
                HasMappedSubBlocks("thingDefinition").
                IsTopLevel(),

            new BlockData("flats",className:"FlatMappings").
                DisableNormalReading().
                HasRequiredStringList("ceiling").
                HasRequiredStringList("floor").
                IsTopLevel(),
        };
    }
}