// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(ILexer lexer)
        {
            var map = new Map();

            TokenType nextToken;
            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndOfFile)
            {
                if (nextToken != TokenType.Identifier)
                {
                    throw new ParsingException("Expecting identifier when parsing map.");
                }

                var identifier = lexer.ReadIdentifier();
                if (identifier.Name == "namespace")
                {
                    if (lexer.DetermineNextToken() != TokenType.Assignment)
                    {
                        throw new ParsingException("Expecting assignment of map.namespace");
                    }
                    lexer.MovePastAssignment();
                    map.Namespace = lexer.ReadString();
                    if (lexer.DetermineNextToken() != TokenType.EndOfAssignment)
                    {
                        throw new ParsingException("Expecting assignment of map.namespace");
                    }
                    lexer.MovePastAssignment();
                }

                nextToken = lexer.DetermineNextToken();
            }

            map.CheckSemanticValidity();

            return map;
        }

        private static int ParseIntegerNumberAssignment(ILexer lexer, string context)
        {
            return ParseAssignment(lexer, l => l.ReadIntegerNumber(), context);
        }

        private static double ParseFloatingPointNumberAssignment(ILexer lexer, string context)
        {
            return ParseAssignment(lexer, l => l.ReadFloatingPointNumber(), context);
        }

        private static bool ParseBooleanAssignment(ILexer lexer, string context)
        {
            return ParseAssignment(lexer, l => l.ReadBoolean(), context);
        }

        private static string ParseStringAssignment(ILexer lexer, string context)
        {
            return ParseAssignment(lexer, l => l.ReadString(), context);
        }

        private static T ParseAssignment<T>(ILexer lexer, Func<ILexer, T> readValue, string context)
        {
            if (lexer.DetermineNextToken() != TokenType.Assignment)
            {
                throw new ParsingException($"Expecting assignment of {context}");
            }
            lexer.AdvanceOneCharacter();
            T result = readValue(lexer);
            if (lexer.DetermineNextToken() != TokenType.EndOfAssignment)
            {
                throw new ParsingException($"Missing end of assignment of {context}");
            }
            lexer.AdvanceOneCharacter();
            return result;
        }
    }
}