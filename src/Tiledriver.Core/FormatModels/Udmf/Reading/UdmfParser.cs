// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.Common.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Udmf.Reading
{
    public static class UdmfParser
    {
        public static IEnumerable<IExpression> Parse(IEnumerable<Token> tokens)
        {
            using var tokenStream = tokens.GetEnumerator();
            while (tokenStream.MoveNext())
            {
                if (tokenStream.Current is IdentifierToken i)
                {
                    switch (tokenStream.GetNext())
                    {
                        case OpenBraceToken _:
                            yield return ParseBlock(i, tokenStream);
                            break;
                        case EqualsToken _:
                            yield return tokenStream.ParseAssignment(i);
                            break;
                        default:
                            throw ParsingException.CreateError(tokenStream.Current, "open brace or equals");
                    }
                }
                else
                {
                    throw ParsingException.CreateError<IdentifierToken>(tokenStream.Current);
                }
            }
        }

        private static IExpression ParseBlock(IdentifierToken name, IEnumerator<Token> tokenStream)
        {
            var token = tokenStream.GetNext();
            switch (token)
            {
                case CloseBraceToken _:
                    return new Block(name, ImmutableArray<Assignment>.Empty);

                case IdentifierToken i:
                    return ParseBlock(name, i, tokenStream);
                default:
                    throw ParsingException.CreateError(token, "identifier or end of block");
            }
        }

        private static Block ParseBlock(IdentifierToken name, IdentifierToken fieldName, IEnumerator<Token> tokenStream)
        {
            var assignments = new List<Assignment>();

            tokenStream.ExpectNext<EqualsToken>();
            assignments.Add(tokenStream.ParseAssignment(fieldName));

            while (true)
            {
                var token = tokenStream.GetNext();
                switch (token)
                {
                    case IdentifierToken i:
                        tokenStream.ExpectNext<EqualsToken>();
                        assignments.Add(tokenStream.ParseAssignment(i));
                        break;
                    case CloseBraceToken cb:
                        return new Block(name, assignments.ToImmutableArray());
                    default:
                        throw ParsingException.CreateError(token, "identifier or end of block");
                }
            }
        }        
    }
}