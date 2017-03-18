// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<Block> Blocks = new[]
        {
            new Block("tile",
                properties:new []
                {
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

            new Block("sector",
                properties:new []
                {
                    new Property("textureCeiling",type:PropertyType.String),
                    new Property("textureFloor",type:PropertyType.String),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("zone",
                properties:new []
                {
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("plane",
                properties:new []
                {
                    new Property("depth",PropertyType.Integer),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("tileSpace",
                parsing:Parsing.Manual,
                normalWriting:false,
                properties:new []
                {
                    new Property("tile",PropertyType.Integer),
                    new Property("sector",PropertyType.Integer),
                    new Property("zone",PropertyType.Integer),
                    new Property("tag",PropertyType.Integer,defaultValue:0),
                }),

            new Block("planeMap",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("tileSpace",PropertyType.List),
                }),

            new Block("thing",
                properties:new []
                {
                    new Property("type",PropertyType.String),
                    new Property("x",PropertyType.Double),
                    new Property("y",PropertyType.Double),
                    new Property("z",PropertyType.Double),
                    new Property("angle",PropertyType.Integer),
                    new Property("ambush", PropertyType.Boolean, defaultValue:false),
                    new Property("patrol", PropertyType.Boolean, defaultValue:false),
                    new Property("skill1", PropertyType.Boolean, defaultValue:false),
                    new Property("skill2", PropertyType.Boolean, defaultValue:false),
                    new Property("skill3", PropertyType.Boolean, defaultValue:false),
                    new Property("skill4", PropertyType.Boolean, defaultValue:false),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("trigger",
                properties:new []
                {
                    new Property("x",PropertyType.Integer),
                    new Property("y",PropertyType.Integer),
                    new Property("z",PropertyType.Integer),
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
                    new Property("secret", type:PropertyType.Boolean, defaultValue:false),
                    new Property("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                }),

            new Block("mapData",
                parsing:Parsing.Manual,
                isSubBlock:false,
                properties:new []
                {
                    new Property("nameSpace",formatName:"namespace", type:PropertyType.String),
                    new Property("tileSize", type:PropertyType.Integer),
                    new Property("name", type:PropertyType.String),
                    new Property("width", type:PropertyType.Integer),
                    new Property("height", type:PropertyType.Integer),
                    new Property("comment", type:PropertyType.String, defaultValue:string.Empty),
                    new Property("tile",type:PropertyType.List),
                    new Property("sector",type:PropertyType.List),
                    new Property("zone",type:PropertyType.List),
                    new Property("plane",type:PropertyType.List),
                    new Property("planeMap",type:PropertyType.List),
                    new Property("thing",type:PropertyType.List),
                    new Property("trigger",type:PropertyType.List),
                    new Property("unknownProperties", PropertyType.UnknownProperties),
                    new Property("unknownBlocks", PropertyType.UnknownBlocks),
                }),
        };
    }
}