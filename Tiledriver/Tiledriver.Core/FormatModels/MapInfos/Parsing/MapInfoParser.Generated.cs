// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public static partial class MapInfoParser
    {
        private static Cluster ParseCluster(MapInfoBlock block)
        {
            var cluster =  Cluster.Default;
            cluster = ParseClusterMetadata(cluster, block );
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "exittext":
                        cluster = cluster.WithExitText( ParseClusterExitText(property, "Cluster exittext") );
                        break;
                    case "exittextislump":
                        cluster = cluster.WithExitTextIsLump( ParseFlag(property, "Cluster exittextislump") );
                        break;
                    case "exittextismessage":
                        cluster = cluster.WithExitTextIsMessage( ParseFlag(property, "Cluster exittextismessage") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Cluster.");
                }
            }
            return cluster;
        }

        private static Episode ParseEpisode(MapInfoBlock block)
        {
            var episode =  Episode.Default;
            episode = ParseEpisodeMetadata(episode, block );
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "key":
                        episode = episode.WithKey( ParseChar(property, "Episode key") );
                        break;
                    case "lookup":
                        episode = episode.WithLookup( ParseString(property, "Episode lookup") );
                        break;
                    case "name":
                        episode = episode.WithName( ParseString(property, "Episode name") );
                        break;
                    case "noskillmenu":
                        episode = episode.WithNoSkillMenu( ParseFlag(property, "Episode noskillmenu") );
                        break;
                    case "optional":
                        episode = episode.WithOptional( ParseFlag(property, "Episode optional") );
                        break;
                    case "picname":
                        episode = episode.WithPicName( ParseString(property, "Episode picname") );
                        break;
                    case "remove":
                        episode = episode.WithRemove( ParseFlag(property, "Episode remove") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Episode.");
                }
            }
            return episode;
        }

        private static GameInfo ParseGameInfo(MapInfoBlock block)
        {
            var gameInfo =  GameInfo.Default;
            block.AssertMetadataLength(0, "GameInfo");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "advisorycolor":
                        gameInfo = gameInfo.WithAdvisoryColor( ParseString(property, "GameInfo advisorycolor") );
                        break;
                    case "advisorypic":
                        gameInfo = gameInfo.WithAdvisoryPic( ParseString(property, "GameInfo advisorypic") );
                        break;
                    case "border":
                        gameInfo = gameInfo.WithBorder( ParseGameBorder(property, "GameInfo border") );
                        break;
                    case "borderflat":
                        gameInfo = gameInfo.WithBorderFlat( ParseString(property, "GameInfo borderflat") );
                        break;
                    case "deathtransition":
                        gameInfo = gameInfo.WithDeathTransition( ParseString(property, "GameInfo deathtransition") );
                        break;
                    case "dialogcolor":
                        gameInfo = gameInfo.WithDialogColor( ParseString(property, "GameInfo dialogcolor") );
                        break;
                    case "doorsoundsequence":
                        gameInfo = gameInfo.WithDoorSoundSequence( ParseString(property, "GameInfo doorsoundsequence") );
                        break;
                    case "drawreadthis":
                        gameInfo = gameInfo.WithDrawReadThis( ParseBoolean(property, "GameInfo drawreadthis") );
                        break;
                    case "finaleflat":
                        gameInfo = gameInfo.WithFinaleFlat( ParseString(property, "GameInfo finaleflat") );
                        break;
                    case "finalemusic":
                        gameInfo = gameInfo.WithFinaleMusic( ParseString(property, "GameInfo finalemusic") );
                        break;
                    case "gamecolormap":
                        gameInfo = gameInfo.WithGameColorMap( ParseString(property, "GameInfo gamecolormap") );
                        break;
                    case "gameoverpic":
                        gameInfo = gameInfo.WithGameOverPic( ParseString(property, "GameInfo gameoverpic") );
                        break;
                    case "gamepalette":
                        gameInfo = gameInfo.WithGamePalette( ParseString(property, "GameInfo gamepalette") );
                        break;
                    case "gibfactor":
                        gameInfo = gameInfo.WithGibFactor( ParseDouble(property, "GameInfo gibfactor") );
                        break;
                    case "highscoresfont":
                        gameInfo = gameInfo.WithHighScoresFont( ParseString(property, "GameInfo highscoresfont") );
                        break;
                    case "highscoresfontcolor":
                        gameInfo = gameInfo.WithHighScoresFontColor( ParseString(property, "GameInfo highscoresfontcolor") );
                        break;
                    case "intermissionmusic":
                        gameInfo = gameInfo.WithIntermissionMusic( ParseString(property, "GameInfo intermissionmusic") );
                        break;
                    case "menucolors":
                        gameInfo = gameInfo.WithMenuColors( ParseMenuColors(property, "GameInfo menucolors") );
                        break;
                    case "menufade":
                        gameInfo = gameInfo.WithMenuFade( ParseString(property, "GameInfo menufade") );
                        break;
                    case "menufontcolor_disabled":
                        gameInfo = gameInfo.WithMenuFontColorDisabled( ParseString(property, "GameInfo menufontcolor_disabled") );
                        break;
                    case "menufontcolor_highlight":
                        gameInfo = gameInfo.WithMenuFontColorHighlight( ParseString(property, "GameInfo menufontcolor_highlight") );
                        break;
                    case "menufontcolor_highlightselection":
                        gameInfo = gameInfo.WithMenuFontColorHighlightSelection( ParseString(property, "GameInfo menufontcolor_highlightselection") );
                        break;
                    case "menufontcolor_invalid":
                        gameInfo = gameInfo.WithMenuFontColorInvalid( ParseString(property, "GameInfo menufontcolor_invalid") );
                        break;
                    case "menufontcolor_invalidselection":
                        gameInfo = gameInfo.WithMenuFontColorInvalidSelection( ParseString(property, "GameInfo menufontcolor_invalidselection") );
                        break;
                    case "menufontcolor_label":
                        gameInfo = gameInfo.WithMenuFontColorLabel( ParseString(property, "GameInfo menufontcolor_label") );
                        break;
                    case "menufontcolor_selection":
                        gameInfo = gameInfo.WithMenuFontColorSelection( ParseString(property, "GameInfo menufontcolor_selection") );
                        break;
                    case "menufontcolor_title":
                        gameInfo = gameInfo.WithMenuFontColorTitle( ParseString(property, "GameInfo menufontcolor_title") );
                        break;
                    case "menumusic":
                        gameInfo = gameInfo.WithMenuMusic( ParseString(property, "GameInfo menumusic") );
                        break;
                    case "menuwindowcolors":
                        gameInfo = gameInfo.WithMenuWindowColors( ParseMenuWindowColors(property, "GameInfo menuwindowcolors") );
                        break;
                    case "messagecolors":
                        gameInfo = gameInfo.WithMessageColors( ParseMessageColors(property, "GameInfo messagecolors") );
                        break;
                    case "messagefontcolor":
                        gameInfo = gameInfo.WithMessageFontColor( ParseString(property, "GameInfo messagefontcolor") );
                        break;
                    case "pageindexfontcolor":
                        gameInfo = gameInfo.WithPageIndexFontColor( ParseString(property, "GameInfo pageindexfontcolor") );
                        break;
                    case "playerclasses":
                        gameInfo = gameInfo.WithPlayerClasses( ParseStringImmutableList(property, "GameInfo playerclasses") );
                        break;
                    case "psyched":
                        gameInfo = gameInfo.WithPsyched( ParsePsyched(property, "GameInfo psyched") );
                        break;
                    case "pushwallsoundsequence":
                        gameInfo = gameInfo.WithPushwallSoundSequence( ParseString(property, "GameInfo pushwallsoundsequence") );
                        break;
                    case "quitmessages":
                        gameInfo = gameInfo.WithQuitMessages( ParseStringImmutableList(property, "GameInfo quitmessages") );
                        break;
                    case "scoresmusic":
                        gameInfo = gameInfo.WithScoresMusic( ParseString(property, "GameInfo scoresmusic") );
                        break;
                    case "signon":
                        gameInfo = gameInfo.WithSignOn( ParseString(property, "GameInfo signon") );
                        break;
                    case "titlemusic":
                        gameInfo = gameInfo.WithTitleMusic( ParseString(property, "GameInfo titlemusic") );
                        break;
                    case "titlepage":
                        gameInfo = gameInfo.WithTitlePage( ParseString(property, "GameInfo titlepage") );
                        break;
                    case "titlepalette":
                        gameInfo = gameInfo.WithTitlePalette( ParseString(property, "GameInfo titlepalette") );
                        break;
                    case "titletime":
                        gameInfo = gameInfo.WithTitleTime( ParseInteger(property, "GameInfo titletime") );
                        break;
                    case "trackhighscores":
                        gameInfo = gameInfo.WithTrackHighScores( ParseBoolean(property, "GameInfo trackhighscores") );
                        break;
                    case "translator":
                        gameInfo = gameInfo.WithTranslator( ParseString(property, "GameInfo translator") );
                        break;
                    case "victorymusic":
                        gameInfo = gameInfo.WithVictoryMusic( ParseString(property, "GameInfo victorymusic") );
                        break;
                    case "victorypic":
                        gameInfo = gameInfo.WithVictoryPic( ParseString(property, "GameInfo victorypic") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in GameInfo.");
                }
            }
            return gameInfo;
        }

        private static Fader ParseFader(MapInfoBlock block)
        {
            var fader =  Fader.Default;
            block.AssertMetadataLength(0, "Fader");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "background":
                        fader = fader.WithBackground( ParseIntermissionBackground(property, "Fader background") );
                        break;
                    case "draw":
                        fader = fader.WithDraw( ParseIntermissionDraw(property, "Fader draw") );
                        break;
                    case "music":
                        fader = fader.WithMusic( ParseString(property, "Fader music") );
                        break;
                    case "time":
                        fader = fader.WithTime( ParseInteger(property, "Fader time") );
                        break;
                    case "fadetype":
                        fader = fader.WithFadeType( ParseString(property, "Fader fadetype") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Fader.");
                }
            }
            return fader;
        }

        private static GoToTitle ParseGoToTitle(MapInfoBlock block)
        {
            var goToTitle =  GoToTitle.Default;
            block.AssertMetadataLength(0, "GoToTitle");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in GoToTitle.");
                }
            }
            return goToTitle;
        }

        private static Image ParseImage(MapInfoBlock block)
        {
            var image =  Image.Default;
            block.AssertMetadataLength(0, "Image");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "background":
                        image = image.WithBackground( ParseIntermissionBackground(property, "Image background") );
                        break;
                    case "draw":
                        image = image.WithDraw( ParseIntermissionDraw(property, "Image draw") );
                        break;
                    case "music":
                        image = image.WithMusic( ParseString(property, "Image music") );
                        break;
                    case "time":
                        image = image.WithTime( ParseInteger(property, "Image time") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Image.");
                }
            }
            return image;
        }

        private static TextScreen ParseTextScreen(MapInfoBlock block)
        {
            var textScreen =  TextScreen.Default;
            block.AssertMetadataLength(0, "TextScreen");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "background":
                        textScreen = textScreen.WithBackground( ParseIntermissionBackground(property, "TextScreen background") );
                        break;
                    case "draw":
                        textScreen = textScreen.WithDraw( ParseIntermissionDraw(property, "TextScreen draw") );
                        break;
                    case "music":
                        textScreen = textScreen.WithMusic( ParseString(property, "TextScreen music") );
                        break;
                    case "time":
                        textScreen = textScreen.WithTime( ParseInteger(property, "TextScreen time") );
                        break;
                    case "text":
                        textScreen = textScreen.WithTexts( ParseStringImmutableList(property, "TextScreen text") );
                        break;
                    case "textalignment":
                        textScreen = textScreen.WithTextAlignment( ParseString(property, "TextScreen textalignment") );
                        break;
                    case "textcolor":
                        textScreen = textScreen.WithTextColor( ParseString(property, "TextScreen textcolor") );
                        break;
                    case "textspeed":
                        textScreen = textScreen.WithTextSpeed( ParseInteger(property, "TextScreen textspeed") );
                        break;
                    case "position":
                        textScreen = textScreen.WithPosition( ParseTextScreenPosition(property, "TextScreen position") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in TextScreen.");
                }
            }
            return textScreen;
        }

        private static VictoryStats ParseVictoryStats(MapInfoBlock block)
        {
            var victoryStats =  VictoryStats.Default;
            block.AssertMetadataLength(0, "VictoryStats");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "background":
                        victoryStats = victoryStats.WithBackground( ParseIntermissionBackground(property, "VictoryStats background") );
                        break;
                    case "draw":
                        victoryStats = victoryStats.WithDraw( ParseIntermissionDraw(property, "VictoryStats draw") );
                        break;
                    case "music":
                        victoryStats = victoryStats.WithMusic( ParseString(property, "VictoryStats music") );
                        break;
                    case "time":
                        victoryStats = victoryStats.WithTime( ParseInteger(property, "VictoryStats time") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in VictoryStats.");
                }
            }
            return victoryStats;
        }

        private static DefaultMap ParseDefaultMap(MapInfoBlock block)
        {
            var defaultMap =  DefaultMap.Default;
            block.AssertMetadataLength(0, "DefaultMap");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "bordertexture":
                        defaultMap = defaultMap.WithBorderTexture( ParseString(property, "DefaultMap bordertexture") );
                        break;
                    case "cluster":
                        defaultMap = defaultMap.WithCluster( ParseInteger(property, "DefaultMap cluster") );
                        break;
                    case "completionstring":
                        defaultMap = defaultMap.WithCompletionString( ParseString(property, "DefaultMap completionstring") );
                        break;
                    case "deathcam":
                        defaultMap = defaultMap.WithDeathCam( ParseBoolean(property, "DefaultMap deathcam") );
                        break;
                    case "defaultceiling":
                        defaultMap = defaultMap.WithDefaultCeiling( ParseString(property, "DefaultMap defaultceiling") );
                        break;
                    case "defaultfloor":
                        defaultMap = defaultMap.WithDefaultFloor( ParseString(property, "DefaultMap defaultfloor") );
                        break;
                    case "ensureinventory":
                        defaultMap = defaultMap.WithEnsureInventories( ParseStringImmutableList(property, "DefaultMap ensureinventory") );
                        break;
                    case "exitfade":
                        defaultMap = defaultMap.WithExitFade( ParseInteger(property, "DefaultMap exitfade") );
                        break;
                    case "floornumber":
                        defaultMap = defaultMap.WithFloorNumber( ParseInteger(property, "DefaultMap floornumber") );
                        break;
                    case "highscoresgraphic":
                        defaultMap = defaultMap.WithHighScoresGraphic( ParseString(property, "DefaultMap highscoresgraphic") );
                        break;
                    case "levelbonus":
                        defaultMap = defaultMap.WithLevelBonus( ParseInteger(property, "DefaultMap levelbonus") );
                        break;
                    case "levelnum":
                        defaultMap = defaultMap.WithLevelNum( ParseInteger(property, "DefaultMap levelnum") );
                        break;
                    case "music":
                        defaultMap = defaultMap.WithMusic( ParseString(property, "DefaultMap music") );
                        break;
                    case "spawnwithweaponraised":
                        defaultMap = defaultMap.WithSpawnWithWeaponRaised( ParseFlag(property, "DefaultMap spawnwithweaponraised") );
                        break;
                    case "secretdeathsounds":
                        defaultMap = defaultMap.WithSecretDeathSounds( ParseBoolean(property, "DefaultMap secretdeathsounds") );
                        break;
                    case "next":
                        defaultMap = defaultMap.WithNext( ParseNextMapInfo(property, "DefaultMap next") );
                        break;
                    case "secretnext":
                        defaultMap = defaultMap.WithSecretNext( ParseNextMapInfo(property, "DefaultMap secretnext") );
                        break;
                    case "victorynext":
                        defaultMap = defaultMap.WithVictoryNext( ParseNextMapInfo(property, "DefaultMap victorynext") );
                        break;
                    case "specialaction":
                        defaultMap = defaultMap.WithAdditionalSpecialAction( ParseSpecialAction(property, "DefaultMap specialaction") );
                        break;
                    case "nointermission":
                        defaultMap = defaultMap.WithNointermission( ParseFlag(property, "DefaultMap nointermission") );
                        break;
                    case "par":
                        defaultMap = defaultMap.WithPar( ParseInteger(property, "DefaultMap par") );
                        break;
                    case "translator":
                        defaultMap = defaultMap.WithTranslator( ParseString(property, "DefaultMap translator") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in DefaultMap.");
                }
            }
            return defaultMap;
        }

        private static AddDefaultMap ParseAddDefaultMap(MapInfoBlock block)
        {
            var addDefaultMap =  AddDefaultMap.Default;
            block.AssertMetadataLength(0, "AddDefaultMap");
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "bordertexture":
                        addDefaultMap = addDefaultMap.WithBorderTexture( ParseString(property, "AddDefaultMap bordertexture") );
                        break;
                    case "cluster":
                        addDefaultMap = addDefaultMap.WithCluster( ParseInteger(property, "AddDefaultMap cluster") );
                        break;
                    case "completionstring":
                        addDefaultMap = addDefaultMap.WithCompletionString( ParseString(property, "AddDefaultMap completionstring") );
                        break;
                    case "deathcam":
                        addDefaultMap = addDefaultMap.WithDeathCam( ParseBoolean(property, "AddDefaultMap deathcam") );
                        break;
                    case "defaultceiling":
                        addDefaultMap = addDefaultMap.WithDefaultCeiling( ParseString(property, "AddDefaultMap defaultceiling") );
                        break;
                    case "defaultfloor":
                        addDefaultMap = addDefaultMap.WithDefaultFloor( ParseString(property, "AddDefaultMap defaultfloor") );
                        break;
                    case "ensureinventory":
                        addDefaultMap = addDefaultMap.WithEnsureInventories( ParseStringImmutableList(property, "AddDefaultMap ensureinventory") );
                        break;
                    case "exitfade":
                        addDefaultMap = addDefaultMap.WithExitFade( ParseInteger(property, "AddDefaultMap exitfade") );
                        break;
                    case "floornumber":
                        addDefaultMap = addDefaultMap.WithFloorNumber( ParseInteger(property, "AddDefaultMap floornumber") );
                        break;
                    case "highscoresgraphic":
                        addDefaultMap = addDefaultMap.WithHighScoresGraphic( ParseString(property, "AddDefaultMap highscoresgraphic") );
                        break;
                    case "levelbonus":
                        addDefaultMap = addDefaultMap.WithLevelBonus( ParseInteger(property, "AddDefaultMap levelbonus") );
                        break;
                    case "levelnum":
                        addDefaultMap = addDefaultMap.WithLevelNum( ParseInteger(property, "AddDefaultMap levelnum") );
                        break;
                    case "music":
                        addDefaultMap = addDefaultMap.WithMusic( ParseString(property, "AddDefaultMap music") );
                        break;
                    case "spawnwithweaponraised":
                        addDefaultMap = addDefaultMap.WithSpawnWithWeaponRaised( ParseFlag(property, "AddDefaultMap spawnwithweaponraised") );
                        break;
                    case "secretdeathsounds":
                        addDefaultMap = addDefaultMap.WithSecretDeathSounds( ParseBoolean(property, "AddDefaultMap secretdeathsounds") );
                        break;
                    case "next":
                        addDefaultMap = addDefaultMap.WithNext( ParseNextMapInfo(property, "AddDefaultMap next") );
                        break;
                    case "secretnext":
                        addDefaultMap = addDefaultMap.WithSecretNext( ParseNextMapInfo(property, "AddDefaultMap secretnext") );
                        break;
                    case "victorynext":
                        addDefaultMap = addDefaultMap.WithVictoryNext( ParseNextMapInfo(property, "AddDefaultMap victorynext") );
                        break;
                    case "specialaction":
                        addDefaultMap = addDefaultMap.WithAdditionalSpecialAction( ParseSpecialAction(property, "AddDefaultMap specialaction") );
                        break;
                    case "nointermission":
                        addDefaultMap = addDefaultMap.WithNointermission( ParseFlag(property, "AddDefaultMap nointermission") );
                        break;
                    case "par":
                        addDefaultMap = addDefaultMap.WithPar( ParseInteger(property, "AddDefaultMap par") );
                        break;
                    case "translator":
                        addDefaultMap = addDefaultMap.WithTranslator( ParseString(property, "AddDefaultMap translator") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in AddDefaultMap.");
                }
            }
            return addDefaultMap;
        }

        private static Map ParseMap(MapInfoBlock block)
        {
            var map =  Map.Default;
            map = ParseMapMetadata(map, block );
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "bordertexture":
                        map = map.WithBorderTexture( ParseString(property, "Map bordertexture") );
                        break;
                    case "cluster":
                        map = map.WithCluster( ParseInteger(property, "Map cluster") );
                        break;
                    case "completionstring":
                        map = map.WithCompletionString( ParseString(property, "Map completionstring") );
                        break;
                    case "deathcam":
                        map = map.WithDeathCam( ParseBoolean(property, "Map deathcam") );
                        break;
                    case "defaultceiling":
                        map = map.WithDefaultCeiling( ParseString(property, "Map defaultceiling") );
                        break;
                    case "defaultfloor":
                        map = map.WithDefaultFloor( ParseString(property, "Map defaultfloor") );
                        break;
                    case "ensureinventory":
                        map = map.WithEnsureInventories( ParseStringImmutableList(property, "Map ensureinventory") );
                        break;
                    case "exitfade":
                        map = map.WithExitFade( ParseInteger(property, "Map exitfade") );
                        break;
                    case "floornumber":
                        map = map.WithFloorNumber( ParseInteger(property, "Map floornumber") );
                        break;
                    case "highscoresgraphic":
                        map = map.WithHighScoresGraphic( ParseString(property, "Map highscoresgraphic") );
                        break;
                    case "levelbonus":
                        map = map.WithLevelBonus( ParseInteger(property, "Map levelbonus") );
                        break;
                    case "levelnum":
                        map = map.WithLevelNum( ParseInteger(property, "Map levelnum") );
                        break;
                    case "music":
                        map = map.WithMusic( ParseString(property, "Map music") );
                        break;
                    case "spawnwithweaponraised":
                        map = map.WithSpawnWithWeaponRaised( ParseFlag(property, "Map spawnwithweaponraised") );
                        break;
                    case "secretdeathsounds":
                        map = map.WithSecretDeathSounds( ParseBoolean(property, "Map secretdeathsounds") );
                        break;
                    case "next":
                        map = map.WithNext( ParseNextMapInfo(property, "Map next") );
                        break;
                    case "secretnext":
                        map = map.WithSecretNext( ParseNextMapInfo(property, "Map secretnext") );
                        break;
                    case "victorynext":
                        map = map.WithVictoryNext( ParseNextMapInfo(property, "Map victorynext") );
                        break;
                    case "specialaction":
                        map = map.WithAdditionalSpecialAction( ParseSpecialAction(property, "Map specialaction") );
                        break;
                    case "nointermission":
                        map = map.WithNointermission( ParseFlag(property, "Map nointermission") );
                        break;
                    case "par":
                        map = map.WithPar( ParseInteger(property, "Map par") );
                        break;
                    case "translator":
                        map = map.WithTranslator( ParseString(property, "Map translator") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Map.");
                }
            }
            return map;
        }

        private static Skill ParseSkill(MapInfoBlock block)
        {
            var skill =  Skill.Default;
            skill = ParseSkillMetadata(skill, block );
            foreach(var property in block.Children)
            {
                switch (property.Name.ToLower())
                {
                    case "damagefactor":
                        skill = skill.WithDamageFactor( ParseDouble(property, "Skill damagefactor") );
                        break;
                    case "fastmonsters":
                        skill = skill.WithFastMontsters( ParseFlag(property, "Skill fastmonsters") );
                        break;
                    case "lives":
                        skill = skill.WithLives( ParseInteger(property, "Skill lives") );
                        break;
                    case "mapfilter":
                        skill = skill.WithMapFilter( ParseInteger(property, "Skill mapfilter") );
                        break;
                    case "mustconfirm":
                        skill = skill.WithMustConfirm( ParseString(property, "Skill mustconfirm") );
                        break;
                    case "name":
                        skill = skill.WithName( ParseString(property, "Skill name") );
                        break;
                    case "picname":
                        skill = skill.WithPicName( ParseString(property, "Skill picname") );
                        break;
                    case "playerdamagefactor":
                        skill = skill.WithPlayerDamageFactor( ParseDouble(property, "Skill playerdamagefactor") );
                        break;
                    case "quizhints":
                        skill = skill.WithQuizHints( ParseBoolean(property, "Skill quizhints") );
                        break;
                    case "scoremultiplier":
                        skill = skill.WithScoreMultiplier( ParseDouble(property, "Skill scoremultiplier") );
                        break;
                    case "spawnfilter":
                        skill = skill.WithSpawnFilter( ParseInteger(property, "Skill spawnfilter") );
                        break;
                    default:
                        throw new ParsingException($"Unknown element '{property.Name}' found in Skill.");
                }
            }
            return skill;
        }

        private static MenuColors ParseMenuColors(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(6, context);
            return new MenuColors(
                border1: ParseString(property.Values[0], context).ToMaybe(),
                border2: ParseString(property.Values[1], context).ToMaybe(),
                border3: ParseString(property.Values[2], context).ToMaybe(),
                background: ParseString(property.Values[3], context).ToMaybe(),
                stripe: ParseString(property.Values[4], context).ToMaybe(),
                stripeBg: ParseString(property.Values[5], context).ToMaybe());
        }

        private static MenuWindowColors ParseMenuWindowColors(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(6, context);
            return new MenuWindowColors(
                background: ParseString(property.Values[0], context).ToMaybe(),
                top: ParseString(property.Values[1], context).ToMaybe(),
                bottom: ParseString(property.Values[2], context).ToMaybe(),
                indexBackground: ParseString(property.Values[3], context).ToMaybe(),
                indexTop: ParseString(property.Values[4], context).ToMaybe(),
                indexBottom: ParseString(property.Values[5], context).ToMaybe());
        }

        private static MessageColors ParseMessageColors(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(3, context);
            return new MessageColors(
                background: ParseString(property.Values[0], context).ToMaybe(),
                top: ParseString(property.Values[1], context).ToMaybe(),
                bottom: ParseString(property.Values[2], context).ToMaybe());
        }

        private static IntermissionDraw ParseIntermissionDraw(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(3, context);
            return new IntermissionDraw(
                texture: ParseString(property.Values[0], context).ToMaybe(),
                x: ParseInteger(property.Values[1], context).ToMaybe(),
                y: ParseInteger(property.Values[2], context).ToMaybe());
        }

        private static TextScreenPosition ParseTextScreenPosition(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(2, context);
            return new TextScreenPosition(
                x: ParseInteger(property.Values[0], context).ToMaybe(),
                y: ParseInteger(property.Values[1], context).ToMaybe());
        }

    }
}
