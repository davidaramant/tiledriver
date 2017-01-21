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
            cluster = ParseClusterMetadata(cluster, block.Metadata);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "exittext":
                        break;
                    case "exittextislump":
                        break;
                    case "exittextismessage":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Cluster.");
                }
            }
            return cluster;
        }

        private static Episode ParseEpisode(MapInfoBlock block)
        {
            var episode =  Episode.Default;
            episode = ParseEpisodeMetadata(episode, block.Metadata);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "key":
                        break;
                    case "lookup":
                        break;
                    case "name":
                        break;
                    case "noskillmenu":
                        break;
                    case "optional":
                        break;
                    case "picname":
                        break;
                    case "remove":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Episode.");
                }
            }
            return episode;
        }

        private static GameInfo ParseGameInfo(MapInfoBlock block)
        {
            var gameInfo =  GameInfo.Default;
            AssertMetadataLength("GameInfo",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "advisorycolor":
                        break;
                    case "advisorypic":
                        break;
                    case "border":
                        break;
                    case "borderflat":
                        break;
                    case "doorsoundsequence":
                        break;
                    case "drawreadthis":
                        break;
                    case "finalemusic":
                        break;
                    case "gamepalette":
                        break;
                    case "gibfactor":
                        break;
                    case "highscoresfont":
                        break;
                    case "highscoresfontcolor":
                        break;
                    case "intermissionmusic":
                        break;
                    case "menucolor":
                        break;
                    case "menufade":
                        break;
                    case "menufontcolor_disabled":
                        break;
                    case "menufontcolor_highlight":
                        break;
                    case "menufontcolor_highlightselection":
                        break;
                    case "menufontcolor_invalid":
                        break;
                    case "menufontcolor_invalidselection":
                        break;
                    case "menufontcolor_label":
                        break;
                    case "menufontcolor_selection":
                        break;
                    case "menufontcolor_title":
                        break;
                    case "menumusic":
                        break;
                    case "pageindexfontcolor":
                        break;
                    case "playerclasses":
                        break;
                    case "pushwallsoundsequence":
                        break;
                    case "quitmessages":
                        break;
                    case "scoresmusic":
                        break;
                    case "signon":
                        break;
                    case "titlemusic":
                        break;
                    case "titletime":
                        break;
                    case "translator":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in GameInfo.");
                }
            }
            return gameInfo;
        }

        private static Intermission ParseIntermission(MapInfoBlock block)
        {
            var intermission =  Intermission.Default;
            intermission = ParseIntermissionMetadata(intermission, block.Metadata);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "intermissionaction":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Intermission.");
                }
            }
            return intermission;
        }

        private static Fader ParseFader(MapInfoBlock block)
        {
            var fader =  Fader.Default;
            AssertMetadataLength("Fader",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "background":
                        break;
                    case "draw":
                        break;
                    case "music":
                        break;
                    case "time":
                        break;
                    case "fadetype":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Fader.");
                }
            }
            return fader;
        }

        private static GoToTitle ParseGoToTitle(MapInfoBlock block)
        {
            var goToTitle =  GoToTitle.Default;
            AssertMetadataLength("GoToTitle",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in GoToTitle.");
                }
            }
            return goToTitle;
        }

        private static Image ParseImage(MapInfoBlock block)
        {
            var image =  Image.Default;
            AssertMetadataLength("Image",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "background":
                        break;
                    case "draw":
                        break;
                    case "music":
                        break;
                    case "time":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Image.");
                }
            }
            return image;
        }

        private static TextScreen ParseTextScreen(MapInfoBlock block)
        {
            var textScreen =  TextScreen.Default;
            AssertMetadataLength("TextScreen",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "background":
                        break;
                    case "draw":
                        break;
                    case "music":
                        break;
                    case "time":
                        break;
                    case "text":
                        break;
                    case "textalignment":
                        break;
                    case "textcolor":
                        break;
                    case "textspeed":
                        break;
                    case "position":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in TextScreen.");
                }
            }
            return textScreen;
        }

        private static VictoryStats ParseVictoryStats(MapInfoBlock block)
        {
            var victoryStats =  VictoryStats.Default;
            AssertMetadataLength("VictoryStats",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "background":
                        break;
                    case "draw":
                        break;
                    case "music":
                        break;
                    case "time":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in VictoryStats.");
                }
            }
            return victoryStats;
        }

        private static DefaultMap ParseDefaultMap(MapInfoBlock block)
        {
            var defaultMap =  DefaultMap.Default;
            AssertMetadataLength("DefaultMap",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "bordertexture":
                        break;
                    case "cluster":
                        break;
                    case "completionstring":
                        break;
                    case "deathcam":
                        break;
                    case "defaultceiling":
                        break;
                    case "defaultfloor":
                        break;
                    case "ensureinventory":
                        break;
                    case "exitfade":
                        break;
                    case "floornumber":
                        break;
                    case "highscoresgraphic":
                        break;
                    case "levelbonus":
                        break;
                    case "levelnum":
                        break;
                    case "music":
                        break;
                    case "spawnwithweaponraised":
                        break;
                    case "secretdeathsounds":
                        break;
                    case "next":
                        break;
                    case "secretnext":
                        break;
                    case "victorynext":
                        break;
                    case "specialaction":
                        break;
                    case "nointermission":
                        break;
                    case "par":
                        break;
                    case "translator":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in DefaultMap.");
                }
            }
            return defaultMap;
        }

        private static AddDefaultMap ParseAddDefaultMap(MapInfoBlock block)
        {
            var addDefaultMap =  AddDefaultMap.Default;
            AssertMetadataLength("AddDefaultMap",block.Metadata,0);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "bordertexture":
                        break;
                    case "cluster":
                        break;
                    case "completionstring":
                        break;
                    case "deathcam":
                        break;
                    case "defaultceiling":
                        break;
                    case "defaultfloor":
                        break;
                    case "ensureinventory":
                        break;
                    case "exitfade":
                        break;
                    case "floornumber":
                        break;
                    case "highscoresgraphic":
                        break;
                    case "levelbonus":
                        break;
                    case "levelnum":
                        break;
                    case "music":
                        break;
                    case "spawnwithweaponraised":
                        break;
                    case "secretdeathsounds":
                        break;
                    case "next":
                        break;
                    case "secretnext":
                        break;
                    case "victorynext":
                        break;
                    case "specialaction":
                        break;
                    case "nointermission":
                        break;
                    case "par":
                        break;
                    case "translator":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in AddDefaultMap.");
                }
            }
            return addDefaultMap;
        }

        private static Map ParseMap(MapInfoBlock block)
        {
            var map =  Map.Default;
            map = ParseMapMetadata(map, block.Metadata);
            foreach(var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    case "bordertexture":
                        break;
                    case "cluster":
                        break;
                    case "completionstring":
                        break;
                    case "deathcam":
                        break;
                    case "defaultceiling":
                        break;
                    case "defaultfloor":
                        break;
                    case "ensureinventory":
                        break;
                    case "exitfade":
                        break;
                    case "floornumber":
                        break;
                    case "highscoresgraphic":
                        break;
                    case "levelbonus":
                        break;
                    case "levelnum":
                        break;
                    case "music":
                        break;
                    case "spawnwithweaponraised":
                        break;
                    case "secretdeathsounds":
                        break;
                    case "next":
                        break;
                    case "secretnext":
                        break;
                    case "victorynext":
                        break;
                    case "specialaction":
                        break;
                    case "nointermission":
                        break;
                    case "par":
                        break;
                    case "translator":
                        break;
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Map.");
                }
            }
            return map;
        }

    }
}
