// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Metadata
{
    public static class MapInfoDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            new BlockData("cluster",
                properties:new []
                {
                    new PropertyData("id",type:PropertyType.Integer,isMetaData:true),
                    new PropertyData(formatName:"exittext",name:"exitText",type:PropertyType.String),
                    new PropertyData(formatName:"exittextlookup",name:"exitTextLookup",type:PropertyType.String),
                    new PropertyData(formatName:"exittextislump",name:"exitTextIsLump",type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData(formatName:"exittextismessage",name:"exitTextIsMessage",type:PropertyType.Boolean, defaultValue:false),
                }),

            new BlockData("episode",
                properties:new []
                {
                    new PropertyData("map",type:PropertyType.String,isMetaData:true),
                    new PropertyData("key",type:PropertyType.Char),
                    new PropertyData("lookup",type:PropertyType.String),
                    new PropertyData("name",type:PropertyType.String),
                    new PropertyData(formatName:"noskillmenu",name:"noSkillMenu",type:PropertyType.Boolean,defaultValue:false),
                    new PropertyData("optional",type:PropertyType.Boolean,defaultValue:false),
                    new PropertyData(formatName:"picname",name:"picName",type:PropertyType.String),
                    new PropertyData("remove",type:PropertyType.Boolean,defaultValue:false),
                }),

            // gameinfo
            new BlockData("gameinfo",className:"gameInfo",
                properties:new[]
                {
                    new PropertyData(formatName:"advisorycolor", name:"advisoryColor", type:PropertyType.String),
                    new PropertyData(formatName:"advisorypic", name:"advisoryPic", type:PropertyType.String),
                    new PropertyData("playScreenBorderColors", type:PropertyType.Block),
                    new PropertyData("playScreenBorderGraphics", type:PropertyType.Block),
                    new PropertyData(formatName:"borderflat", name:"borderFlat", type:PropertyType.String),
                    new PropertyData(formatName:"doorsoundsequence", name:"doorSoundSequence", type:PropertyType.String),
                    new PropertyData(formatName:"drawreadthis", name:"drawReadThis", type:PropertyType.Boolean),
                    new PropertyData(formatName:"finalemusic", name:"finaleMusic", type:PropertyType.String),
                    new PropertyData(formatName:"gamepalette", name:"gamePalette", type:PropertyType.String),
                    new PropertyData(formatName:"gibfactor", name:"gibFactor", type:PropertyType.Double),
                    new PropertyData(formatName:"highscoresfont", name:"highScoresFont", type:PropertyType.String),
                    new PropertyData(formatName:"highscoresfontcolor", name:"highScoresFontColor", type:PropertyType.String),
                    new PropertyData(formatName:"intermissionmusic", name:"intermissionMusic", type:PropertyType.String),
                    new PropertyData(formatName:"menucolor", name:"menuColor", type:PropertyType.String), // = "<border1>", "<border2>", <border3>", "<background>", "<stripe>", "<stripebg>"
                    new PropertyData(formatName:"menufade", name:"menuFade", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_disabled", name:"menuFontColorDisabled", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_highlight", name:"menuFontColorHighlight", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_highlightselection", name:"menuFontColorHighlightSelection", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_invalid", name:"menuFontColorInvalid", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_invalidselection", name:"menuFontColorInvalidSelection", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_label", name:"menuFontColorLabel", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_selection", name:"menuFontColorSelection", type:PropertyType.String),
                    new PropertyData(formatName:"menufontcolor_title", name:"menuFontColorTitle", type:PropertyType.String),
                    new PropertyData(formatName:"menumusic", name:"menuMusic", type:PropertyType.String),
                    new PropertyData(formatName:"pageindexfontcolor", name:"pageIndexFontColor", type:PropertyType.String),
                    new PropertyData(formatName:"playerclasses", name:"playerClasses", type:PropertyType.List, collectionType:"string"),
                    new PropertyData(formatName:"pushwallsoundsequence", name:"pushwallSoundSequence", type:PropertyType.String),
                    new PropertyData(formatName:"quitmessages", name:"quitMessages", type:PropertyType.List, collectionType:"string"),
                    new PropertyData(formatName:"scoresmusic", name:"scoresMusic", type:PropertyType.String),
                    new PropertyData(formatName:"signon", name:"signOn", type:PropertyType.String),
                    new PropertyData(formatName:"titlemusic", name:"titleMusic", type:PropertyType.String),
                    new PropertyData(formatName:"titletime", name:"titleTime", type:PropertyType.Integer),
                    new PropertyData("translator", type:PropertyType.String),
                }),

            new BlockData("playScreenBorderColors",
                properties:new[]
                {
                    new PropertyData("topColor",type:PropertyType.String),
                    new PropertyData("bottomColor",type:PropertyType.String),
                    new PropertyData("highlightColor",type:PropertyType.String),
                }),

            new BlockData("playScreenBorderGraphics",
                properties:new[]
                {
                    new PropertyData("topLeft",type:PropertyType.String),
                    new PropertyData("top",type:PropertyType.String),
                    new PropertyData("topRight",type:PropertyType.String),
                    new PropertyData("left",type:PropertyType.String),
                    new PropertyData("right",type:PropertyType.String),
                    new PropertyData("bottomLeft",type:PropertyType.String),
                    new PropertyData("bottom",type:PropertyType.String),
                    new PropertyData("bottomRight",type:PropertyType.String),
                }),

            new BlockData("menuColor",
                properties:new[]
                {
                    new PropertyData("border1",type:PropertyType.String),
                    new PropertyData("border2",type:PropertyType.String),
                    new PropertyData("border3",type:PropertyType.String),
                    new PropertyData("background",type:PropertyType.String),
                    new PropertyData("stripe",type:PropertyType.String),
                    new PropertyData("stripeBg",type:PropertyType.String),
                }),

            // intermission
            new BlockData("intermission",
                properties:new []
                {
                    new PropertyData("name",type:PropertyType.String,isMetaData:true),
                    new PropertyData("intermissionAction",type:PropertyType.List,collectionType:"IIntermissionAction"),
                }),

            new BlockData("IntermissionAction",
                isAbstract: true,
                properties:new[]
                {
                    new PropertyData("background",type:PropertyType.String),
                    new PropertyData("backgroundTiled",type:PropertyType.Boolean),
                    new PropertyData("backgroundPalette",type:PropertyType.String),
                    new PropertyData("draw",type:PropertyType.String),
                    new PropertyData("drawX",type:PropertyType.Integer),
                    new PropertyData("drawY",type:PropertyType.Integer),
                    new PropertyData("music",type:PropertyType.String),
                    new PropertyData("time",type:PropertyType.Integer),
                }),

            new BlockData("Fader",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:new[]
                {
                    new PropertyData(formatName:"fadetype",name:"fadeType",type:PropertyType.String),
                }),

            new BlockData(formatName:"GotoTitle",className:"GoToTitle",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<PropertyData>()),

            new BlockData("Image",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<PropertyData>()),

            new BlockData("TextScreen",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:new[]
                {
                    new PropertyData("text",type:PropertyType.List, collectionType:"string"),
                    new PropertyData(formatName:"textalignment",name:"textAlignment",type:PropertyType.String),
                    new PropertyData(formatName:"textcolor",name:"textColor",type:PropertyType.String),
                    new PropertyData(formatName:"textspeed",name:"textSpeed",type:PropertyType.Integer),
                    new PropertyData("positionX",type:PropertyType.Integer),
                    new PropertyData("positionY",type:PropertyType.Integer),
                }),

            new BlockData("VictoryStats",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<PropertyData>()), 


            // map
            new BlockData("defaultmap",className:"DefaultMap",
                inheritsFrom:"BaseMap",
                canSetPropertiesFrom:new [] {"AddDefaultMap"},
                properties:Enumerable.Empty<PropertyData>()),
            new BlockData("adddefaultmap",className:"AddDefaultMap",
                inheritsFrom:"BaseMap",
                properties:Enumerable.Empty<PropertyData>()),
            new BlockData("BaseMap",
                isAbstract: true,
                properties:new []
                {
                    new PropertyData(formatName:"bordertexture",name:"borderTexture",type:PropertyType.String),
                    new PropertyData("cluster",type:PropertyType.Integer),
                    new PropertyData(formatName:"completionstring",name:"completionString",type:PropertyType.String),
                    new PropertyData(formatName:"deathcam",name:"deathCam",type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData(formatName:"defaultceiling",name:"defaultCeiling",type:PropertyType.String),
                    new PropertyData(formatName:"defaultfloor",name:"defaultFloor",type:PropertyType.String),
                    new PropertyData(formatName:"ensureinventory",name:"ensureInventory",type:PropertyType.List, collectionType:"string"),
                    new PropertyData(formatName:"exitfade",name:"exitFade",type:PropertyType.Integer),
                    new PropertyData(formatName:"floornumber",name:"floorNumber",type:PropertyType.Integer),
                    new PropertyData(formatName:"highscoresgraphic",name:"highScoresGraphic",type:PropertyType.String),
                    new PropertyData(formatName:"levelbonus",name:"levelBonus",type:PropertyType.Integer),
                    new PropertyData(formatName:"levelnum",name:"levelNum",type:PropertyType.Integer),
                    new PropertyData("music",type:PropertyType.String),
                    new PropertyData(formatName:"spawnwithweaponraised",name:"spawnWithWeaponRaised",type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData(formatName:"secretdeathsounds",name:"secretDeathSounds",type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData("next",type:PropertyType.String),
                    new PropertyData(formatName:"secretnext",name:"secretNext",type:PropertyType.String),
                    new PropertyData(formatName:"victorynext",name:"victoryNext",type:PropertyType.String),
                    new PropertyData(formatName:"nextendsequence",name:"nextEndSequence",type:PropertyType.String),
                    new PropertyData(formatName:"secretnextendsequence",name:"secretNextEndSequence",type:PropertyType.String),
                    new PropertyData(formatName:"victorynextendsequence",name:"victoryNextEndSequence",type:PropertyType.String),
                    new PropertyData(formatName:"specialaction",name:"specialAction",type:PropertyType.List),
                    new PropertyData(formatName:"nointermission",name:"nointermission",type:PropertyType.Boolean, defaultValue:false),
                    new PropertyData("par",type:PropertyType.Integer),
                    new PropertyData("translator",type:PropertyType.String),
                }),
            new BlockData("map",
                inheritsFrom:"BaseMap",
                canSetPropertiesFrom:new [] {"DefaultMap"},
                properties:new []
                {
                    new PropertyData("mapLump",type:PropertyType.String,isMetaData:true),
                    new PropertyData("mapName",type:PropertyType.String,isMetaData:true),
                    new PropertyData("mapNameLookup",type:PropertyType.String,isMetaData:true),
                }),

            new BlockData("specialaction",className: "SpecialAction",
                properties:new []
                {
                    new PropertyData(formatName:"actorclass",name:"actorClass",type:PropertyType.String),
                    new PropertyData("special",PropertyType.String),
                    new PropertyData("arg0",PropertyType.String),
                    new PropertyData("arg1",PropertyType.String),
                    new PropertyData("arg2",PropertyType.String),
                    new PropertyData("arg3",PropertyType.String),
                }),

            new BlockData("mapInfo",
                isSubBlock:false,
                normalReading:false,
                properties:new []
                {
                    new PropertyData("cluster",type:PropertyType.List),
                    new PropertyData("episode",type:PropertyType.List),
                    new PropertyData("gameInfo",formatName:"gameinfo",type:PropertyType.Block),
                    new PropertyData("intermission",type:PropertyType.List),
                    new PropertyData("map",type:PropertyType.List),
                }),
        };
    }
}
