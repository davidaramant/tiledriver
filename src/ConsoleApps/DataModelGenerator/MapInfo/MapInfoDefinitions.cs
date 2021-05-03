// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.Metadata
{
    static class MapInfoDefinitions
    {
        public static readonly ImmutableArray<IBlock> Blocks = new IBlock[]
        {
            // AUTOMAP

            new NormalBlock("autoMap",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty(formatName: "background", name: "Background"),
                    new StringProperty(formatName: "doorColor", name: "DoorColor"),
                    new StringProperty(formatName: "floorColor", name: "FloorColor"),
                    new StringProperty(formatName: "fontColor", name: "FontColor"),
                    new StringProperty(formatName: "wallColor", name: "WallColor"),
                    new StringProperty(formatName: "yourColor", name: "YourColor"),
                }.ToImmutableArray()),

            // CLUSTER

            new NormalBlock("cluster",
                Metadata: ImmutableArray<Property>.Empty.Add(new IntegerProperty("id")),
                Properties: new Property[]
                {
                    new BlockProperty("exitText", propertyType: "ClusterExitText"),
                    new FlagProperty("exitTextIsLump"),
                    new FlagProperty("exitTextIsMessage"),
                }.ToImmutableArray()),

            // EPISODE

            new NormalBlock("episode",
                Metadata: ImmutableArray.Create<Property>().Add(new StringProperty("map")),
                Properties: new Property[]
                {
                    new CharProperty("key"),
                    new StringProperty("lookup"),
                    new StringProperty("name"),
                    new FlagProperty("noSkillMenu"),
                    new FlagProperty("optional"),
                    new StringProperty("picName"),
                    new FlagProperty("remove"),
                }.ToImmutableArray()),

            // GAMEINFO

            new NormalBlock("gameInfo",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty("advisoryColor"),
                    new StringProperty("advisoryPic"),
                    new BlockProperty("border", propertyType: "IGameBorder"),
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
                    new StringProperty(formatName: "menufontcolor_highlightselection",
                        name: "menuFontColorHighlightSelection"),
                    new StringProperty(formatName: "menufontcolor_invalid", name: "menuFontColorInvalid"),
                    new StringProperty(formatName: "menufontcolor_invalidselection",
                        name: "menuFontColorInvalidSelection"),
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
                }.ToImmutableArray()),

            new NormalBlock("MenuColors",
                Metadata: ImmutableArray<Property>.Empty,
                Serialization: SerializationType.OrderedProperties,
                Properties: new Property[]
                {
                    new StringProperty("border1"),
                    new StringProperty("border2"),
                    new StringProperty("border3"),
                    new StringProperty("background"),
                    new StringProperty("stripe"),
                    new StringProperty("stripeBg"),
                }.ToImmutableArray()),

            new NormalBlock("MenuWindowColors",
                Metadata: ImmutableArray<Property>.Empty,
                Serialization: SerializationType.OrderedProperties,
                Properties: new Property[]
                {
                    new StringProperty("background"),
                    new StringProperty("top"),
                    new StringProperty("bottom"),
                    new StringProperty("indexBackground"),
                    new StringProperty("indexTop"),
                    new StringProperty("indexBottom"),
                }.ToImmutableArray()),

            new NormalBlock("MessageColors",
                Serialization: SerializationType.OrderedProperties,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty("background"),
                    new StringProperty("top"),
                    new StringProperty("bottom"),
                }.ToImmutableArray()),

            // INTERMISSION

            new NormalBlock("intermission",
                Serialization: SerializationType.Custom,
                Metadata: ImmutableArray<Property>.Empty.Add(new StringProperty("name")),
                Properties: new Property[]
                {
                    new ListProperty("intermissionActions", elementType: "IIntermissionAction"),
                }.ToImmutableArray()),

            new NormalBlock("IntermissionDraw",
                Serialization: SerializationType.OrderedProperties,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty("texture"),
                    new IntegerProperty("x"),
                    new IntegerProperty("y"),
                }.ToImmutableArray()),

            new AbstractBlock("BaseIntermissionAction",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new BlockProperty("background", propertyType: "IntermissionBackground"),
                    new BlockProperty("draw", propertyType: "IntermissionDraw"),
                    new StringProperty("music"),
                    new DoubleProperty("time"),
                }.ToImmutableArray()),

            new InheritedBlock("Fader",
                BaseClass: "BaseIntermissionAction",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new IdentifierProperty("fadeType"),
                }.ToImmutableArray()),

            new NormalBlock("GoToTitile",
                Serialization: SerializationType.Normal,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("Image",
                BaseClass: "BaseIntermissionAction",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("TextScreen",
                BaseClass: "BaseIntermissionAction",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("TextScreen",
                BaseClass: "BaseIntermissionAction",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new ListProperty("text", elementType: "string"),
                    new IdentifierProperty("textAlignment"),
                    new IdentifierProperty("textAnchor"),
                    new StringProperty("textColor"),
                    new DoubleProperty("textDelay"),
                    new IntegerProperty("textSpeed"),
                    new BlockProperty("position", propertyType: "TextScreenPosition"),
                }.ToImmutableArray()),

            new NormalBlock("TextScreenPosition",
                Serialization: SerializationType.OrderedProperties,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new IntegerProperty("x"),
                    new IntegerProperty("y"),
                }.ToImmutableArray()),

            new NormalBlock("VictoryStats",
                Serialization: SerializationType.Normal,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new BlockProperty("background", propertyType: "IntermissionBackground"),
                    new BlockProperty("draw", propertyType: "IntermissionDraw"),
                    new StringProperty("music"),
                    new DoubleProperty("time"),
                }.ToImmutableArray()),

            // MAP

            new AbstractBlock("BaseMap",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty("borderTexture"),
                    new IntegerProperty("cluster"),
                    new StringProperty("completionString"),
                    new BooleanProperty("deathCam", defaultValue: false),
                    new StringProperty("defaultCeiling"),
                    new StringProperty("defaultFloor"),
                    new ListProperty("ensureInventory", elementType: "string"),
                    new BlockProperty("exitFade", propertyType: "ExitFadeInfo"),
                    new IntegerProperty("floorNumber"),
                    new StringProperty("highScoresGraphic"),
                    new IntegerProperty("levelBonus"),
                    new IntegerProperty("levelNum"),
                    new StringProperty("music"),
                    new FlagProperty("spawnWithWeaponRaised"),
                    new BooleanProperty("secretDeathSounds", defaultValue: false),
                    new BlockProperty("next", propertyType: "NextMapInfo"),
                    new BlockProperty("secretNext", "NextMapInfo"),
                    new BlockProperty("victoryNext", propertyType: "NextMapInfo"),
                    new BlockProperty("SpecialActions", propertyType: "ImmutableList<SpecialAction>"),
                    new FlagProperty("noIntermission"),
                    new IntegerProperty("par"),
                    new StringProperty("translator"),
                }.ToImmutableArray()),

            new InheritedBlock("defaultMap",
                BaseClass: "BaseMap",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("addDefaultMap",
                BaseClass: "BaseMap",
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("map",
                BaseClass: "BaseMap",
                Metadata: new Property[]
                {
                    new StringProperty("mapLump"),
                    new StringProperty("mapName"),
                    new StringProperty("mapNameLookup"),
                }.ToImmutableArray(),
                Properties: ImmutableArray<Property>.Empty),


            new NormalBlock("specialAction",
                Serialization: SerializationType.Custom,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty(formatName: "actorclass", name: "actorClass"),
                    new StringProperty("special"),
                    new IntegerProperty("arg0"),
                    new IntegerProperty("arg1"),
                    new IntegerProperty("arg2"),
                    new IntegerProperty("arg3"),
                    new IntegerProperty("arg4"),
                }.ToImmutableArray()),

            // SKILL

            new NormalBlock("skill",
                Serialization: SerializationType.Normal,
                Metadata: ImmutableArray<Property>.Empty.Add(new IdentifierProperty("id")),
                Properties: new Property[]
                {
                    new DoubleProperty("damageFactor"),
                    new FlagProperty("fastMontsters"),
                    new IntegerProperty("lives"),
                    new IntegerProperty("mapFilter"),
                    new StringProperty("mustConfirm"),
                    new StringProperty("name"),
                    new StringProperty("picName"),
                    new DoubleProperty("playerDamageFactor"),
                    new BooleanProperty("quizHints"),
                    new DoubleProperty("scoreMultiplier"),
                    new IntegerProperty("spawnFilter"),
                }.ToImmutableArray()),

            // TOP-LEVEL

            //new NormalBlock("mapInfo",
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
        }.ToImmutableArray();
    }
}
