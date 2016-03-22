// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(ILexer lexer)
        {
            return ParseMap(lexer);
        }

        /// <remarks>
        /// A TileSpace is a comma-separated list of numbers, unlike every other block.
        /// </remarks>
        private static TileSpace ParseTileSpace(ILexer lexer)
        {
            var tileSpace = new TileSpace();

            if (lexer.DetermineNextToken() != TokenType.StartBlock)
            {
                throw new ParsingException("Expecting start of block when parsing Sector.");
            }
            lexer.AdvanceOneCharacter();

            if (lexer.DetermineNextToken() != TokenType.Unknown)
            {
                throw new ParsingException("Expected Tile number in TileSpace");
            }
            tileSpace.Tile = lexer.ReadIntegerNumber();

            if (lexer.DetermineNextToken() != TokenType.Unknown)
            {
                throw new ParsingException("Expected Sector number in TileSpace");
            }
            tileSpace.Sector = lexer.ReadIntegerNumber();

            if (lexer.DetermineNextToken() != TokenType.Unknown)
            {
                throw new ParsingException("Expected Zone number in TileSpace");
            }
            tileSpace.Zone = lexer.ReadIntegerNumber();

            var nextToken = lexer.DetermineNextToken();
            if (nextToken == TokenType.Comma)
            {
                tileSpace.Tag = lexer.ReadIntegerNumber();
                nextToken = lexer.DetermineNextToken();
            }

            if (nextToken != TokenType.EndBlock)
            {
                throw  new ParsingException("Unexpected token in TileSpace");
            }
            lexer.AdvanceOneCharacter();

            tileSpace.CheckSemanticValidity();
            return tileSpace;
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