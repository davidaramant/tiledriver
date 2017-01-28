// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Metadata
{
    public static class MapInfoDefinitions
    {
        public static List<Property> GetAllPropertiesOf(string className)
        {
            return GetAllPropertiesOf(Blocks.Single(b => b.ClassName == className));
        }

        public static List<Property> GetAllPropertiesOf(Block block)
        {
            var allProperties = new List<Property>();

            if (block.BaseClass.HasValue)
            {
                allProperties.AddRange(GetAllPropertiesOf(block.BaseClass.Value));
            }
            allProperties.AddRange(block.Properties);

            return allProperties;
        }

        public static readonly IEnumerable<Block> Blocks = new[]
        {
            new Block("cluster",
                isSubBlock:false,
                properties:new []
                {
                    new Property("id",type:PropertyType.Integer,isMetaData:true),
                    new Property(formatName:"exittext",name:"exitText",type:PropertyType.Block, collectionType:"ClusterExitText"),
                    new Property(formatName:"exittextislump",name:"exitTextIsLump",type:PropertyType.Flag, defaultValue:false),
                    new Property(formatName:"exittextismessage",name:"exitTextIsMessage",type:PropertyType.Flag, defaultValue:false),
                }),

            new Block("ClusterExitText",
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("text",type:PropertyType.String),
                    new Property("lookup",type:PropertyType.Boolean),
                }),

            // There is also 'clearepisodes'
            new Block("episode",
                isSubBlock:false,
                properties:new []
                {
                    new Property("map",type:PropertyType.String,isMetaData:true),
                    new Property("key",type:PropertyType.Char),
                    new Property("lookup",type:PropertyType.String),
                    new Property("name",type:PropertyType.String),
                    new Property(formatName:"noskillmenu",name:"noSkillMenu",type:PropertyType.Flag,defaultValue:false),
                    new Property("optional",type:PropertyType.Flag,defaultValue:false),
                    new Property(formatName:"picname",name:"picName",type:PropertyType.String),
                    new Property("remove",type:PropertyType.Flag,defaultValue:false),
                }),

            // gameinfo
            new Block("gameinfo",className:"GameInfo",
                isSubBlock:false,
                canSetPropertiesFrom:new []{"GameInfo"},
                properties:new[]
                {
                    new Property(formatName:"advisorycolor", name:"advisoryColor", type:PropertyType.String),
                    new Property(formatName:"advisorypic", name:"advisoryPic", type:PropertyType.String),
                    new Property(formatName:"border", name:"border", type:PropertyType.Block, collectionType:"GameBorder"),
                    new Property(formatName:"borderflat", name:"borderFlat", type:PropertyType.String),
                    new Property(formatName:"deathtransition", name:"deathTransition", type:PropertyType.String),
                    new Property(formatName:"dialogcolor", name:"dialogColor", type:PropertyType.String),
                    new Property(formatName:"doorsoundsequence", name:"doorSoundSequence", type:PropertyType.String),
                    new Property(formatName:"drawreadthis", name:"drawReadThis", type:PropertyType.Boolean),
                    new Property(formatName:"finaleflat", name:"finaleFlat", type:PropertyType.String),
                    new Property(formatName:"finalemusic", name:"finaleMusic", type:PropertyType.String),
                    new Property(formatName:"gamecolormap", name:"gameColorMap", type:PropertyType.String),
                    new Property(formatName:"gameoverpic", name:"gameOverPic", type:PropertyType.String),
                    new Property(formatName:"gamepalette", name:"gamePalette", type:PropertyType.String),
                    new Property(formatName:"gibfactor", name:"gibFactor", type:PropertyType.Double),
                    new Property(formatName:"highscoresfont", name:"highScoresFont", type:PropertyType.String),
                    new Property(formatName:"highscoresfontcolor", name:"highScoresFontColor", type:PropertyType.String),
                    new Property(formatName:"intermissionmusic", name:"intermissionMusic", type:PropertyType.String),
                    new Property(formatName:"menucolors", name:"MenuColors", type:PropertyType.Block),
                    new Property(formatName:"menufade", name:"menuFade", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_disabled", name:"menuFontColorDisabled", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_highlight", name:"menuFontColorHighlight", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_highlightselection", name:"menuFontColorHighlightSelection", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_invalid", name:"menuFontColorInvalid", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_invalidselection", name:"menuFontColorInvalidSelection", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_label", name:"menuFontColorLabel", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_selection", name:"menuFontColorSelection", type:PropertyType.String),
                    new Property(formatName:"menufontcolor_title", name:"menuFontColorTitle", type:PropertyType.String),
                    new Property(formatName:"menumusic", name:"menuMusic", type:PropertyType.String),
                    new Property(formatName:"menuwindowcolors", name:"menuWindowColors", type:PropertyType.Block),
                    new Property(formatName:"messagecolors", name:"messageColors", type:PropertyType.Block),
                    new Property(formatName:"messagefontcolor", name:"messageFontColor", type:PropertyType.String),
                    new Property(formatName:"pageindexfontcolor", name:"pageIndexFontColor", type:PropertyType.String),
                    new Property(formatName:"playerclasses", name:"PlayerClasses",singularName:"PlayerClass", type:PropertyType.ImmutableList, collectionType:"string"),
                    new Property(formatName:"psyched", name:"psyched", type:PropertyType.Block),
                    new Property(formatName:"pushwallsoundsequence", name:"pushwallSoundSequence", type:PropertyType.String),
                    new Property(formatName:"quitmessages", name:"QuitMessages",singularName:"QuitMessage", type:PropertyType.ImmutableList, collectionType:"string"),
                    new Property(formatName:"scoresmusic", name:"scoresMusic", type:PropertyType.String),
                    new Property(formatName:"signon", name:"signOn", type:PropertyType.String),
                    new Property(formatName:"titlemusic", name:"titleMusic", type:PropertyType.String),
                    new Property(formatName:"titlepage", name:"titlePage", type:PropertyType.String),
                    new Property(formatName:"titlepalette", name:"titlePalette", type:PropertyType.String),
                    new Property(formatName:"titletime", name:"titleTime", type:PropertyType.Integer),
                    new Property(formatName:"trackhighscores", name:"trackHighScores", type:PropertyType.Boolean),
                    new Property(formatName:"translator", name:"translator", type:PropertyType.String),
                    new Property(formatName:"victorymusic", name:"victoryMusic", type:PropertyType.String),
                    new Property(formatName:"victorypic", name:"victoryPic", type:PropertyType.String),
                }),

            new Block("GameBorder",
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("Colors",type:PropertyType.Block,collectionType:"GameBorderColors"),
                    new Property("Graphics",type:PropertyType.Block,collectionType:"GameBorderGraphics"),
                }),


            new Block("GameBorderColors",
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("topColor",type:PropertyType.String),
                    new Property("bottomColor",type:PropertyType.String),
                    new Property("highlightColor",type:PropertyType.String),
                }),

            new Block("GameBorderGraphics",
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("offset",type:PropertyType.Integer),
                    new Property("topLeft",type:PropertyType.String),
                    new Property("top",type:PropertyType.String),
                    new Property("topRight",type:PropertyType.String),
                    new Property("left",type:PropertyType.String),
                    new Property("right",type:PropertyType.String),
                    new Property("bottomLeft",type:PropertyType.String),
                    new Property("bottom",type:PropertyType.String),
                    new Property("bottomRight",type:PropertyType.String),
                }),

            new Block("MenuColors",
                parsing:Parsing.OrderedProperties,
                properties:new[]
                {
                    new Property("border1",type:PropertyType.String),
                    new Property("border2",type:PropertyType.String),
                    new Property("border3",type:PropertyType.String),
                    new Property("background",type:PropertyType.String),
                    new Property("stripe",type:PropertyType.String),
                    new Property("stripeBg",type:PropertyType.String),
                }),

            new Block("MenuWindowColors",
                parsing:Parsing.OrderedProperties,
                properties:new[]
                {
                    new Property("background",type:PropertyType.String),
                    new Property("top",type:PropertyType.String),
                    new Property("bottom",type:PropertyType.String),
                    new Property("indexBackground",type:PropertyType.String),
                    new Property("indexTop",type:PropertyType.String),
                    new Property("indexBottom",type:PropertyType.String),
                }),

            new Block("MessageColors",
                parsing:Parsing.OrderedProperties,
                properties:new[]
                {
                    new Property("background",type:PropertyType.String),
                    new Property("top",type:PropertyType.String),
                    new Property("bottom",type:PropertyType.String),
                }),

            new Block("Psyched",
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("color1",type:PropertyType.String),
                    new Property("color2",type:PropertyType.String),
                    new Property("offset",type:PropertyType.Integer, defaultValue:0),
                }),

            // intermission
            new Block("intermission",
                isSubBlock:false,
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("name",type:PropertyType.String,isMetaData:true),
                    new Property(formatName:"intermissionaction",name:"IntermissionActions",singularName:"IntermissionAction",type:PropertyType.ImmutableList,collectionType:"IIntermissionAction"),
                }),

            new Block("IntermissionAction",
                isAbstract: true,
                parsing:Parsing.Manual,
                properties:new[]
                {
                    new Property("background",type:PropertyType.Block, collectionType:"IntermissionBackground"),
                    new Property("draw",type:PropertyType.Block, collectionType:"IntermissionDraw"),
                    new Property("music",type:PropertyType.String),
                    new Property("time",type:PropertyType.Integer),
                }),

            new Block("IntermissionBackground",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("texture",type:PropertyType.String),
                    new Property("tiled",type:PropertyType.Boolean),
                    new Property("palette",type:PropertyType.String),
                }),

            new Block("IntermissionDraw",
                parsing:Parsing.OrderedProperties,
                properties:new []
                {
                    new Property("texture",type:PropertyType.String),
                    new Property("x",type:PropertyType.Integer),
                    new Property("y",type:PropertyType.Integer),
                }),

            new Block("Fader",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:new[]
                {
                    new Property(formatName:"fadetype",name:"fadeType",type:PropertyType.String),
                }),

            new Block(formatName:"GotoTitle",className:"GoToTitle",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<Property>()),

            new Block("Image",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<Property>()),

            new Block("TextScreen",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:new[]
                {
                    new Property(formatName:"text",name:"Texts",singularName:"Text",type:PropertyType.ImmutableList, collectionType:"string"),
                    new Property(formatName:"textalignment",name:"textAlignment",type:PropertyType.String),
                    new Property(formatName:"textcolor",name:"textColor",type:PropertyType.String),
                    new Property(formatName:"textspeed",name:"textSpeed",type:PropertyType.Integer),
                    new Property("position",type:PropertyType.Block, collectionType:"TextScreenPosition"),
                }),

            new Block("TextScreenPosition",
                parsing:Parsing.OrderedProperties,
                properties:new []
                {
                    new Property("x",type:PropertyType.Integer),
                    new Property("y",type:PropertyType.Integer),
                }),

            new Block("VictoryStats",
                inheritsFrom:"IntermissionAction",
                implements:new [] {"IIntermissionAction"},
                properties:Enumerable.Empty<Property>()), 


            // map
            new Block("defaultmap",className:"DefaultMap",
                isSubBlock:false,
                inheritsFrom:"BaseMap",
                canSetPropertiesFrom:new [] {"AddDefaultMap"},
                properties:Enumerable.Empty<Property>()),
            new Block("adddefaultmap",className:"AddDefaultMap",
                isSubBlock:false,
                inheritsFrom:"BaseMap",
                properties:Enumerable.Empty<Property>()),
            new Block("BaseMap",
                isAbstract: true,
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property(formatName:"bordertexture",name:"borderTexture",type:PropertyType.String),
                    new Property("cluster",type:PropertyType.Integer),
                    new Property(formatName:"completionstring",name:"completionString",type:PropertyType.String),
                    new Property(formatName:"deathcam",name:"deathCam",type:PropertyType.Boolean, defaultValue:false),
                    new Property(formatName:"defaultceiling",name:"defaultCeiling",type:PropertyType.String),
                    new Property(formatName:"defaultfloor",name:"defaultFloor",type:PropertyType.String),
                    new Property(formatName:"ensureinventory",name:"EnsureInventories",singularName:"EnsureInventory",type:PropertyType.ImmutableList, collectionType:"string"),
                    new Property(formatName:"exitfade",name:"exitFade",type:PropertyType.Integer),
                    new Property(formatName:"floornumber",name:"floorNumber",type:PropertyType.Integer),
                    new Property(formatName:"highscoresgraphic",name:"highScoresGraphic",type:PropertyType.String),
                    new Property(formatName:"levelbonus",name:"levelBonus",type:PropertyType.Integer),
                    new Property(formatName:"levelnum",name:"levelNum",type:PropertyType.Integer),
                    new Property("music",type:PropertyType.String),
                    new Property(formatName:"spawnwithweaponraised",name:"spawnWithWeaponRaised",type:PropertyType.Flag, defaultValue:false),
                    new Property(formatName:"secretdeathsounds",name:"secretDeathSounds",type:PropertyType.Boolean, defaultValue:false),
                    new Property("next",type:PropertyType.Block,collectionType:"NextMapInfo"),
                    new Property(formatName:"secretnext",name:"secretNext",type:PropertyType.Block,collectionType:"NextMapInfo"),
                    new Property(formatName:"victorynext",name:"victoryNext",type:PropertyType.Block,collectionType:"NextMapInfo"),
                    new Property(formatName:"specialaction",name:"SpecialActions",singularName:"SpecialAction",type:PropertyType.ImmutableList, allowMultiple:true),
                    new Property(formatName:"nointermission",name:"nointermission",type:PropertyType.Flag, defaultValue:false),
                    new Property("par",type:PropertyType.Integer),
                    new Property("translator",type:PropertyType.String),
                }),
            new Block("map",
                isSubBlock:false,
                inheritsFrom:"BaseMap",
                propertyFallbacksFrom:new [] {"DefaultMap", "GameInfo"},
                properties:new []
                {
                    new Property("mapLump",type:PropertyType.String,isMetaData:true),
                    new Property("mapName",type:PropertyType.String,isMetaData:true),
                    new Property("mapNameLookup",type:PropertyType.String,isMetaData:true),
                }),

            new Block("NextMapInfo",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property("Name",PropertyType.String),
                    new Property("EndSequence",PropertyType.Boolean,defaultValue:false),
                }),

            new Block("specialaction",className: "SpecialAction",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property(formatName:"actorclass",name:"actorClass",type:PropertyType.String),
                    new Property("special",PropertyType.String),
                    new Property("arg0",PropertyType.Integer),
                    new Property("arg1",PropertyType.Integer),
                    new Property("arg2",PropertyType.Integer),
                    new Property("arg3",PropertyType.Integer),
                    new Property("arg4",PropertyType.Integer),
                }),

            new Block("mapInfo",
                parsing:Parsing.Manual,
                properties:new []
                {
                    new Property(formatName:"cluster",name:"Clusters",singularName:"Cluster",type:PropertyType.ImmutableList),
                    new Property(formatName:"episode",name:"Episodes",singularName:"Episode",type:PropertyType.ImmutableList),
                    new Property("gameInfo",formatName:"gameinfo",type:PropertyType.Block),
                    new Property(formatName:"intermission",name:"Intermissions",singularName:"Intermission",type:PropertyType.ImmutableList),
                    new Property(formatName:"map",name:"Maps",singularName:"Map",type:PropertyType.ImmutableList),
                }),
        };
    }
}
