// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using System.Collections.Generic;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax
{
    public sealed class SyntaxAnalyzer
    {
        public IEnumerable<Expression> Analyze(ILexer lexer)
        {
            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.EndOfFile);

                if (idToken.Type == TokenType.EndOfFile) break;

                var name = idToken.TryAsIdentifier().Value;

                // based on current "enable lightlevels" versus blocks
                var nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.OpenParen);

                switch (nextToken.Type)
                {
                    case TokenType.Identifier:
                        yield return ParseGlobalExpression(lexer, name, nextToken.TryAsIdentifier().Value);
                        continue;

                    case TokenType.OpenParen:
                    // parse as block
                    default:
                        throw new ParsingException("Unknown token type.");
                }
            }
        }

        private static Expression ParseGlobalExpression(ILexer lexer, Identifier name, Identifier firstQualifier)
        {
            var qualifiers = new List<Identifier> { firstQualifier };

            var nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.Semicolon);

            while (nextToken.Type != TokenType.Semicolon)
            {
                qualifiers.Add(nextToken.TryAsIdentifier().Value);
                nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.Semicolon);
            }

            return Expression.Simple(
                name: name.ToMaybe(),
                oldnum: Maybe<ushort>.Nothing,
                qualifiers: qualifiers);
        }
    }
}