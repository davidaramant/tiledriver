// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

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

            var tokenSteam = new TokenStream(tokens, resourceProvider, XlatLexer.Create);
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

        static ParsingException CreateError(Token? token, string expected)
        {
            if (token == null)
            {
                return new ParsingException("Unexpected end of file");
            }
            return new ParsingException($"Unexpected token {token.GetType().Name} (expected {expected}) on {token.Location}");
        }

        static ParsingException CreateError<TExpected>(Token? token) => CreateError(token, typeof(TExpected).Name);

        static Token? GetNext(IEnumerator<Token> enumerator) => enumerator.MoveNext() ? enumerator.Current : null;

        static TExpected ExpectNext<TExpected>(IEnumerator<Token> tokenStream) where TExpected : Token
        {
            var nextToken = GetNext(tokenStream);
            return nextToken is TExpected token ? token : throw CreateError<TExpected>(nextToken);
        }

        private static TileMappings ParseTileMappings(IEnumerator<Token> enumerator)
        {
            ExpectNext<OpenBraceToken>(enumerator);

            var ambushModzones = new List<AmbushModzone>();
            var changeTriggerModzones = new List<ChangeTriggerModzone>();
            var tileTemplates = new List<TileTemplate>();
            var triggerTemplates = new List<TriggerTemplate>();
            var zoneTemplates = new List<ZoneTemplate>();

            while (true)
            {
                var token = GetNext(enumerator);
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
                                zoneTemplates.Add(ParseZone(id, enumerator));
                                break;

                            default:
                                throw CreateError(id, "unknown identifier");
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
                        throw CreateError(token, "identifier or end of block");
                }
            }
        }

        // TODO: Move a lot of the stuff in UwmfSemanticAnalyzer to a common place

        private static ZoneTemplate ParseZone(IdentifierToken id, IEnumerator<Token> tokenStream)
        {
            var oldNumToken = ExpectNext<IntegerToken>(tokenStream);

            var block = ParseBlock(id, tokenStream);
            var lookup = block.GetFieldAssignments();

            return new ZoneTemplate(
                oldNumToken.ValueAsUshort(() => CreateError(oldNumToken, "UShort value")),
                Comment: "");
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

        private static Block ParseBlock(IdentifierToken name, IEnumerator<Token> tokenStream)
        {
            var assignments = new List<Assignment>();

            while (true)
            {
                var token = GetNext(tokenStream);
                switch (token)
                {
                    case IdentifierToken i:
                        ExpectNext<EqualsToken>(tokenStream);
                        assignments.Add(ParseAssignment(i, tokenStream));
                        break;
                    case CloseBraceToken cb:
                        return new Block(name, assignments.ToImmutableArray());
                    default:
                        throw CreateError(token, "identifier or end of block");
                }
            }
        }

        private static Assignment ParseAssignment(IdentifierToken id, IEnumerator<Token> tokenStream)
        {
            var valueToken = GetNext(tokenStream);
            switch (valueToken)
            {
                case IntegerToken i: break;
                case FloatToken f: break;
                case BooleanToken b: break;
                case StringToken s: break;
                default:
                    throw CreateError(valueToken, "value");
            }

            ExpectNext<SemicolonToken>(tokenStream);

            return new Assignment(id, valueToken);
        }
    }
}