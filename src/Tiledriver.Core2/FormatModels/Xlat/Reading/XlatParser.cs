// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Uwmf;

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
                flatMappings.LastOrDefault());
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
                                ParseModzone(tokenStream, id, ambushModzones, changeTriggerModzones);
                                break;

                            case "tile":
                                tileTemplates.Add(ParseTileTemplate(tokenStream, id));
                                break;

                            case "trigger":
                                triggerTemplates.Add(ParseTriggerTemplate(tokenStream, id));
                                break;

                            case "zone":
                                zoneTemplates.Add(ParseZone(tokenStream, id));
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

        private static void ParseModzone(
            IEnumerator<Token> tokenStream,
            IdentifierToken id,
            List<AmbushModzone> ambushModzones,
            List<ChangeTriggerModzone> changeTriggerModzones)
        {
            var oldNum =
                tokenStream
                    .ExpectNext<IntegerToken>()
                    .ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

            bool fillZone = false;

            var next = tokenStream.ExpectNext<IdentifierToken>();

            if (next.Id.ToLower() == "fillzone")
            {
                fillZone = true;
                next = tokenStream.ExpectNext<IdentifierToken>();
            }

            if (next.Id.ToLower() == "ambush")
            {
                tokenStream.ExpectNext<SemicolonToken>();

                ambushModzones.Add(new AmbushModzone(oldNum, fillZone));
            }
            else if (next.Id.ToLower() == "changetrigger")
            {
                var action = tokenStream.ExpectNext<StringToken>().Value;

                var block = tokenStream.ParseBlock(id);

                var triggerTemplate = ReadTriggerTemplate(oldNum, block);

                changeTriggerModzones.Add(new ChangeTriggerModzone(oldNum, action, triggerTemplate, fillZone));
            }
            else
            {
                throw ParsingException.CreateError(next, "ambush or changetrigger");
            }
        }

        private static TileTemplate ParseTileTemplate(IEnumerator<Token> tokenStream, IdentifierToken id)
        {
            var oldNum =
                tokenStream
                    .ExpectNext<IntegerToken>()
                    .ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

            var block = tokenStream.ParseBlock(id);

            return ReadTileTemplate(oldNum, block);
        }

        private static TriggerTemplate ParseTriggerTemplate(IEnumerator<Token> tokenStream, IdentifierToken id)
        {
            var oldNum =
                tokenStream
                    .ExpectNext<IntegerToken>()
                    .ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

            var block = tokenStream.ParseBlock(id);

            return ReadTriggerTemplate(oldNum, block);
        }

        private static ZoneTemplate ParseZone(IEnumerator<Token> tokenStream, IdentifierToken id)
        {
            var oldNum =
                tokenStream
                    .ExpectNext<IntegerToken>()
                    .ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));

            var block = tokenStream.ParseBlock(id);
            var fields = block.GetFieldAssignments();

            return new ZoneTemplate(
                oldNum,
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
            tokenStream.ExpectNext<OpenBraceToken>();

            var thingMappings = new List<IThingMapping>();

            while (true)
            {
                var token = tokenStream.GetNext();
                switch (token)
                {
                    case IdentifierToken id:
                        switch (id.Id.ToLower())
                        {
                            case "elevator":
                                thingMappings.Add(ParseElevator(tokenStream, id));
                                break;

                            case "trigger":
                                break;

                            default:
                                throw ParsingException.CreateError(id, "unknown identifier");
                        }
                        break;

                    case OpenBraceToken:
                        break;

                    case CloseBraceToken:
                        return thingMappings;

                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }

        private static Elevator ParseElevator(IEnumerator<Token> tokenStream, IdentifierToken id)
        {
            var oldNum =
                tokenStream
                    .ExpectNext<IntegerToken>()
                    .ValueAsUshort(token => ParsingException.CreateError(token, "UShort value"));
            tokenStream.ExpectNext<SemicolonToken>();

            return new Elevator(oldNum);
        }
    }
}