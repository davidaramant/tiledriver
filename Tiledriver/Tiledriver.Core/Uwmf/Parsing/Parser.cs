// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Core.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(ILexerOld lexerOld)
        {
            return ParseMap(lexerOld);
        }

        #region PlaneMap/TileSpace parsing

        private static PlaneMap ParsePlaneMap(ILexerOld lexerOld)
        {
            var planeMap = new PlaneMap();

            TokenTypeOld nextToken;
            nextToken = lexerOld.DetermineNextToken();
            if (nextToken != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException($"Expecting start of block when parsing PlaneMap but found {nextToken}.");
            }
            lexerOld.AdvanceOneCharacter();

            while ((nextToken = lexerOld.DetermineNextToken()) != TokenTypeOld.EndBlock)
            {
                if (nextToken == TokenTypeOld.StartBlock)
                {
                    planeMap.TileSpaces.Add(ParseTileSpace(lexerOld));
                }
                else if (nextToken == TokenTypeOld.Comma)
                {
                    lexerOld.AdvanceOneCharacter();
                }
                else
                {
                    throw new UwmfParsingException($"Unexpected token in PlaneMap: {nextToken}");
                }
            }
            lexerOld.AdvanceOneCharacter();

            planeMap.CheckSemanticValidity();
            return planeMap;
        }

        private static TileSpace ParseTileSpace(ILexerOld lexerOld)
        {
            var tileSpace = new TileSpace();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.StartBlock)
            {
                throw new UwmfParsingException("Expecting start of block when parsing Sector.");
            }
            lexerOld.AdvanceOneCharacter();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.Unknown)
            {
                throw new UwmfParsingException("Expected Tile number in TileSpace");
            }
            tileSpace.Tile = lexerOld.ReadIntegerNumber();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.Comma)
            {
                throw new UwmfParsingException("Expected comma after Tile number in TileSpace");
            }
            lexerOld.AdvanceOneCharacter();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.Unknown)
            {
                throw new UwmfParsingException("Expected Sector number in TileSpace");
            }
            tileSpace.Sector = lexerOld.ReadIntegerNumber();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.Comma)
            {
                throw new UwmfParsingException("Expected comma after Sector number in TileSpace");
            }
            lexerOld.AdvanceOneCharacter();

            if (lexerOld.DetermineNextToken() != TokenTypeOld.Unknown)
            {
                throw new UwmfParsingException("Expected Zone number in TileSpace");
            }
            tileSpace.Zone = lexerOld.ReadIntegerNumber();

            var nextToken = lexerOld.DetermineNextToken();
            if (nextToken == TokenTypeOld.Comma)
            {
                lexerOld.AdvanceOneCharacter();
                tileSpace.Tag = lexerOld.ReadIntegerNumber();
                nextToken = lexerOld.DetermineNextToken();
            }

            if (nextToken != TokenTypeOld.EndBlock)
            {
                throw new UwmfParsingException("Unexpected token in TileSpace");
            }
            lexerOld.AdvanceOneCharacter();

            tileSpace.CheckSemanticValidity();
            return tileSpace;
        }

        #endregion PlaneMap/TileSpace parsing

        #region Assignment Parsing Methods

        private static int ParseIntegerNumberAssignment(ILexerOld lexerOld, string context)
        {
            return ParseAssignment(lexerOld, l => l.ReadIntegerNumber(), context);
        }

        private static double ParseFloatingPointNumberAssignment(ILexerOld lexerOld, string context)
        {
            return ParseAssignment(lexerOld, l => l.ReadFloatingPointNumber(), context);
        }

        private static bool ParseBooleanAssignment(ILexerOld lexerOld, string context)
        {
            return ParseAssignment(lexerOld, l => l.ReadBoolean(), context);
        }

        private static string ParseStringAssignment(ILexerOld lexerOld, string context)
        {
            return ParseAssignment(lexerOld, l => l.ReadString(), context);
        }

        private static T ParseAssignment<T>(ILexerOld lexerOld, Func<ILexerOld, T> readValue, string context)
        {
            if (lexerOld.DetermineNextToken() != TokenTypeOld.Assignment)
            {
                throw new UwmfParsingException($"Expecting assignment of {context}");
            }
            lexerOld.AdvanceOneCharacter();
            T result = readValue(lexerOld);
            if (lexerOld.DetermineNextToken() != TokenTypeOld.EndOfAssignment)
            {
                throw new UwmfParsingException($"Missing end of assignment of {context}");
            }
            lexerOld.AdvanceOneCharacter();
            return result;
        }

        #endregion Assignment Parsing Methods
    }
}