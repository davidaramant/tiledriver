// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class XlatDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("ambushModzone",
                normalReading:false,
                properties:new []
                {
                    new PropertyData("fillzone", type:PropertyType.Boolean, defaultValue:false),
                }),

            new BlockData("changeTriggerModzone",
                normalReading:false,
                properties:new []
                {
                    new PropertyData("fillzone", type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData("action", type:PropertyType.String),
                    new PropertyData("positionlessTrigger", type:PropertyType.Block),
                }),

            new BlockData("thingDefinition",
                normalReading:false,
                properties:new []
                {
                    new PropertyData("actor",type:PropertyType.String),
                    new PropertyData("angles",type:PropertyType.Integer),
                    new PropertyData("holowall",type:PropertyType.Boolean),
                    new PropertyData("pathing",type:PropertyType.Boolean),
                    new PropertyData("ambush",type:PropertyType.Boolean),
                    new PropertyData("minskill",type:PropertyType.Integer),
                }),

            new BlockData("positionlessTrigger",
                properties:new []
                {
                    new PropertyData("action",PropertyType.String),
                    new PropertyData("arg0",PropertyType.Integer, defaultValue:0),
                    new PropertyData("arg1",PropertyType.Integer, defaultValue:0),
                    new PropertyData("arg2",PropertyType.Integer, defaultValue:0),
                    new PropertyData("arg3",PropertyType.Integer, defaultValue:0),
                    new PropertyData("arg4",PropertyType.Integer, defaultValue:0),
                    new PropertyData("activateEast", type:PropertyType.Boolean ,defaultValue:true),
                    new PropertyData("activateNorth", type:PropertyType.Boolean ,defaultValue:true),
                    new PropertyData("activateWest", type:PropertyType.Boolean ,defaultValue:true),
                    new PropertyData("activateSouth", type:PropertyType.Boolean ,defaultValue:true),
                    new PropertyData("playerCross", type:PropertyType.Boolean ,defaultValue:false),
                    new PropertyData("playerUse", type:PropertyType.Boolean ,defaultValue:false),
                    new PropertyData("monsterUse", type:PropertyType.Boolean ,defaultValue:false),
                    new PropertyData("repeatable", type:PropertyType.Boolean ,defaultValue:false),
                    new PropertyData("secret", type:PropertyType.Boolean ,defaultValue:false),
                    new PropertyData("comment",type:PropertyType.String,defaultValue:string.Empty), 
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("tiles",className:"TileMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new[]
                {
                    new PropertyData("ambushModzone",type:PropertyType.MappedBlockList),
                    new PropertyData("changeTriggerModzone",type:PropertyType.MappedBlockList),
                    new PropertyData("tile",type:PropertyType.MappedBlockList),
                    new PropertyData("positionlessTrigger",type:PropertyType.MappedBlockList),
                    new PropertyData("zone",type:PropertyType.MappedBlockList),
                }),

            new BlockData("things",className:"ThingMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new []
                {
                    new PropertyData("elevator",type:PropertyType.UshortSet),
                    new PropertyData("positionlessTrigger",type:PropertyType.MappedBlockList),
                    new PropertyData("thingDefinition",type:PropertyType.MappedBlockList),
                }),

            new BlockData("flats",className:"FlatMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new[]
                {
                    new PropertyData("ceiling",type:PropertyType.StringList),
                    new PropertyData("floor",type:PropertyType.StringList),
                }),
        };
    }
}