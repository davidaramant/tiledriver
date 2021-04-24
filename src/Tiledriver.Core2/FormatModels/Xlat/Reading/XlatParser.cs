// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

            var tokenSource = new TokenSource(tokens, resourceProvider, XlatLexer.Create);
            using var tokenStream = tokenSource.GetEnumerator();

            while (tokenStream.MoveNext())
            {
                var id = tokenStream.Current as IdentifierToken;
                if (id == null)
                {
                    throw new ParsingException($"Unexpected token: {tokenStream.Current}");
                }

                switch (id.Id.ToLower())
                {
                    case "enable":
                    case "disable":
                        // global flag, ignore
                        tokenStream.ExpectNext<IdentifierToken>();
                        tokenStream.ExpectNext<SemicolonToken>();
                        break;

                    case "music":
                        throw new ParsingException("This should be ignored");

                    case "tiles":
                        tileMappings.Add(ParseTileMappings(tokenStream));
                        break;

                    case "things":
                        thingMappings.AddRange(ParseThingMappings(tokenStream));
                        break;

                    case "flats":
                        flatMappings.Add(ParseFlatMappings(tokenStream));
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
            throw new NotImplementedException();
            return new TileMappings(
                ImmutableList<AmbushModzone>.Empty,
                ImmutableList<ChangeTriggerModzone>.Empty,
                ImmutableList<TileTemplate>.Empty,
                ImmutableList<TriggerTemplate>.Empty,
                ImmutableList<ZoneTemplate>.Empty);
        }

        private static FlatMappings Merge(IEnumerable<FlatMappings> flatMappings)
        {
            throw new NotImplementedException();
            return new FlatMappings(
                ImmutableList<string>.Empty,
                ImmutableList<string>.Empty);
        }

        private static TileMappings ParseTileMappings(IEnumerator<Token> tokenStream)
        {
            tokenStream.ExpectNext<OpenBraceToken>();

            var ambushModzones = new List<AmbushModzone>();
            var changeTriggerModzones = new List<ChangeTriggerModzone>();
            var tileTemplates = new List<TileTemplate>();
            var triggerTemplates = new List<TriggerTemplate>();
            var zoneTemplates = new List<ZoneTemplate>();

            while (true)
            {
                var token = tokenStream.GetNext();
                switch (token)
                {
                    case IdentifierToken id:
                        switch (id.Id.ToLower())
                        {
                            case "modzone":
                                break;

                            case "tile":
                                break;

                            case "trigger":
                                break;

                            case "zone":
                                zoneTemplates.Add(ParseZone(id, tokenStream));
                                break;

                            default:
                                throw ParsingException.CreateError(id, "unknown identifier");
                        }
                        break;

                    case CloseBraceToken:
                        return new TileMappings(
                            ambushModzones.ToImmutableList(),
                            changeTriggerModzones.ToImmutableList(),
                            tileTemplates.ToImmutableList(),
                            triggerTemplates.ToImmutableList(),
                            zoneTemplates.ToImmutableList());

                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }

        private static ZoneTemplate ParseZone(IdentifierToken id, IEnumerator<Token> tokenStream)
        {
            var oldNumToken = tokenStream.ExpectNext<IntegerToken>();

            var block = tokenStream.ParseBlock(id);
            var fields = block.GetFieldAssignments();

            return new ZoneTemplate(
                oldNumToken.ValueAsUshort(() => ParsingException.CreateError(oldNumToken, "UShort value")),
                Comment: fields.GetOptionalFieldValue("comment", ""));
        }

        private static FlatMappings ParseFlatMappings(IEnumerator<Token> tokenStream)
        {
            var ceilings = new List<string>();
            var floors = new List<string>();

            tokenStream.ExpectNext<OpenBraceToken>();

            while (true)
            {
                var token = tokenStream.GetNext();
                switch (token)
                {
                    case IdentifierToken id:
                        switch (id.Id.ToLower())
                        {
                            case "ceiling":
                                ceilings.AddRange(ParseStringList(tokenStream));
                                break;

                            case "floor":
                                floors.AddRange(ParseStringList(tokenStream));
                                break;

                            default:
                                throw ParsingException.CreateError(id, "unknown identifier");
                        }
                        break;

                    case CloseBraceToken:
                        return new FlatMappings(
                            ceilings.ToImmutableList(),
                            floors.ToImmutableList());

                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }

        private static List<string> ParseStringList(IEnumerator<Token> tokenStream)
        {
            var strings = new List<string>();

            tokenStream.ExpectNext<OpenBraceToken>();

            while (true)
            {
                var token = tokenStream.GetNext();
                switch (token)
                {
                    case CommaToken:
                        break;

                    case StringToken s:
                        strings.Add(s.Value);
                        break;

                    case CloseBraceToken:
                        return strings;

                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }

        private static IEnumerable<IThingMapping> ParseThingMappings(IEnumerator<Token> tokenStream)
        {
            throw new NotImplementedException();
        }
    }
}