// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapInfos.Parsing
{
    public static partial class MapInfoParser
    {
        public static MapInfo Parse(IEnumerable<IMapInfoElement> elements)
        {
            return null;
        }

        private static Cluster ParseClusterMetadata(Cluster cluster, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Cluster");

            var id = ParseInt(block.Metadata[0], "Cluster id");

            return cluster.WithId(id);
        }

        private static Episode ParseEpisodeMetadata(Episode episode, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Episode");

            var map = ParseQuotedStringWithMinLength(block.Metadata[0], 1, "Episode map");

            return episode.WithMap(map);
        }

        private static Intermission ParseIntermissionMetadata(Intermission intermission, MapInfoBlock block)
        {
            block.AssertMetadataLength(1, "Intermission");

            return intermission.WithName(block.Metadata[0]);
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

        private static Map ParseMapLumpMetadata(Map map, string metadata)
        {
            var mapLump = ParseQuotedStringWithMinLength(metadata, 1, "Map lump name");
            return map.WithMapLump(mapLump);
        }

        private static int ParseInt(string s, string context)
        {
            int result;
            if (!int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                throw new ParsingException($"{context} was not an integer.");
            }
            return result;
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
            throw new ParsingException($"{context} was not a properly formatted string.");
        }

        private static int ParseInteger(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            return ParseInt(property.Values[0], context);
        }

        private static double ParseDouble(IMapInfoElement element, string context)
        {
            var property = element.AssertAsProperty(context);

            property.AssertValuesLength(1, context);

            double result;
            if (!double.TryParse(property.Values[0], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                throw new ParsingException($"{context} was not a floating point number.");
            }
            return result;
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
            throw new ParsingException($"{context} was not a properly formatted string.");
        }

        private static char ParseChar(IMapInfoElement element, string context)
        {
            var s = ParseString(element, context);

            if (s.Length != 1)
                throw new ParsingException($"{context} was not a properly formatted char.");

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
                    throw new ParsingException($"{context} was not a properly formatted boolean.");
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
            throw new NotImplementedException();
            return ImmutableList<string>.Empty;
        }

        public static ClusterExitText ParseClusterExitText(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return ClusterExitText.Default;
        }

        public static GameBorder ParseGameBorder(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return GameBorder.Default;
        }

        public static MenuColor ParseMenuColor(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return MenuColor.Default;
        }

        public static ImmutableList<IntermissionAction> ParseIntermissionActionImmutableList(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return ImmutableList<IntermissionAction>.Empty;
        }

        public static IntermissionBackground ParseIntermissionBackground(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return IntermissionBackground.Default;
        }

        public static IntermissionDraw ParseIntermissionDraw(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return IntermissionDraw.Default;
        }

        public static TextScreenPosition ParseTextScreenPosition(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return TextScreenPosition.Default;
        }

        public static NextMapInfo ParseNextMapInfo(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return NextMapInfo.Default;
        }

        public static ImmutableList<SpecialAction> ParseSpecialActionImmutableList(IMapInfoElement element, string context)
        {
            throw new NotImplementedException();
            return ImmutableList<SpecialAction>.Empty;
        }

        private static Intermission ParseIntermission(MapInfoBlock block)
        {
            throw new NotImplementedException();
            var intermission = Intermission.Default;
            intermission = ParseIntermissionMetadata(intermission, block);
            foreach (var property in block.Children)
            {
                switch (property.Name.ToString())
                {
                    default:
                        throw new ParsingException($"Unknown property {property.Name} found in Intermission.");
                }
            }
            return intermission;
        }

    }
}