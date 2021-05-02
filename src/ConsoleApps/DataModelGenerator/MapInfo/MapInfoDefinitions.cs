// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.Metadata
{
    static class MapInfoDefinitions
    {
        public static IEnumerable<Block> Blocks()
        {
            yield return new Block(FormatName: "automap", ClassName: "AutoMap",
                Properties: new Property[]
                {
                    new StringProperty(formatName:"background",name:"Background"),
                    new StringProperty(formatName:"doorColor",name:"DoorColor"),
                    new StringProperty(formatName:"floorColor",name:"FloorColor"),
                    new StringProperty(formatName:"fontColor",name:"FontColor"),
                    new StringProperty(formatName:"wallColor",name:"WallColor"),
                    new StringProperty(formatName:"yourColor",name:"YourColor"),
                }.ToImmutableArray());

            yield return new Block("cluster",
                     Properties: new Property[]
                     {
                        new MetadataIntegerProperty("id"),
                        new BlockProperty("exitText", propertyType: "ClusterExitText"),
                        new FlagProperty("exitTextIsLump"),
                        new FlagProperty("exitTextIsMessage"),
                     }.ToImmutableArray());

            yield return new Block("ClusterExitText",
                    Serialization: SerializationType.Custom,
                    Properties: new Property[]
                    {
                        new StringProperty("text"),
                        new BooleanProperty("lookup"),
                    }.ToImmutableArray());

            yield return new Block("episode",
                    Properties: new Property[]
                    {
                        new MetadataStringProperty("map"),
                        new CharProperty("key"),
                        new StringProperty("lookup"),
                        new StringProperty("name"),
                        new FlagProperty("noSkillMenu"),
                        new FlagProperty("optional"),
                        new StringProperty("picName"),
                        new FlagProperty("remove"),
                    }.ToImmutableArray());

            // TODO: clearepisodes

            yield return new Block("gameInfo",
                    Properties: new Property[]
                    {
                        new StringProperty("advisoryColor"),
                        new StringProperty("advisoryPic"),
                        new BlockProperty("border", propertyType:"IGameBorder"),
                        new StringProperty("borderFlat"),
                        new StringProperty("deathTransition"),
                        new StringProperty("dialogColor"),
                        new StringProperty("doorSoundSequence"),
                        new BooleanProperty("drawReadThis"),
                        new StringProperty("finaleFlat"),
                        new StringProperty("finaleMusic"),
                        new StringProperty("gameColorMap"),
                        new StringProperty("gameOverPic"),
                        new StringProperty("gamePalette"),
                        new DoubleProperty("gibFactor"),
                        new StringProperty("highScoresFont"),
                        new StringProperty("highScoresFontColor"),
                        new StringProperty("intermissionMusic"),
                        new BlockProperty("menuColors"),
                        new StringProperty("menuFade"),
                        new StringProperty(formatName: "menufontcolor_disabled", name: "menuFontColorDisabled"),
                        new StringProperty(formatName: "menufontcolor_highlight", name: "menuFontColorHighlight"),
                        new StringProperty(formatName: "menufontcolor_highlightselection", name: "menuFontColorHighlightSelection"),
                        new StringProperty(formatName: "menufontcolor_invalid", name: "menuFontColorInvalid"),
                        new StringProperty(formatName: "menufontcolor_invalidselection", name: "menuFontColorInvalidSelection"),
                        new StringProperty(formatName: "menufontcolor_label", name: "menuFontColorLabel"),
                        new StringProperty(formatName: "menufontcolor_selection", name: "menuFontColorSelection"),
                        new StringProperty(formatName: "menufontcolor_title", name: "menuFontColorTitle"),
                        new StringProperty(formatName: "menumusic", name: "menuMusic"),
                        new BlockProperty("menuWindowColors"),
                        new BlockProperty("messageColors"),
                        new StringProperty("messageFontColor"),
                        new StringProperty("pageIndexFontColor"),
                        new ListProperty("playerClasses", elementType: "string"),
                        new BlockProperty("psyched"),
                        new StringProperty("pushwallSoundSequence"),
                        new ListProperty("quitMessages", elementType: "string"),
                        new StringProperty("scoresMusic"),
                        new StringProperty("signOn"),
                        new StringProperty("titleMusic"),
                        new StringProperty("titlePage"),
                        new StringProperty("titlePalette"),
                        new IntegerProperty("titleTime"),
                        new BooleanProperty("trackHighScores"),
                        new StringProperty("translator"),
                        new StringProperty("victoryMusic"),
                        new StringProperty("victoryPic"),
                    }.ToImmutableArray());

            yield return new Block("GameBorderColors",
                    Serialization: SerializationType.Custom,
                    Properties: new Property[]
                    {
                        new StringProperty("topColor"),
                        new StringProperty("bottomColor"),
                        new StringProperty("highlightColor"),
                    }.ToImmutableArray());

            yield return new Block("GameBorderGraphics",
                    Serialization: SerializationType.Custom,
                    Properties: new Property[]
                    {
                        new IntegerProperty("offset"),
                        new StringProperty("topLeft"),
                        new StringProperty("top"),
                        new StringProperty("topRight"),
                        new StringProperty("left"),
                        new StringProperty("right"),
                        new StringProperty("bottomLeft"),
                        new StringProperty("bottom"),
                        new StringProperty("bottomRight"),
                    }.ToImmutableArray());

            yield return new Block("MenuColors",
                    Serialization: SerializationType.OrderedProperties,
                    Properties: new Property[]
                    {
                        new StringProperty("border1"),
                        new StringProperty("border2"),
                        new StringProperty("border3"),
                        new StringProperty("background"),
                        new StringProperty("stripe"),
                        new StringProperty("stripeBg"),
                    }.ToImmutableArray());

            yield return new Block("MenuWindowColors",
                    Serialization: SerializationType.OrderedProperties,
                    Properties: new Property[]
                    {
                        new StringProperty("background"),
                        new StringProperty("top"),
                        new StringProperty("bottom"),
                        new StringProperty("indexBackground"),
                        new StringProperty("indexTop"),
                        new StringProperty("indexBottom"),
                    }.ToImmutableArray());

            yield return new Block("MessageColors",
                    Serialization: SerializationType.OrderedProperties,
                    Properties: new Property[]
                    {
                        new StringProperty("background"),
                        new StringProperty("top"),
                        new StringProperty("bottom"),
                    }.ToImmutableArray());

            yield return new Block("Psyched",
                    Serialization: SerializationType.Custom,
                    Properties: new Property[]
                    {
                        new StringProperty("color1"),
                        new StringProperty("color2"),
                        new IntegerProperty("offset", defaultValue: 0),
                    }.ToImmutableArray());

            //yield return new Block("intermission",
            //        IsSubBlock: false,
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new Property("name", type: PropertyType.String, isMetaData: true),
            //            new Property(formatName: "intermissionaction", name: "IntermissionActions",
            //                singularName: "IntermissionAction", type: PropertyType.ImmutableList,
            //                collectionType: "IIntermissionAction"),
            //        });

            //yield return new Block("IntermissionAction",
            //        isAbstract: true,
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new Property("background", type: PropertyType.Block, collectionType: "IntermissionBackground"),
            //            new Property("draw", type: PropertyType.Block, collectionType: "IntermissionDraw"),
            //            new StringProperty("music"),
            //            new Property("time", type: PropertyType.Block, collectionType:"IntermissionTime"),
            //        });

            //yield return new Block("IntermissionTime",
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new DoubleProperty("time"),
            //            new Property("TitleTime",type:PropertyType.Boolean),
            //        });

            //yield return new Block("IntermissionBackground",
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new StringProperty("texture"),
            //            new BooleanProperty("tiled"),
            //            new StringProperty("palette"),
            //        });

            //yield return new Block("IntermissionDraw",
            //        parsing: Parsing.OrderedProperties,
            //        Properties: new[]
            //        {
            //            new StringProperty("texture"),
            //            new IntegerProperty("x"),
            //            new IntegerProperty("y"),
            //        });

            //yield return new Block("Fader",
            //        inheritsFrom: "IntermissionAction",
            //        implements: new[] { "IIntermissionAction" },
            //        Properties: new[]
            //        {
            //            new Property(formatName: "fadetype", name: "fadeType", type: PropertyType.Identifier),
            //        });

            //yield return new Block(formatName: "GotoTitle", className: "GoToTitle",
            //        implements: new[] { "IIntermissionAction" },
            //        Properties: Enumerable.Empty<Property>());

            //yield return new Block("Image",
            //        inheritsFrom: "IntermissionAction",
            //        implements: new[] { "IIntermissionAction" },
            //        Properties: Enumerable.Empty<Property>());

            //yield return new Block("TextScreen",
            //        inheritsFrom: "IntermissionAction",
            //        implements: new[] { "IIntermissionAction" },
            //        Properties: new[]
            //        {
            //            new Property(formatName: "text", name: "Texts", singularName: "Text",
            //                type: PropertyType.ImmutableList, collectionType: "string"),
            //            new Property(formatName: "textalignment", name: "textAlignment", type: PropertyType.Identifier),
            //            new Property(formatName: "textanchor", name: "textAnchor", type: PropertyType.Identifier),
            //            new StringProperty(formatName: "textcolor", name: "textColor"),
            //            new DoubleProperty(formatName: "textdelay", name: "textDelay"),
            //            new IntegerProperty(formatName: "textspeed", name: "textSpeed"),
            //            new Property("position", type: PropertyType.Block, collectionType: "TextScreenPosition"),
            //        });

            //yield return new Block("TextScreenPosition",
            //        parsing: Parsing.OrderedProperties,
            //        Properties: new[]
            //        {
            //            new IntegerProperty("x"),
            //            new IntegerProperty("y"),
            //        });

            //yield return new Block("VictoryStats",
            //        inheritsFrom: "IntermissionAction",
            //        implements: new[] { "IIntermissionAction" },
            //        Properties: Enumerable.Empty<Property>());

            //yield return new Block("defaultmap", className: "DefaultMap",
            //        IsSubBlock: false,
            //        inheritsFrom: "BaseMap",
            //        canSetPropertiesFrom: new[] { "AddDefaultMap" },
            //        Properties: Enumerable.Empty<Property>());

            //yield return new Block("adddefaultmap", className: "AddDefaultMap",
            //        IsSubBlock: false,
            //        inheritsFrom: "BaseMap",
            //        Properties: Enumerable.Empty<Property>());

            //yield return new Block("BaseMap",
            //        isAbstract: true,
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new StringProperty(formatName: "bordertexture", name: "borderTexture"),
            //            new IntegerProperty("cluster"),
            //            new StringProperty(formatName: "completionstring", name: "completionString"),
            //            new Property(formatName: "deathcam", name: "deathCam", type: PropertyType.Boolean,
            //                defaultValue: false),
            //            new StringProperty(formatName: "defaultceiling", name: "defaultCeiling"),
            //            new StringProperty(formatName: "defaultfloor", name: "defaultFloor"),
            //            new Property(formatName: "ensureinventory", name: "EnsureInventories",
            //                singularName: "EnsureInventory", type: PropertyType.ImmutableList, collectionType: "string"),
            //            new Property(formatName: "exitfade", name: "exitFade", type: PropertyType.Block, collectionType:"ExitFadeInfo"),
            //            new IntegerProperty(formatName: "floornumber", name: "floorNumber"),
            //            new StringProperty(formatName: "highscoresgraphic", name: "highScoresGraphic"),
            //            new IntegerProperty(formatName: "levelbonus", name: "levelBonus"),
            //            new IntegerProperty(formatName: "levelnum", name: "levelNum"),
            //            new StringProperty("music"),
            //            new Property(formatName: "spawnwithweaponraised", name: "spawnWithWeaponRaised",
            //                type: PropertyType.Flag, defaultValue: false),
            //            new Property(formatName: "secretdeathsounds", name: "secretDeathSounds",
            //                type: PropertyType.Boolean, defaultValue: false),
            //            new Property("next", type: PropertyType.Block, collectionType: "NextMapInfo"),
            //            new Property(formatName: "secretnext", name: "secretNext", type: PropertyType.Block,
            //                collectionType: "NextMapInfo"),
            //            new Property(formatName: "victorynext", name: "victoryNext", type: PropertyType.Block,
            //                collectionType: "NextMapInfo"),
            //            new Property(formatName: "specialaction", name: "SpecialActions", singularName: "SpecialAction",
            //                type: PropertyType.ImmutableList, allowMultiple: true),
            //            new Property(formatName: "nointermission", name: "nointermission", type: PropertyType.Flag,
            //                defaultValue: false),
            //            new IntegerProperty("par"),
            //            new StringProperty("translator"),
            //        });

            //yield return new Block("map",
            //        IsSubBlock: false,
            //        inheritsFrom: "BaseMap",
            //        propertyFallbacksFrom: new[] { "DefaultMap", "GameInfo" },
            //        Properties: new[]
            //        {
            //            new Property("mapLump", type: PropertyType.String, isMetaData: true),
            //            new Property("mapName", type: PropertyType.String, isMetaData: true),
            //            new Property("mapNameLookup", type: PropertyType.String, isMetaData: true),
            //        });

            //yield return new Block("NextMapInfo",
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new Property("Name", PropertyType.String),
            //            new Property("EndSequence", PropertyType.Boolean, defaultValue: false),
            //        });

            //yield return new Block("ExitFadeInfo",
            //    parsing: Parsing.Manual,
            //    Properties: new[]
            //    {
            //        new Property("Color", PropertyType.String),
            //        new Property("Time", PropertyType.Double),
            //    });

            //yield return new Block("specialaction", className: "SpecialAction",
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new StringProperty(formatName: "actorclass", name: "actorClass"),
            //            new Property("special", PropertyType.String),
            //            new Property("arg0", PropertyType.Integer),
            //            new Property("arg1", PropertyType.Integer),
            //            new Property("arg2", PropertyType.Integer),
            //            new Property("arg3", PropertyType.Integer),
            //            new Property("arg4", PropertyType.Integer),
            //        });

            //yield return new Block("skill",
            //    Properties: new[]
            //    {
            //        new Property("id", type:PropertyType.Identifier, isMetaData:true),
            //        new Property(formatName:"damagefactor",name:"DamageFactor", type:PropertyType.Double),
            //        new Property(formatName:"fastmonsters",name:"FastMontsters", type:PropertyType.Flag),
            //        new Property(formatName:"lives",name:"Lives", type:PropertyType.Integer),
            //        new Property(formatName:"mapfilter",name:"MapFilter", type:PropertyType.Integer),
            //        new Property(formatName:"mustconfirm",name:"MustConfirm", type:PropertyType.String),
            //        new Property(formatName:"name",name:"Name", type:PropertyType.String),
            //        new Property(formatName:"picname",name:"PicName", type:PropertyType.String),
            //        new Property(formatName:"playerdamagefactor",name:"PlayerDamageFactor", type:PropertyType.Double),
            //        new Property(formatName:"quizhints",name:"QuizHints", type:PropertyType.Boolean),
            //        new Property(formatName:"scoremultiplier",name:"ScoreMultiplier", type:PropertyType.Double),
            //        new Property(formatName:"spawnfilter",name:"SpawnFilter", type:PropertyType.Integer),
            //    });

            //yield return new Block("mapInfo",
            //        parsing: Parsing.Manual,
            //        Properties: new[]
            //        {
            //            new Property("autoMap", formatName: "automap", type: PropertyType.Block),
            //            new Property(formatName: "cluster", name: "Clusters", singularName: "Cluster",type: PropertyType.ImmutableList),
            //            new Property(formatName: "episode", name: "Episodes", singularName: "Episode",type: PropertyType.ImmutableList),
            //            new Property("gameInfo", formatName: "gameinfo", type: PropertyType.Block),
            //            new Property(formatName: "intermission", name: "Intermissions", singularName: "Intermission",type: PropertyType.ImmutableList),
            //            new Property(formatName: "map", name: "Maps", singularName: "Map",type: PropertyType.ImmutableList),
            //            new Property(formatName: "skill", name: "Skills", singularName: "Skill",type: PropertyType.ImmutableList),
            //        });
        }
    }
}
