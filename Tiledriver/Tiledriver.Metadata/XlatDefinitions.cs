// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class XlatDefinitions
    {
        public static readonly IEnumerable<Block> Blocks = new[]
        {
            new Block("ambushModzone",
                normalReading:false,
                properties:new []
                {
                    new Property("fillzone", type:PropertyType.Boolean, defaultValue:false),
                }),

            new Block("changeTriggerModzone",
                normalReading:false,
                properties:new []
                {
                    new Property("fillzone", type:PropertyType.Boolean, defaultValue:false),
                    new Property("action", type:PropertyType.String),
                    new Property("positionlessTrigger", type:PropertyType.Block),
                }),

            new Block("thingDefinition",
                normalReading:false,
                properties:new []
                {
                    new Property("actor",type:PropertyType.String),
                    new Property("angles",type:PropertyType.Integer),
                    new Property("holowall",type:PropertyType.Boolean),
                    new Property("pathing",type:PropertyType.Boolean),
                    new Property("ambush",type:PropertyType.Boolean),
                    new Property("minskill",type:PropertyType.Integer),
                }),

            new Block("positionlessTrigger",
                properties:new []
                {
                    new Property("action",PropertyType.String),
                    new Property("arg0",PropertyType.Integer, defaultValue:0),
                    new Property("arg1",PropertyType.Integer, defaultValue:0),
                    new Property("arg2",PropertyType.Integer, defaultValue:0),
                    new Property("arg3",PropertyType.Integer, defaultValue:0),
                    new Property("arg4",PropertyType.Integer, defaultValue:0),
                    new Property("activateEast", type:PropertyType.Boolean ,defaultValue:true),
                    new Property("activateNorth", type:PropertyType.Boolean ,defaultValue:true),
                    new Property("activateWest", type:PropertyType.Boolean ,defaultValue:true),
                    new Property("activateSouth", type:PropertyType.Boolean ,defaultValue:true),
                    new Property("playerCross", type:PropertyType.Boolean ,defaultValue:false),
                    new Property("playerUse", type:PropertyType.Boolean ,defaultValue:false),
                    new Property("monsterUse", type:PropertyType.Boolean ,defaultValue:false),
                    new Property("repeatable", type:PropertyType.Boolean ,defaultValue:false),
                    new Property("secret", type:PropertyType.Boolean ,defaultValue:false),
                    new Property("comment",type:PropertyType.String,defaultValue:string.Empty), 
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("tiles",className:"TileMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new[]
                {
                    new Property("ambushModzone",type:PropertyType.MappedBlockList),
                    new Property("changeTriggerModzone",type:PropertyType.MappedBlockList),
                    new Property("tile",type:PropertyType.MappedBlockList),
                    new Property("positionlessTrigger",type:PropertyType.MappedBlockList),
                    new Property("zone",type:PropertyType.MappedBlockList),
                }),

            new Block("things",className:"ThingMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new []
                {
                    new Property("elevator",type:PropertyType.Set, collectionType:"ushort"),
                    new Property("positionlessTrigger",type:PropertyType.MappedBlockList),
                    new Property("thingDefinition",type:PropertyType.MappedBlockList),
                }),

            new Block("flats",className:"FlatMappings",
                normalReading:false,
                isSubBlock:false,
                properties:new[]
                {
                    new Property("ceiling",type:PropertyType.List, collectionType:"string"),
                    new Property("floor",type:PropertyType.List, collectionType:"string"),
                }),
        };
    }
}