// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
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
                        yield return ParseBlock(lexer, name);
                        continue;

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
                name: name,
                oldnum: Maybe<ushort>.Nothing,
                qualifiers: qualifiers);
        }

        private static Expression ParseBlock(ILexer lexer, Identifier name)
        {
            var subexpressions = new List<Expression>();

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(TokenType.CloseParen, TokenType.Identifier, TokenType.OpenParen);
                switch (nextToken.Type)
                {
                    case TokenType.CloseParen:
                        return Expression.Block(name, subexpressions);

                    case TokenType.Identifier:
                        subexpressions.Add(ParseSubExpression(lexer, nextToken.TryAsIdentifier().Value));
                        continue;

                    case TokenType.OpenParen:
                        subexpressions.Add(ParseValueList(lexer));
                        continue;

                    default:
                        throw new ParsingException("Unknown token type.");
                }
            }
        }

        private static Expression ParseSubExpression(ILexer lexer, Identifier name)
        {
            var oldNum = Maybe<ushort>.Nothing;
            var qualifiers = new List<Identifier>();

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(
                    TokenType.Integer, 
                    TokenType.Identifier, 
                    TokenType.OpenParen,
                    TokenType.Semicolon);

                switch (nextToken.Type)
                {
                    case TokenType.Integer:
                        if (oldNum.HasValue)
                        {
                            throw new ParsingException("Already found an old num!");
                        }
                        oldNum = nextToken.AsUshort();
                        break;

                    case TokenType.Semicolon:
                        return Expression.Simple(
                            name: name,
                            oldnum: oldNum,
                            qualifiers: qualifiers);

                    case TokenType.Identifier:
                        qualifiers.Add(nextToken.TryAsIdentifier().Value);
                        break;

                    case TokenType.OpenParen:
                    default:
                        throw new ParsingException("Unknown state.");
                }
            }
        }

        private static Expression ParseValueList(ILexer lexer)
        {
            throw new NotImplementedException();
        }
    }
}