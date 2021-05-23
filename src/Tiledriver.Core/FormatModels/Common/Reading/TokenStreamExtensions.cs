// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Common.Reading
{
    public static class TokenStreamExtensions
    {
        public static Token? GetNext(this IEnumerator<Token> enumerator) => enumerator.MoveNext() ? enumerator.Current : null;

        public static Token? GetNextSkippingNewlines(this IEnumerator<Token> enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not NewLineToken)
                {
                    return enumerator.Current;
                }
            }

            return null;
        }

        public static TExpected ExpectNext<TExpected>(this IEnumerator<Token> tokenStream) where TExpected : Token
        {
            var nextToken = GetNext(tokenStream);
            return nextToken is TExpected token ? token : throw ParsingException.CreateError<TExpected>(nextToken);
        }

        public static TExpected ExpectNextSkippingNewlines<TExpected>(this IEnumerator<Token> tokenStream) where TExpected : Token
        {
            var nextToken = GetNextSkippingNewlines(tokenStream);
            return nextToken is TExpected token ? token : throw ParsingException.CreateError<TExpected>(nextToken);
        }

        public static Block ParseBlock(this IEnumerator<Token> tokenStream, IdentifierToken name)
        {
            var assignments = new List<Assignment>();

            while (true)
            {
                var token = GetNext(tokenStream);
                switch (token)
                {
                    case IdentifierToken i:
                        ExpectNext<EqualsToken>(tokenStream);
                        assignments.Add(tokenStream.ParseAssignment(i));
                        break;
                    case CloseBraceToken cb:
                        return new Block(name, assignments.ToImmutableArray());
                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }

        public static Assignment ParseAssignment(this IEnumerator<Token> tokenStream, IdentifierToken id)
        {
            var valueToken = GetNext(tokenStream);
            switch (valueToken)
            {
                case IntegerToken i: break;
                case FloatToken f: break;
                case BooleanToken b: break;
                case StringToken s: break;
                default:
                    throw ParsingException.CreateError(valueToken, "value");
            }

            ExpectNext<SemicolonToken>(tokenStream);

            return new Assignment(id, valueToken);
        }
    }
}