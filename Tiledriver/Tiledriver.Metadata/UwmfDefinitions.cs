// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class UwmfDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("tile",
                properties:new []
                {
                    new PropertyData("textureEast",PropertyType.String),
                    new PropertyData("textureNorth",PropertyType.String),
                    new PropertyData("textureWest",PropertyType.String),
                    new PropertyData("textureSouth",PropertyType.String),
                    new PropertyData("blockingEast",PropertyType.Boolean, defaultValue:true),
                    new PropertyData("blockingNorth",PropertyType.Boolean, defaultValue:true),
                    new PropertyData("blockingWest",PropertyType.Boolean, defaultValue:true),
                    new PropertyData("blockingSouth",PropertyType.Boolean, defaultValue:true),
                    new PropertyData("offsetVertical",PropertyType.Boolean, defaultValue:false),
                    new PropertyData("offsetHorizontal",PropertyType.Boolean, defaultValue:false),
                    new PropertyData("dontOverlay",PropertyType.Boolean, defaultValue:false),
                    new PropertyData("mapped",PropertyType.Integer,defaultValue:0),
                    new PropertyData("soundSequence",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("textureOverhead",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("sector",
                properties:new []
                {
                    new PropertyData("textureCeiling",type:PropertyType.String),
                    new PropertyData("textureFloor",type:PropertyType.String),
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("zone",
                properties:new []
                {
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("plane",
                properties:new []
                {
                    new PropertyData("depth",PropertyType.Integer),
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("tileSpace",
                normalReading:false,
                normalWriting:false,
                properties:new []
                {
                    new PropertyData("tile",PropertyType.Integer),
                    new PropertyData("sector",PropertyType.Integer),
                    new PropertyData("zone",PropertyType.Integer),
                    new PropertyData("tag",PropertyType.Integer,defaultValue:0),
                }),

            new BlockData("planeMap",
                normalReading:false,
                properties:new []
                {
                    new PropertyData("tileSpace",PropertyType.List),
                }),

            new BlockData("thing",
                properties:new []
                {
                    new PropertyData("type",PropertyType.String),
                    new PropertyData("x",PropertyType.Double),
                    new PropertyData("y",PropertyType.Double),
                    new PropertyData("z",PropertyType.Double),
                    new PropertyData("angle",PropertyType.Integer),
                    new PropertyData("ambush", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("patrol", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("skill1", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("skill2", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("skill3", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("skill4", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("skill5", PropertyType.Boolean, defaultValue:false),
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("trigger",
                properties:new []
                {
                    new PropertyData("x",PropertyType.Integer),
                    new PropertyData("y",PropertyType.Integer),
                    new PropertyData("z",PropertyType.Integer),
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
                    new PropertyData("secret", type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData("comment",type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                }),

            new BlockData("map",
                normalReading:false,
                isSubBlock:false,
                properties:new []
                {
                    new PropertyData("nameSpace",formatName:"namespace", type:PropertyType.String),
                    new PropertyData("tileSize", type:PropertyType.Integer),
                    new PropertyData("name", type:PropertyType.String),
                    new PropertyData("width", type:PropertyType.Integer),
                    new PropertyData("height", type:PropertyType.Integer),
                    new PropertyData("comment", type:PropertyType.String, defaultValue:string.Empty),
                    new PropertyData("tile",type:PropertyType.List),
                    new PropertyData("sector",type:PropertyType.List),
                    new PropertyData("zone",type:PropertyType.List),
                    new PropertyData("plane",type:PropertyType.List),
                    new PropertyData("planeMap",type:PropertyType.List),
                    new PropertyData("thing",type:PropertyType.List),
                    new PropertyData("trigger",type:PropertyType.List),
                    new PropertyData("unknownProperties", PropertyType.UnknownProperties),
                    new PropertyData("unknownBlocks", PropertyType.UnknownBlocks),
                }),
        };
    }
}