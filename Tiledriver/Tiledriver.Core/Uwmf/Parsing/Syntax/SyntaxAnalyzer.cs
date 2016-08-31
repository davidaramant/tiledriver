﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.Uwmf.Parsing.Syntax
{
    public sealed class SyntaxAnalyzer
    {
        public UwmfSyntaxTree Analyze(IUwmfLexer lexer)
        {
            var globalAssignments = new List<Assignment>();
            var blocks = new List<Block>();
            var arrayBlocks = new List<ArrayBlock>();

            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.EndOfFile);

                if (idToken.Type == TokenType.EndOfFile) break;

                var name = new Identifier(idToken.ValueAsString);

                var nextToken = lexer.MustReadTokenOfTypes(TokenType.Equal, TokenType.OpenParen);
                if (nextToken.Type == TokenType.Equal)
                {
                    globalAssignments.Add(ParseGlobalAssignment(name, lexer));
                }
                else // Must be a block
                {
                    var firstBlockToken =
                        lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.OpenParen, TokenType.CloseParen);

                    switch (firstBlockToken.Type)
                    {
                        case TokenType.Identifier:
                            var assignmentName = new Identifier(firstBlockToken.ValueAsString);
                            var assignment = ParseAssignment(assignmentName, lexer);
                            blocks.Add(ParseBlock(name, assignment, lexer));
                            break;

                        case TokenType.CloseParen:
                            blocks.Add(new Block(name, Enumerable.Empty<Assignment>()));
                            break;

                        case TokenType.OpenParen:
                            arrayBlocks.Add(ParseArrayBlock(name,lexer));
                            break;

                        default:
                            throw new UwmfParsingException("This can't happen.");
                    }
                }
            }
            return new UwmfSyntaxTree(globalAssignments, blocks, arrayBlocks);
        }

        private static ArrayBlock ParseArrayBlock(Identifier blockName, IUwmfLexer lexer)
        {
            var tuples = new List<IReadOnlyList<int>>
            {
                ReadTuple(lexer)
            };

            while (lexer.MustReadTokenOfTypes(TokenType.Comma, TokenType.CloseParen).Type == TokenType.Comma)
            {
                lexer.MustReadTokenOfTypes(TokenType.OpenParen);
                tuples.Add(ReadTuple(lexer));
            }

            return new ArrayBlock(blockName, tuples);
        }

        private static IReadOnlyList<int> ReadTuple(IUwmfLexer lexer)
        {
            var tuple = new List<int>();
            // This assumes the opening paren has already been read.

            var firstToken = lexer.MustReadTokenOfTypes(TokenType.Integer, TokenType.CloseParen);
            if (firstToken.Type == TokenType.CloseParen) return tuple;

            tuple.Add(firstToken.ValueAsInt);

            while (true)
            {
                var token = lexer.MustReadTokenOfTypes(TokenType.Comma, TokenType.CloseParen);
                if (token.Type == TokenType.CloseParen) return tuple;

                tuple.Add(lexer.MustReadTokenOfTypes(TokenType.Integer).ValueAsInt);
            }
        }

        private static Block ParseBlock(Identifier blockName, Assignment firstAssigment, IUwmfLexer lexer)
        {
            var assignments = new List<Assignment> { firstAssigment };

            while (true)
            {
                var token = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (token.Type == TokenType.CloseParen)
                {
                    return new Block(blockName, assignments);
                }
                else
                {
                    var assignmentName = token.ValueAsString;
                    assignments.Add(ParseAssignment(new Identifier(assignmentName), lexer));
                }
            }
        }

        private static Assignment ParseAssignment(Identifier name, IUwmfLexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.Equal);

            return ParseGlobalAssignment(name, lexer);
        }

        private static Assignment ParseGlobalAssignment(Identifier name, IUwmfLexer lexer)
        {
            var valueToken = lexer.MustReadValueToken();

            lexer.MustReadTokenOfTypes(TokenType.Semicolon);

            return new Assignment(name, valueToken);
        }
    }
}