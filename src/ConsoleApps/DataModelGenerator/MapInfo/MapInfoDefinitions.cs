// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using System.Linq;
using Tiledriver.DataModelGenerator.MapInfo.MetadataModel;
using Tiledriver.DataModelGenerator.MetadataModel;

namespace Tiledriver.DataModelGenerator.MapInfo
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
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseIntermissionAction"),
                metadata: ImmutableArray<Property>.Empty,
                properties: new Property[]
                {
                    new IdentifierProperty("fadeType"),
                }),

            new NormalBlock("GoToTitile",
                Serialization: SerializationType.Normal,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("Image",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseIntermissionAction"),
                metadata: ImmutableArray<Property>.Empty,
                properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("TextScreen",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseIntermissionAction"),
                metadata: ImmutableArray<Property>.Empty,
                properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("TextScreen",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseIntermissionAction"),
                metadata: ImmutableArray<Property>.Empty,
                properties: new Property[]
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
                    new StringProperty("borderTexture", isNullable:true),
                    new IntegerProperty("cluster", isNullable:true),
                    new StringProperty("completionString", isNullable:true),
                    new BooleanProperty("deathCam", isNullable:true),
                    new StringProperty("defaultCeiling", isNullable:true),
                    new StringProperty("defaultFloor", isNullable:true),
                    new ListProperty("ensureInventory", elementType: "string"),
                    new BlockProperty("exitFade", propertyType: "ExitFadeInfo", isNullable:true),
                    new IntegerProperty("floorNumber", isNullable:true),
                    new StringProperty("highScoresGraphic", isNullable:true),
                    new IntegerProperty("levelBonus", isNullable:true),
                    new IntegerProperty("levelNum", isNullable:true),
                    new StringProperty("music", isNullable:true),
                    new FlagProperty("spawnWithWeaponRaised", isNullable:true),
                    new BooleanProperty("secretDeathSounds", isNullable:true),
                    new BlockProperty("next", propertyType: "NextMapInfo", isNullable:true),
                    new BlockProperty("secretNext", "NextMapInfo", isNullable:true),
                    new BlockProperty("victoryNext", propertyType: "NextMapInfo", isNullable:true),
                    new ListProperty("SpecialActions", elementType: "SpecialAction"),
                    new FlagProperty("noIntermission",isNullable:true),
                    new IntegerProperty("par", isNullable:true),
                    new StringProperty("translator", isNullable:true),
                }.ToImmutableArray()),

            new InheritedBlock("defaultMap",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseMap"),
                metadata: ImmutableArray<Property>.Empty,
                properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("addDefaultMap",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseMap"),
                metadata: ImmutableArray<Property>.Empty,
                properties: ImmutableArray<Property>.Empty),

            new InheritedBlock("map",
                getBaseClass: ()=>Blocks.OfType<AbstractBlock>().Single(b=>b.ClassName == "BaseMap"),
                metadata: new Property[]
                {
                    new StringProperty("mapLump"),
                    new StringProperty("mapName", isNullable:true),
                    new BooleanProperty("isMapNameLookup", defaultValue:false),
                }.ToImmutableArray(),
                properties: ImmutableArray<Property>.Empty),


            new NormalBlock("specialAction",
                Serialization: SerializationType.Custom,
                Metadata: ImmutableArray<Property>.Empty,
                Properties: new Property[]
                {
                    new StringProperty(formatName: "actorclass", name: "actorClass"),
                    new StringProperty("special"),
                    new IntegerProperty("arg0", defaultValue:0),
                    new IntegerProperty("arg1", defaultValue:0),
                    new IntegerProperty("arg2", defaultValue:0),
                    new IntegerProperty("arg3", defaultValue:0),
                    new IntegerProperty("arg4", defaultValue:0),
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

        }.ToImmutableArray();
    }
}
