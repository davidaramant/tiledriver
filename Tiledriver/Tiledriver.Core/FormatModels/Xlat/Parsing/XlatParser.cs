// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Xlat.Parsing.Syntax;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing
{
    public static class XlatParser
    {
        public static MapTranslator Parse(IEnumerable<Expression> xlatExpressions)
        {
            var tileMappings = new TileMappings();
            var thingMappings = new ThingMappings();
            var flatMappings = new FlatMappings();
            var enableLightLevels = false;

            foreach (var exp in xlatExpressions)
            {
                var name = exp.Name.OrElse(() => new ParsingException("Found global expression without name."));

                switch (name.ToString())
                {
                    case "enable":
                        if (exp.Oldnum.HasValue || exp.HasAssignments || exp.Values.Any() || exp.Qualifiers.Count() != 1 || exp.SubExpressions.Any())
                        {
                            throw new ParsingException("Invalid structure of 'enable' command.");
                        }
                        var flag = exp.Qualifiers.First();
                        if (flag != new Identifier("lightlevels"))
                        {
                            throw new ParsingException($"Attempted to enable unknown flag '{flag}'");
                        }
                        enableLightLevels = true;
                        break;

                    case "tiles":
                        ValidateSectionBlock(exp, "tiles");
                        var parsedTiles = ParseTiles(exp.SubExpressions);
                        tileMappings = parsedTiles;
                        // TODO: merge with old
                        break;

                    case "things":
                        ValidateSectionBlock(exp, "things");
                        var parsedThings = ParseThings(exp.SubExpressions);
                        thingMappings = parsedThings;
                        // TODO: merge with old
                        break;

                    case "flats":
                        ValidateSectionBlock(exp, "flats");
                        var parsedFlats = ParseFlats(exp.SubExpressions);
                        flatMappings = parsedFlats;
                        // TODO: merge with old
                        break;

                    default:
                        throw new ParsingException("Unknown identifier in XLAT global scope: " + name);
                }
            }

            return new MapTranslator(tileMappings, thingMappings, flatMappings, enableLightLevels);
        }

        private static void ValidateSectionBlock(Expression section, string name)
        {
            if (section.Oldnum.HasValue || section.HasAssignments || section.Values.Any() || section.Qualifiers.Any())
            {
                throw new ParsingException($"Invalid structure of '{name}' section.");
            }
        }

        private static TileMappings ParseTiles(IEnumerable<Expression> expressions)
        {
            throw new NotImplementedException();
        }

        private static ThingMappings ParseThings(IEnumerable<Expression> expressions)
        {
            throw new NotImplementedException();
        }

        private static FlatMappings ParseFlats(IEnumerable<Expression> expressions)
        {
            var exps = expressions.ToArray();
            if (exps.Length > 2)
            {
                throw new ParsingException("Too many definitions inside of 'flats'");
            }

            var flatMappings = new FlatMappings();
            foreach (var exp in exps)
            {
                var name = exp.Name.Select(id => id.ToString()).OrElse(() => new ParsingException("Found 'flats' expression without name."));
                if (name != "ceiling" && name != "floor")
                {
                    throw new ParsingException($"Unknown expression '{name}' inside of flats.");
                }
                
                if (exp.Oldnum.HasValue || exp.HasAssignments || exp.Qualifiers.Any() || exp.SubExpressions.Any())
                {
                    throw new ParsingException($"Invalid structure of '{name}' expression in 'flats' command.");
                }

                if (name == "ceiling")
                {
                    if (flatMappings.Ceiling.Any())
                    {
                        throw new ParsingException("Duplicate 'ceiling' expression in flats.");
                    }
                    flatMappings.Ceiling.AddRange(exp.Values.Select(t=>t.TryAsString().Value));
                }
                else
                {
                    if (flatMappings.Floor.Any())
                    {
                        throw new ParsingException("Duplicate 'floor' expression in flats.");
                    }
                    flatMappings.Floor.AddRange(exp.Values.Select(t => t.TryAsString().Value));
                }
            }
            return flatMappings;
        }
    }
}