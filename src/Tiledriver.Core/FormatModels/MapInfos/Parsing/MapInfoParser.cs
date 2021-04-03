// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.Extensions.Strings;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public static partial class MapInfoParser
    {
        public static MapInfo Parse(IEnumerable<IMapInfoElement> elements)
        {
            var autoMap = AutoMap.Default;
            var clusters = new List<Cluster>();
            var episodes = new List<Episode>();
            var gameInfo = GameInfo.Default;
            var intermissions = new List<Intermission>();
            var defaultMap = DefaultMap.Default;
            var maps = new List<Map>();
            var skills = new List<Skill>();

            foreach (var element in elements)
            {
                switch (element.Name.ToLower())
                {
                    case "automap":
                        var newAutoMap = ParseAutoMap(element.AssertAsBlock("AutoMap"));
                        autoMap = autoMap.WithAutoMap(newAutoMap);
                        break;

                    case "cluster":
                        clusters.Add(ParseCluster(element.AssertAsBlock("Cluster")));
                        break;

                    case "clearepisodes":
                        var flag = element.AssertAsProperty("Clear Episodes");
                        flag.AssertValuesLength(0, "Clear Episodes");
                        episodes.Clear();
                        break;

                    case "episode":
                        episodes.Add(ParseEpisode(element.AssertAsBlock("Episode")));
                        break;

                    case "gameinfo":
                        var newGameInfo = ParseGameInfo(element.AssertAsBlock("GameInfo"));
                        gameInfo = gameInfo.WithGameInfo(newGameInfo);
                        break;

                    case "intermission":
                        intermissions.Add(ParseIntermission(element.AssertAsBlock("Intermission")));
                        break;

                    case "defaultmap":
                        defaultMap = ParseDefaultMap(element.AssertAsBlock("DefaultMap"));
                        break;

                    case "adddefaultmap":
                        var addDefaultMap = ParseAddDefaultMap(element.AssertAsBlock("AddDefaultMap"));
                        defaultMap = defaultMap.WithAddDefaultMap(addDefaultMap);
                        break;

                    case "map":
                        var map = ParseMap(element.AssertAsBlock("Map"));
                        maps.Add(map.WithFallbackDefaultMap(defaultMap).WithFallbackGameInfo(gameInfo));
                        break;

                    case "skill":
                        skills.Add(ParseSkill(element.AssertAsBlock("Skill")));
                        break;

                    default:
                        throw new ParsingException($"Unknown element '{element.Name}' in global MapInfo scope.");
                }
            }

            return new MapInfo(autoMap.ToMaybe(), clusters, episodes, gameInfo.ToMaybe(), intermissions, maps, skills);
        }

        private static Cluster ParseClusterMetadata(Cluster cluster, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Cluster");

            var id = ParseInteger(block.Metadata[0], "Cluster id");

            return cluster.WithId(id);
        }

        private static Episode ParseEpisodeMetadata(Episode episode, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Episode");

            var map = ParseQuotedStringWithMinLength(block.Metadata[0], 1, "Episode map");

            return episode.WithMap(map);
        }

        private static Map ParseMapMetadata(Map map, MapInfoBlock block)
        {
            var metadata = block.Metadata;

            switch (metadata.Length)
            {
                case 1:
                    return ParseMapLumpMetadata(map, metadata[0]);
                case 2:
                    map = ParseMapLumpMetadata(map, metadata[0]);

                    var mapName = ParseQuotedStringWithMinLength(metadata[1], 1, "Map name");

                    return map.WithMapName(mapName);
                case 3:
                    map = ParseMapLumpMetadata(map, metadata[0]);

                    if (metadata[1] != "lookup")
                    {
                        throw new ParsingException($"Expected 'lookup' after Map name but got: " + metadata[1]);
                    }

                    var mapNameLookup = ParseQuotedStringWithMinLength(metadata[2], 1, "Map name lookup");

                    return map.WithMapNameLookup(mapNameLookup);
                default:
                    throw new ParsingException($"Unexpected metadata length in Map: " + metadata.Length);
            }
        }

        private static Skill ParseSkillMetadata(Skill skill, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Skill");

            return skill.WithId(block.Metadata[0]);
        }

        private static Map ParseMapLumpMetadata(Map map, string metadata)
        {
            var mapLump = ParseQuotedStringWithMinLength(metadata, 1, "Map lump name");
            return map.WithMapLump(mapLump);
        }

        private static string ParseString(string s, string context)
        {
            return ParseQuotedStringWithMinLength(s, 0, context);
        }

        private static string ParseQuotedStringWithMinLength(string s, int minLength, string context)
        {
            if (s.Length > 2 && s[0] == '"' && s.Last() == '"')
            {
                var str = s.Substring(1).RemoveLast(1);
                if (str.Length >= minLength)
                {
                    return str;
                }
            }
            throw new ParsingException($"{context} was not a properly formatted string: " + s);
        }

        private static int ParseInteger(string s, string context)
        {
            if (!int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
            {
                throw new ParsingException($"{context} was not an integer: " + s);
            }
            return result;
        }

        private static int ParseInteger(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            return ParseInteger(property.Values[0], context);
        }

        private static double ParseDouble(string s, string context)
        {
            if (!double.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
            {
                throw new ParsingException($"{context} was not an integer: " + s);
            }
            return result;
        }

        private static double ParseDouble(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            double result;
            if (!double.TryParse(property.Values[0], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                throw new ParsingException($"{context} was not a floating point number: " + property.Values[0]);
            }
            return result;
        }

        private static string ParseIdentifier(string s, string context)
        {
            // TODO: Should this be a real Identifier type?
            return s;
        }

        private static string ParseIdentifier(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            return property.Values[0];
        }

        private static string ParseString(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            var s = property.Values[0];

            if (s.Length > 2 && s[0] == '"' && s.Last() == '"')
            {
                return s.Substring(1).RemoveLast(1);
            }
            throw new ParsingException($"{context} was not a properly formatted string: " + s);
        }

        private static char ParseChar(IMapInfoElement element, string context)
        {
            var s = ParseString(element, context);

            if (s.Length != 1)
                throw new ParsingException($"{context} was not a properly formatted char: " + s);

            return s[0];
        }

        private static bool ParseBoolean(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            switch (property.Values[0])
            {
                case "true":
                    return true;
                case "false":
                    return false;
                default:
                    throw new ParsingException($"{context} was not a properly formatted boolean: " + property.Values[0]);
            }
        }

        private static bool ParseFlag(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(0, context);

            return true;
        }

        public static ImmutableList<string> ParseStringImmutableList(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            return property.Values.Select(v => ParseQuotedStringWithMinLength(v, 1, context)).ToImmutableList();
        }

        public static ClusterExitText ParseClusterExitText(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            switch (property.Values.Length)
            {
                case 1:
                    return ClusterExitText.Default.WithText(ParseQuotedStringWithMinLength(property.Values[0], 0, context));

                case 2:
                    if (property.Values[0] != "lookup")
                    {
                        throw new ParsingException($"Expected 'lookup' in {context}.");
                    }
                    return new ClusterExitText(
                        text: ParseQuotedStringWithMinLength(property.Values[1], 0, context).ToMaybe(),
                        lookup: true.ToMaybe());

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {property.Values.Length}");
            }
        }

        public static GameBorder ParseGameBorder(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            var v = property.Values;

            switch (v.Length)
            {
                case 4:
                    if (v[0] != "inset")
                    {
                        throw new ParsingException($"Expected 'inset' in {context}: " + v[0]);
                    }

                    return GameBorder.Default.WithColors(new GameBorderColors(
                        topColor: ParseQuotedStringWithMinLength(v[1], 1, context).ToMaybe(),
                        bottomColor: ParseQuotedStringWithMinLength(v[2], 1, context).ToMaybe(),
                        highlightColor: ParseQuotedStringWithMinLength(v[3], 1, context).ToMaybe()));

                case 10:
                    // v[1] is intentionally skipped
                    return GameBorder.Default.WithGraphics(new GameBorderGraphics(
                        offset: ParseInteger(v[0], context).ToMaybe(),
                        topLeft: ParseQuotedStringWithMinLength(v[2], 1, context).ToMaybe(),
                        top: ParseQuotedStringWithMinLength(v[3], 1, context).ToMaybe(),
                        topRight: ParseQuotedStringWithMinLength(v[4], 1, context).ToMaybe(),
                        left: ParseQuotedStringWithMinLength(v[5], 1, context).ToMaybe(),
                        right: ParseQuotedStringWithMinLength(v[6], 1, context).ToMaybe(),
                        bottomLeft: ParseQuotedStringWithMinLength(v[7], 1, context).ToMaybe(),
                        bottom: ParseQuotedStringWithMinLength(v[8], 1, context).ToMaybe(),
                        bottomRight: ParseQuotedStringWithMinLength(v[9], 1, context).ToMaybe()));

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {property.Values.Length}");
            }
        }

        public static Psyched ParsePsyched(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            var v = property.Values;

            switch (v.Length)
            {
                case 2:
                    return Psyched.Default.
                        WithColor1(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithColor2(ParseQuotedStringWithMinLength(v[1], 1, context));

                case 3:
                    return new Psyched(
                        color1: ParseQuotedStringWithMinLength(v[0], 1, context).ToMaybe(),
                        color2: ParseQuotedStringWithMinLength(v[1], 1, context).ToMaybe(),
                        offset: ParseInteger(v[2], context).ToMaybe());

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {property.Values.Length}");
            }
        }

        public static IntermissionBackground ParseIntermissionBackground(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            // TODO: The 'Identifier' thing is a bit of a hack.  ECWolf has some special cases for the texture.

            switch (property.Values.Length)
            {
                case 1:
                    return IntermissionBackground.Default.WithTexture(ParseIdentifier(property.Values[0], context));

                case 2:
                    return IntermissionBackground.Default.
                        WithTexture(ParseIdentifier(property.Values[0], context)).
                        WithTiled(ParseInteger(property.Values[1], context) != 0);

                case 3:
                    return new IntermissionBackground(
                        texture: ParseIdentifier(property.Values[0], context).ToMaybe(),
                        tiled: (ParseInteger(property.Values[1], context) != 0).ToMaybe(),
                        palette: ParseQuotedStringWithMinLength(property.Values[2], 1, context).ToMaybe());

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {property.Values.Length}");
            }
        }

        public static IntermissionTime ParseIntermissionTime(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);
            property.AssertValuesLength(1, context);

            var v = property.Values[0];

            if (double.TryParse(v, out var time))
            {
                return IntermissionTime.Default.WithTime(time);
            }
            else if (v.ToLowerInvariant() == "titletime")
            {
                return IntermissionTime.Default.WithTitleTime(true);
            }
            else
            {
                throw new ParsingException($"Unknown value in {context}: " + v);
            }
        }

        public static NextMapInfo ParseNextMapInfo(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            var v = property.Values;

            switch (v.Length)
            {
                case 1:
                    return NextMapInfo.Default.WithName(ParseQuotedStringWithMinLength(v[0], 1, context));

                case 2:
                    if (v[0] != "EndSequence")
                    {
                        throw new ParsingException($"Expected 'EndSequence' in {context}: " + v[0]);
                    }

                    return new NextMapInfo(
                        name: ParseQuotedStringWithMinLength(v[1], 1, context).ToMaybe(),
                        endSequence: true.ToMaybe());

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {v.Length}");
            }
        }

        public static ExitFadeInfo ParseExitFadeInfo(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            var v = property.Values;

            if (v.Length != 2)
            {
                throw new ParsingException($"Unexpected number of values for {context}: {v.Length}");
            }

            return new ExitFadeInfo(
                color: ParseQuotedStringWithMinLength(v[0], 1, context).ToMaybe(), 
                time: ParseDouble(v[1], context).ToMaybe());            
        }

        public static SpecialAction ParseSpecialAction(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            var v = property.Values;

            switch (v.Length)
            {
                case 2:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context));
                case 3:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context)).
                        WithArg0(ParseInteger(v[2], context));
                case 4:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context)).
                        WithArg0(ParseInteger(v[2], context)).
                        WithArg1(ParseInteger(v[3], context));
                case 5:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context)).
                        WithArg0(ParseInteger(v[2], context)).
                        WithArg1(ParseInteger(v[3], context)).
                        WithArg2(ParseInteger(v[4], context));
                case 6:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context)).
                        WithArg0(ParseInteger(v[2], context)).
                        WithArg1(ParseInteger(v[3], context)).
                        WithArg2(ParseInteger(v[4], context)).
                        WithArg3(ParseInteger(v[5], context));
                case 7:
                    return SpecialAction.Default.
                        WithActorClass(ParseQuotedStringWithMinLength(v[0], 1, context)).
                        WithSpecial(ParseQuotedStringWithMinLength(v[1], 1, context)).
                        WithArg0(ParseInteger(v[2], context)).
                        WithArg1(ParseInteger(v[3], context)).
                        WithArg2(ParseInteger(v[4], context)).
                        WithArg3(ParseInteger(v[5], context)).
                        WithArg4(ParseInteger(v[6], context));

                default:
                    throw new ParsingException($"Unexpected number of values for {context}: {v.Length}");
            }
        }

        private static Intermission ParseIntermission(MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Intermission");

            var name = block.Metadata[0];
            var children = new List<IIntermissionAction>();

            foreach (var property in block.Children)
            {
                var subBlock = property.AssertAsBlock($"Intermission {property.Name}");

                switch (property.Name.ToLower())
                {
                    case "fader":
                        children.Add(ParseFader(subBlock));
                        break;

                    case "gototitle":
                        children.Add(ParseGoToTitle(subBlock));
                        break;

                    case "image":
                        children.Add(ParseImage(subBlock));
                        break;

                    case "textscreen":
                        children.Add(ParseTextScreen(subBlock));
                        break;

                    case "victorystats":
                        children.Add(ParseVictoryStats(subBlock));
                        break;

                    default:
                        throw new ParsingException($"Unknown property '{property.Name}' found in Intermission.");
                }
            }
            return new Intermission(name.ToMaybe(), children);
        }

    }
}