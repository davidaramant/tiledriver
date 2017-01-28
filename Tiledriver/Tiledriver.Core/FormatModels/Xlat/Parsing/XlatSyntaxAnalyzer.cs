﻿// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing
{
    public sealed class XlatSyntaxAnalyzer
    {
        private readonly IResourceProvider _resourceProvider;

        public XlatSyntaxAnalyzer(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public IEnumerable<Expression> Analyze(ILexer lexer)
        {
            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.EndOfFile);

                if (idToken.Type == TokenType.EndOfFile) break;

                var name = idToken.TryAsIdentifier().Value;

                if (name.ToString() == "include")
                {
                    foreach (var exp in ParseInclude(lexer))
                    {
                        yield return exp;
                    }
                    continue;
                }

                // based on current "enable lightlevels" versus blocks
                var nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.OpenParen);

                switch (nextToken.Type)
                {
                    case TokenType.Identifier:
                        yield return ParseGlobalExpression(lexer, name, nextToken);
                        continue;

                    case TokenType.OpenParen:
                        yield return ParseBlock(lexer, name);
                        continue;

                    default:
                        throw new ParsingException("Unknown token type.");
                }
            }
        }

        private IEnumerable<Expression> ParseInclude(ILexer lexer)
        {
            var lumpName = lexer.MustReadTokenOfTypes(TokenType.String).AsString();

            using (var ms = _resourceProvider.Lookup(lumpName))
            using (var reader = new StreamReader(ms, Encoding.ASCII))
            {
                // The ToArray is needed because otherwise the stream is closed
                return Analyze(new XlatLexer(reader)).ToArray();
            }
        }

        private static Expression ParseGlobalExpression(ILexer lexer, Identifier name, Token firstQualifier)
        {
            var qualifiers = new List<Token> { firstQualifier };

            var nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.Semicolon);

            while (nextToken.Type != TokenType.Semicolon)
            {
                qualifiers.Add(nextToken);
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
            var qualifiers = new List<Token>();

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(
                    TokenType.Integer,
                    TokenType.Identifier,
                    TokenType.String,
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

                    case TokenType.String:
                    case TokenType.Identifier:
                        qualifiers.Add(nextToken);
                        break;

                    case TokenType.OpenParen:
                        return ParseExpressionWithList(lexer, name, oldNum, qualifiers);

                    default:
                        throw new ParsingException("Unknown state.");
                }
            }
        }

        private static Expression ParseExpressionWithList(
            ILexer lexer,
            Identifier name,
            Maybe<ushort> oldnum,
            IEnumerable<Token> qualifiers)
        {
            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(
                    TokenType.CloseParen,
                    TokenType.Identifier,
                    TokenType.String);

                switch (nextToken.Type)
                {
                    case TokenType.CloseParen:
                        return Expression.Simple(name, oldnum, qualifiers);
                    case TokenType.Identifier:
                        return ParseExpressionWithProperties(lexer, name, oldnum, qualifiers,
                            nextToken.TryAsIdentifier().Value);
                    case TokenType.String:
                        return ParseExpressionWithStringValues(lexer, name, oldnum, qualifiers, nextToken);
                    default:
                        throw new ParsingException("Unknown state");
                }
            }
        }

        private static Expression ParseExpressionWithProperties(
            ILexer lexer,
            Identifier name,
            Maybe<ushort> oldnum,
            IEnumerable<Token> qualifiers,
            Identifier identifierOfFirstProperty)
        {
            var assignments = new List<Assignment> { ParseAssignment(identifierOfFirstProperty, lexer) };

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (nextToken.Type == TokenType.CloseParen)
                {
                    return Expression.PropertyList(name: name.ToMaybe(), oldnum: oldnum, qualifiers: qualifiers, properties: assignments);
                }
                else
                {
                    var assignmentName = nextToken.TryAsIdentifier().Value;
                    assignments.Add(ParseAssignment(assignmentName, lexer));
                }
            }
        }

        public static Assignment ParseAssignment(Identifier name, ILexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.Equal);

            var valueToken = lexer.MustReadValueToken();

            lexer.MustReadTokenOfTypes(TokenType.Semicolon);

            return new Assignment(name, valueToken);
        }

        private static Expression ParseExpressionWithStringValues(
            ILexer lexer,
            Identifier name,
            Maybe<ushort> oldnum,
            IEnumerable<Token> qualifiers,
            Token firstValue)
        {
            var values = new List<Token> { firstValue };

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(
                    TokenType.Comma,
                    TokenType.CloseParen);

                if (nextToken.Type == TokenType.CloseParen)
                    return Expression.ValueList(name.ToMaybe(), oldnum, qualifiers, values);

                var stringToken = lexer.MustReadTokenOfTypes(TokenType.String);

                values.Add(stringToken);
            }
        }

        private static Expression ParseValueList(ILexer lexer)
        {
            // Leave it up to the next stage to make sense of this mess.  
            // There's no easy way to transmit this info properly to the next stage.
            var values = new List<Token>();

            while (true)
            {
                var nextToken = lexer.MustReadTokenOfTypes(
                    TokenType.CloseParen,
                    TokenType.Comma,
                    TokenType.Integer,
                    TokenType.Meta,
                    TokenType.Identifier,
                    TokenType.Pipe);

                if (nextToken.Type == TokenType.CloseParen)
                    return Expression.ValueList(
                        name: Maybe<Identifier>.Nothing,
                        oldnum: Maybe<ushort>.Nothing,
                        qualifiers: Enumerable.Empty<Token>(),
                        values: values);

                values.Add(nextToken);
            }
        }
    }
}