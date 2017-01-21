// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

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

        private static Cluster ParseClusterMetadata(Cluster cluster, ImmutableArray<string> metadata)
        {
            AssertMetadataLength("Cluster", metadata, 1);

            var id = ParseInt(metadata[0], "Cluster id");

            return cluster.WithId(id);
        }

        private static Episode ParseEpisodeMetadata(Episode episode, ImmutableArray<string> metadata)
        {
            AssertMetadataLength("Episode", metadata, 1);

            var map = ParseQuotedStringWithMinLength(metadata[0], 1, "Episode map");

            return episode.WithMap(map);
        }

        private static Intermission ParseIntermissionMetadata(Intermission intermission, ImmutableArray<string> metadata)
        {
            AssertMetadataLength("Intermission", metadata, 1);

            return intermission.WithName(metadata[0]);
        }

        private static Map ParseMapMetadata(Map map, ImmutableArray<string> metadata)
        {
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

        private static void AssertMetadataLength(string className, ImmutableArray<string> metadata, int length)
        {
            if (metadata.Length != length)
            {
                throw new ParsingException($"Expected {length} pieces of metadata for {className} but found {metadata.Length}.");
            }
        }

        public static int ParseInt(string s, string context)
        {
            int result;
            if (!int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out result))
            {
                throw new ParsingException($"{context} was not an integer.");
            }
            return result;
        }

        public static Maybe<string> ParseQuotedString(string s)
        {
            if (s.Length < 2 || s[0] != '"' || s.Last() != '"')
                return Maybe<string>.Nothing;

            return s.Substring(1).RemoveLast(1).ToMaybe();
        }

        public static string ParseQuotedStringWithMinLength(string s, int minLength, string context)
        {
            var result = ParseQuotedString(s);

            if (result.HasValue)
            {
                var str = result.Value;
                if (str.Length >= minLength)
                {
                    return str;
                }
            }
            throw new ParsingException($"{context} was not a properly formatted string.");
        }
    }
}