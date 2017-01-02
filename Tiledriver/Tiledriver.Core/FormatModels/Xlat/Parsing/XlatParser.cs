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
                        var flag = exp.Qualifiers.First().TryAsIdentifier().OrElse(() => new ParsingException("Invalid structure of 'enable' command"));
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

        #region Tiles

        private static TileMappings ParseTiles(IEnumerable<Expression> expressions)
        {
            var tileMappings = new TileMappings();
            foreach (var exp in expressions)
            {
                var name = exp.Name.OrElse(() => new ParsingException("Found expression without a name in 'tiles' section."));

                switch (name.ToString())
                {
                    case "modzone":
                        ParseModZone(exp, tileMappings);
                        break;

                    case "tile":
                        ParseTile(exp, tileMappings);
                        break;

                    case "trigger":
                        ParseTrigger(exp, tileMappings);
                        break;

                    case "zone":
                        ParseSoundZone(exp, tileMappings);
                        break;

                    default:
                        throw new ParsingException($"Unknown expression '{name}' in tiles section.");
                }
            }
            return tileMappings;
        }

        private static void ParseModZone(Expression exp, TileMappings tileMappings)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in modzone definition."));

            var qualifierQueue = new Queue<Token>(exp.Qualifiers);

            if (!qualifierQueue.Any())
            {
                throw new ParsingException("Invalid structure of modzone.");
            }

            bool fillzone = false;

            var qualifier =
                qualifierQueue.Dequeue()
                    .TryAsIdentifier()
                    .OrElse(() => new ParsingException("Unknown qualifier for modzone."));

            if (qualifier.ToString() == "fillzone")
            {
                fillzone = true;
                if (!qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of modzone.");
                }
                qualifier =
                    qualifierQueue.Dequeue()
                        .TryAsIdentifier()
                        .OrElse(() => new ParsingException("Unknown qualifier for modzone."));
            }

            if (qualifier.ToString() == "ambush")
            {
                if (exp.HasAssignments || exp.Values.Any() || exp.SubExpressions.Any() || qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of ambush modzone.");
                }
                tileMappings.AmbushModzones.Add(oldnum, new AmbushModzone(fillzone: fillzone));
            }
            else if (qualifier.ToString() == "changetrigger")
            {
                if (!qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of modzone.");
                }
                var action =
                    qualifierQueue.Dequeue()
                        .TryAsString()
                        .OrElse(() => new ParsingException("Invalid structure for modzone."));
                if (exp.Values.Any() || exp.SubExpressions.Any() || qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of changetrigger modzone.");
                }

                var trigger = ParsePositionlessTrigger(exp);
                tileMappings.ChangeTriggerModzones.Add(oldnum, new ChangeTriggerModzone(action, trigger, fillzone));
            }
            else
            {
                throw new ParsingException($"Unknown qualifier '{qualifier}' in modzone.");
            }
        }

        private static void ParseTile(Expression exp, TileMappings tileMappings)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in tile definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for tile.");
            }
            var zone = Uwmf.Parsing.Parser.ParseTile(exp);

            tileMappings.Tiles.Add(oldnum, zone);
        }

        private static void ParseTrigger(Expression exp, TileMappings tileMappings)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in trigger definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for trigger.");
            }
            var trigger = ParsePositionlessTrigger(exp);

            tileMappings.PositionlessTriggers.Add(oldnum, trigger);
        }

        public static PositionlessTrigger ParsePositionlessTrigger(IHaveAssignments block)
        {
            // HACK: This is copy-pasted from the generated UWMF parser code with the x/y/z stuff removed
            var parsedBlock = new PositionlessTrigger();
            block.GetValueFor("Action").SetRequiredString(value => parsedBlock.Action = value, "Trigger", "Action");
            block.GetValueFor("Arg0").SetOptionalIntegerNumber(value => parsedBlock.Arg0 = value, "Trigger", "Arg0");
            block.GetValueFor("Arg1").SetOptionalIntegerNumber(value => parsedBlock.Arg1 = value, "Trigger", "Arg1");
            block.GetValueFor("Arg2").SetOptionalIntegerNumber(value => parsedBlock.Arg2 = value, "Trigger", "Arg2");
            block.GetValueFor("Arg3").SetOptionalIntegerNumber(value => parsedBlock.Arg3 = value, "Trigger", "Arg3");
            block.GetValueFor("Arg4").SetOptionalIntegerNumber(value => parsedBlock.Arg4 = value, "Trigger", "Arg4");
            block.GetValueFor("ActivateEast").SetOptionalBoolean(value => parsedBlock.ActivateEast = value, "Trigger", "ActivateEast");
            block.GetValueFor("ActivateNorth").SetOptionalBoolean(value => parsedBlock.ActivateNorth = value, "Trigger", "ActivateNorth");
            block.GetValueFor("ActivateWest").SetOptionalBoolean(value => parsedBlock.ActivateWest = value, "Trigger", "ActivateWest");
            block.GetValueFor("ActivateSouth").SetOptionalBoolean(value => parsedBlock.ActivateSouth = value, "Trigger", "ActivateSouth");
            block.GetValueFor("PlayerCross").SetOptionalBoolean(value => parsedBlock.PlayerCross = value, "Trigger", "PlayerCross");
            block.GetValueFor("PlayerUse").SetOptionalBoolean(value => parsedBlock.PlayerUse = value, "Trigger", "PlayerUse");
            block.GetValueFor("MonsterUse").SetOptionalBoolean(value => parsedBlock.MonsterUse = value, "Trigger", "MonsterUse");
            block.GetValueFor("Repeatable").SetOptionalBoolean(value => parsedBlock.Repeatable = value, "Trigger", "Repeatable");
            block.GetValueFor("Secret").SetOptionalBoolean(value => parsedBlock.Secret = value, "Trigger", "Secret");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Trigger", "Comment");
            return parsedBlock;
        }

        private static void ParseSoundZone(Expression exp, TileMappings tileMappings)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in sound zone definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for sound zone.");
            }
            var zone = Uwmf.Parsing.Parser.ParseZone(exp);

            tileMappings.Zones.Add(oldnum, zone);
        }

        #endregion Tiles

        private static ThingMappings ParseThings(IEnumerable<Expression> expressions)
        {
            throw new NotImplementedException();

            var thingMappings = new ThingMappings();
            foreach (var exp in expressions)
            {

            }
            return thingMappings;
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
                    flatMappings.Ceiling.AddRange(exp.Values.Select(t => t.TryAsString().Value));
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