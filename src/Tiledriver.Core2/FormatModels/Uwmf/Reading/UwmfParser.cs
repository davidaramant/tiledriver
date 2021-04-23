// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.UnifiedLexing;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading
{
    public static class UwmfParser
    {
        public static IEnumerable<IExpression> Parse(IEnumerable<Token> tokens)
        {
            // UWMF does not care about newlines
            tokens = tokens.Where(t => t is not NewLineToken);

            using var tokenStream = tokens.GetEnumerator();
            while (tokenStream.MoveNext())
            {
                if (tokenStream.Current is IdentifierToken i)
                {
                    switch (GetNext(tokenStream))
                    {
                        case OpenBraceToken _:
                            yield return ParseBlockOrTupleList(i, tokenStream);
                            break;
                        case EqualsToken _:
                            yield return ParseAssignment(i, tokenStream);
                            break;
                        default:
                            throw CreateError(tokenStream.Current, "open brace or equals");
                    }
                }
                else
                {
                    throw CreateError<IdentifierToken>(tokenStream.Current);
                }
            }
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

        private static IExpression ParseBlockOrTupleList(IdentifierToken name, IEnumerator<Token> tokenStream)
        {
            var token = GetNext(tokenStream);
            switch (token)
            {
                case OpenBraceToken ob:
                    return ParseIntTupleList(name, ob.Location, tokenStream);

                case CloseBraceToken _:
                    return new Block(name, ImmutableArray<Assignment>.Empty);

                case IdentifierToken i:
                    return ParseBlock(name, i, tokenStream);
                default:
                    throw CreateError(token, "identifier, end of block, or start of tuple");
            }
        }

        private static Block ParseBlock(IdentifierToken name, IdentifierToken fieldName, IEnumerator<Token> tokenStream)
        {
            var assignments = new List<Assignment>();

            ExpectNext<EqualsToken>(tokenStream);
            assignments.Add(ParseAssignment(fieldName, tokenStream));

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

        private static IntTupleBlock ParseIntTupleList(IdentifierToken name, FilePosition startLocation, IEnumerator<Token> tokenStream)
        {
            var tuples = ImmutableArray.CreateBuilder<IntTuple>();
            tuples.Add(ParseIntTuple(startLocation, tokenStream));

            while (true)
            {
                var token = GetNext(tokenStream);
                switch (token)
                {
                    case CommaToken _:
                        var openBrace = ExpectNext<OpenBraceToken>(tokenStream);
                        tuples.Add(ParseIntTuple(openBrace.Location, tokenStream));
                        break;

                    case CloseBraceToken _:
                        return new IntTupleBlock(name, tuples.ToImmutable());

                    default:
                        throw CreateError(token, "comma or end of block");
                }
            }

        }

        private static IntTuple ParseIntTuple(FilePosition startLocation, IEnumerator<Token> tokenStream)
        {
            var ints = ImmutableList.CreateBuilder<IntegerToken>();

            var token = GetNext(tokenStream);
            switch (token)
            {
                case CloseBraceToken _:
                    return new IntTuple(startLocation,ints.ToImmutable());
                case IntegerToken i:
                    ints.Add(i);
                    break;
                default:
                    throw CreateError(token, "integer or end of tuple");
            }

            while (true)
            {
                token = GetNext(tokenStream);
                switch (token)
                {
                    case CloseBraceToken _:
                        return new IntTuple(startLocation, ints.ToImmutable());

                    case CommaToken _:
                        token = GetNext(tokenStream);
                        if (token is IntegerToken i)
                        {
                            ints.Add(i);
                        }
                        else
                        {
                            throw CreateError(token, "integer");
                        }

                        break;

                    default:
                        throw CreateError(token, "Comma or end of tuple");
                }
            }
        }
    }
}