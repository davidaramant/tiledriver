// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class XlatDefinitions
    {
        public static readonly IEnumerable<Block> Blocks = new[]
        {
            new Block("elevator",
                implements:new []{"IThingMapping"},
                properties:new []
                {
                    new Property("oldNum",PropertyType.Ushort,isMetaData:true),
                }),

            new Block("thingTemplate",
                parsing:Parsing.Manual,
                implements:new []{"IThingMapping"},
                properties:new []
                {
                    new Property("oldNum",PropertyType.Ushort,isMetaData:true),
                    new Property("type",type:PropertyType.String),
                    new Property("angles",type:PropertyType.Integer),
                    new Property("holowall",type:PropertyType.Boolean),
                    new Property("pathing",type:PropertyType.Boolean),
                    new Property("ambush",type:PropertyType.Boolean),
                    new Property("minskill",type:PropertyType.Integer),
                }),

            new Block("triggerTemplate",
                implements:new []{"IThingMapping"},
                properties:new []
                {
                    new Property("oldNum",PropertyType.Ushort,isMetaData:true),
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

            new Block("ambushModzone",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("oldNum",type:PropertyType.Ushort,isMetaData:true),
                    new Property("fillzone", type:PropertyType.Boolean, defaultValue:false),
                }),

            new Block("changeTriggerModzone",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("oldNum",type:PropertyType.Ushort,isMetaData:true),
                    new Property("fillzone", type:PropertyType.Boolean, defaultValue:false),
                    new Property("action", type:PropertyType.String),
                    new Property("triggerTemplate", type:PropertyType.Block),
                }),

            new Block("tileTemplate",
                properties:new []
                {
                    new Property("oldNum",type:PropertyType.Ushort,isMetaData:true),
                    new Property("textureEast",PropertyType.String),
                    new Property("textureNorth",PropertyType.String),
                    new Property("textureWest",PropertyType.String),
                    new Property("textureSouth",PropertyType.String),
                    new Property("blockingEast",PropertyType.Boolean, defaultValue:true),
                    new Property("blockingNorth",PropertyType.Boolean, defaultValue:true),
                    new Property("blockingWest",PropertyType.Boolean, defaultValue:true),
                    new Property("blockingSouth",PropertyType.Boolean, defaultValue:true),
                    new Property("offsetVertical",PropertyType.Boolean, defaultValue:false),
                    new Property("offsetHorizontal",PropertyType.Boolean, defaultValue:false),
                    new Property("dontOverlay",PropertyType.Boolean, defaultValue:false),
                    new Property("mapped",PropertyType.Integer,defaultValue:0),
                    new Property("soundSequence",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("textureOverhead",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("zoneTemplate",
                properties:new []
                {
                    new Property("oldNum",type:PropertyType.Ushort,isMetaData:true),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("tiles",className:"TileMappings",
                parsing:Parsing.Manual,
                isSubBlock:false,
                properties:new[]
                {
                    new Property("ambushModzone",type:PropertyType.List),
                    new Property("changeTriggerModzone",type:PropertyType.List),
                    new Property("tileTemplate",type:PropertyType.List),
                    new Property("triggerTemplate",type:PropertyType.List),
                    new Property("zoneTemplate",type:PropertyType.List),
                }),

            new Block("flats",className:"FlatMappings",
                parsing:Parsing.Manual,
                isSubBlock:false,
                properties:new[]
                {
                    new Property("ceiling",type:PropertyType.List, collectionType:"string"),
                    new Property("floor",type:PropertyType.List, collectionType:"string"),
                }),
        };
    }
}