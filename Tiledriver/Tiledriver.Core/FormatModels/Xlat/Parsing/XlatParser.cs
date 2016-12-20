// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Piglet.Parser;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing
{
    public static class XlatParser
    {
        public static MapTranslator Parse(ILexer lexer)
        {
            var tileMappings = new TileMappings();
            var thingMappings = new ThingMappings();
            var flatMappings = new FlatMappings();
            var enableLightLevels = false;

            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.EndOfFile);

                if (idToken.Type == TokenType.EndOfFile) break;

                var name = idToken.TryAsIdentifier().Value;

                switch (name.ToString())
                {
                    case "enable":
                        var lightLevelsId = lexer.MustReadIdentifier();
                        if (lightLevelsId.ToString() != "lightlevels")
                        {
                            throw new ParseException("Invalid syntax: Expected 'lightlevels'");
                        }
                        lexer.MustReadTokenOfTypes(TokenType.Semicolon);
                        enableLightLevels = true;
                        break;

                    case "include":
                        throw new NotImplementedException("inlude statement");
                        
                    case "tiles":
                        ParseTiles(tileMappings,lexer);
                        break;
                    case "things":
                        ParseThings(thingMappings, lexer);
                        break;
                    case "flats":
                        ParseFlats(flatMappings, lexer);
                        break;

                    default:
                        throw new ParseException("Unknown identifier in XLAT global scope: " + name);
                }
            }

            return new MapTranslator(tileMappings, thingMappings, flatMappings, enableLightLevels);
        }

        #region Tiles

        private static void ParseTiles(TileMappings tileMappings, ILexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.OpenParen);

            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (idToken.Type == TokenType.CloseParen) break;

                var name = idToken.TryAsIdentifier().Value;

                var oldNum = lexer.MustReadUshort();

                switch (name.ToString())
                {
                    case "modzone":
                    case "tile":
                    case "trigger":
                    case "zone":
                    default:
                        throw new ParseException("Unknown identifier in XLAT tiles: " + name);
                }
            }
        }

        #endregion

        private static void ParseThings(ThingMappings thingMappings, ILexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.OpenParen);

            while (true)
            {
                // TODO: Handle OpenParen too - thing definitions!!
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (idToken.Type == TokenType.CloseParen) break;

                var name = idToken.TryAsIdentifier().Value;

                switch (name.ToString())
                {
                    default:
                        throw new ParseException("Unknown identifier in XLAT things: " + name);
                }
            }
        }

        private static void ParseFlats(FlatMappings flatMappings, ILexer lexer)
        {
            lexer.MustReadTokenOfTypes(TokenType.OpenParen);

            while (true)
            {
                var idToken = lexer.MustReadTokenOfTypes(TokenType.Identifier, TokenType.CloseParen);

                if (idToken.Type == TokenType.CloseParen) break;

                var name = idToken.TryAsIdentifier().Value;

                switch (name.ToString())
                {
                    default:
                        throw new ParseException("Unknown identifier in XLAT flats:" + name);
                }
            }
        }
    }
}