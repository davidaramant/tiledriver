/*
** Parser.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

using System;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(ILexer lexer)
        {
            return ParseMap(lexer);
        }

        #region PlaneMap/TileSpace parsing

        private static PlaneMap ParsePlaneMap(ILexer lexer)
        {
            var planeMap = new PlaneMap();

            TokenType nextToken;
            nextToken = lexer.DetermineNextToken();
            if (nextToken != TokenType.StartBlock)
            {
                throw new ParsingException($"Expecting start of block when parsing PlaneMap but found {nextToken}.");
            }
            lexer.AdvanceOneCharacter();

            while ((nextToken = lexer.DetermineNextToken()) != TokenType.EndBlock)
            {
                if (nextToken == TokenType.StartBlock)
                {
                    planeMap.TileSpaces.Add(ParseTileSpace(lexer));
                }
                else if (nextToken == TokenType.Comma)
                {
                    lexer.AdvanceOneCharacter();
                }
                else
                {
                    throw new ParsingException($"Unexpected token in PlaneMap: {nextToken}");
                }
            }
            lexer.AdvanceOneCharacter();

            planeMap.CheckSemanticValidity();
            return planeMap;
        }

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

            if (lexer.DetermineNextToken() != TokenType.Comma)
            {
                throw new ParsingException("Expected comma after Tile number in TileSpace");
            }
            lexer.AdvanceOneCharacter();

            if (lexer.DetermineNextToken() != TokenType.Unknown)
            {
                throw new ParsingException("Expected Sector number in TileSpace");
            }
            tileSpace.Sector = lexer.ReadIntegerNumber();

            if (lexer.DetermineNextToken() != TokenType.Comma)
            {
                throw new ParsingException("Expected comma after Sector number in TileSpace");
            }
            lexer.AdvanceOneCharacter();

            if (lexer.DetermineNextToken() != TokenType.Unknown)
            {
                throw new ParsingException("Expected Zone number in TileSpace");
            }
            tileSpace.Zone = lexer.ReadIntegerNumber();

            var nextToken = lexer.DetermineNextToken();
            if (nextToken == TokenType.Comma)
            {
                lexer.AdvanceOneCharacter();
                tileSpace.Tag = lexer.ReadIntegerNumber();
                nextToken = lexer.DetermineNextToken();
            }

            if (nextToken != TokenType.EndBlock)
            {
                throw new ParsingException("Unexpected token in TileSpace");
            }
            lexer.AdvanceOneCharacter();

            tileSpace.CheckSemanticValidity();
            return tileSpace;
        }

        #endregion PlaneMap/TileSpace parsing

        #region Assignment Parsing Methods

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

        #endregion Assignment Parsing Methods
    }
}