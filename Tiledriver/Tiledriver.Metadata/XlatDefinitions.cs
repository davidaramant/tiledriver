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
                HasSubBlock("trigger"),

            new BlockData("thingDefinition").
                DisableNormalReading().
                HasRequiredUshort("oldnum").
                HasRequiredString("actor").
                HasRequiredInteger("angles").
                HasRequiredBoolean("holowall").
                HasRequiredBoolean("pathing").
                HasRequiredInteger("minskill"),

            new BlockData("tiles",className:"TileMappings").
                DisableNormalReading().
                HasMappedSubBlocks("ambushModzone", "changeTriggerModzone", "tile", "trigger", "zone" ).
                IsTopLevel(),

            new BlockData("things",className:"ThingMappings").
                DisableNormalReading().
                HasRequiredUshortSet("elevator").
                HasMappedSubBlocks("trigger").
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