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
                HasSubBlock("positionlessTrigger"),

            new BlockData("thingDefinition").
                DisableNormalReading().
                HasRequiredUshort("oldnum").
                HasRequiredString("actor").
                HasRequiredInteger("angles").
                HasRequiredBoolean("holowall").
                HasRequiredBoolean("pathing").
                HasRequiredBoolean("ambush").
                HasRequiredInteger("minskill"),

            new BlockData("positionlessTrigger").
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

            new BlockData("tiles",className:"TileMappings").
                DisableNormalReading().
                HasMappedSubBlocks("ambushModzone", "changeTriggerModzone", "tile", "positionlessTrigger", "zone" ).
                IsTopLevel(),

            new BlockData("things",className:"ThingMappings").
                DisableNormalReading().
                HasRequiredUshortSet("elevator").
                HasMappedSubBlocks("positionlessTrigger").
                HasSubBlockLists("thingDefinition").
                IsTopLevel(),

            new BlockData("flats",className:"FlatMappings").
                DisableNormalReading().
                HasRequiredStringList("ceiling").
                HasRequiredStringList("floor").
                IsTopLevel(),
        };
    }
}