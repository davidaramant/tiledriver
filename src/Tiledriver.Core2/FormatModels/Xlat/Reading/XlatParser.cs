// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;

namespace Tiledriver.Core.FormatModels.Xlat.Reading
{
    public static partial class XlatParser
    {
        public static MapTranslation Parse(
            IEnumerable<Token> tokens,
            IResourceProvider resourceProvider)
        {
            List<TileMappings> tileMappings = new();
            List<IThingMapping> thingMappings = new();
            List<FlatMappings> flatMappings = new();

            var tokenSteam = new TokenStream(tokens, resourceProvider);
            using var enumerator = tokenSteam.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var id = enumerator.Current as IdentifierToken;
                if (id == null)
                {
                    throw new ParsingException($"Unexpected token: {enumerator.Current}");
                }

                switch (id.Id.ToLower())
                {
                    case "enable":
                    case "disable":
                        // global flag, ignore
                        Skip<IdentifierToken>(enumerator);
                        Skip<SemicolonToken>(enumerator);
                        break;

                    case "music":
                        throw new ParsingException("This should be ignored");

                    case "tiles":
                        tileMappings.Add(ParseTileMappings(enumerator));
                        break;

                    case "things":
                        thingMappings.AddRange(ParseThingMappings(enumerator));
                        break;

                    case "flats":
                        flatMappings.Add(ParseFlatMappings(enumerator));
                        break;

                    default:
                        throw new ParsingException($"Unexpected identifier: {id}");
                }

            }

            return new MapTranslation(
                Merge(tileMappings),
                thingMappings,
                Merge(flatMappings));
        }

        private static TileMappings Merge(IEnumerable<TileMappings> tileMappings)
        {
            return new TileMappings(
                ImmutableList<AmbushModzone>.Empty,
                ImmutableList<ChangeTriggerModzone>.Empty,
                ImmutableList<TileTemplate>.Empty,
                ImmutableList<TriggerTemplate>.Empty,
                ImmutableList<ZoneTemplate>.Empty);
        }

        private static FlatMappings Merge(IEnumerable<FlatMappings> flatMappings)
        {
            return new FlatMappings(
                ImmutableList<string>.Empty,
                ImmutableList<string>.Empty);
        }

        private static void Skip<TTOken>(IEnumerator<Token> enumerator) where TTOken : Token
        {
            if (!enumerator.MoveNext() || enumerator.Current is TTOken)
            {
                throw new ParsingException($"Unexpected token: {enumerator.Current}");
            }
        }

        private static TileMappings ParseTileMappings(IEnumerator<Token> enumerator)
        {
            return new TileMappings(
                ImmutableList<AmbushModzone>.Empty,
                ImmutableList<ChangeTriggerModzone>.Empty,
                ImmutableList<TileTemplate>.Empty,
                ImmutableList<TriggerTemplate>.Empty,
                ImmutableList<ZoneTemplate>.Empty);
        }

        private static FlatMappings ParseFlatMappings(IEnumerator<Token> enumerator)
        {
            return new FlatMappings(
                ImmutableList<string>.Empty,
                ImmutableList<string>.Empty);
        }

        private static IEnumerable<IThingMapping> ParseThingMappings(IEnumerator<Token> enumerator)
        {
            return Enumerable.Empty<IThingMapping>();
        }
    }
}