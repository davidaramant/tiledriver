// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.Xlat.Parsing
{
    public static class XlatParser
    {
        public static MapTranslatorInfo Parse(IEnumerable<Expression> xlatExpressions)
        {
            var tileMappings = new TileMappings();
            var thingMappings = new List<IThingMapping>();
            var flatMappings = new FlatMappings();
            var enableLightLevels = false;

            foreach (var exp in xlatExpressions)
            {
                var name = exp.Name.OrElse(() => new ParsingException("Found global expression without name."));

                switch (name.ToLower())
                {
                    // TODO: Is there a 'disable' as well?  (yes, there is)
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
                        tileMappings.Add(parsedTiles);
                        break;

                    case "things":
                        ValidateSectionBlock(exp, "things");
                        var parsedThings = ParseThings(exp.SubExpressions);
                        thingMappings.AddRange(parsedThings);
                        break;

                    case "flats":
                        ValidateSectionBlock(exp, "flats");
                        var parsedFlats = ParseFlats(exp.SubExpressions);
                        flatMappings.Add(parsedFlats);
                        break;

                    default:
                        throw new ParsingException("Unknown identifier in XLAT global scope: " + name);
                }
            }

            return new MapTranslatorInfo(tileMappings, thingMappings, flatMappings, enableLightLevels);
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

                switch (name.ToLower())
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
            var oldNum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in modzone definition."));

            var qualifierQueue = new Queue<Token>(exp.Qualifiers);

            if (!qualifierQueue.Any())
            {
                throw new ParsingException("Invalid structure of modzone.");
            }

            bool fillzone = false;

            var qualifier = qualifierQueue.DequeueOfType(TokenType.Identifier).AsIdentifier();

            if (qualifier.ToLower() == "fillzone")
            {
                fillzone = true;

                qualifier = qualifierQueue.DequeueOfType(TokenType.Identifier).AsIdentifier(); ;
            }

            if (qualifier.ToLower() == "ambush")
            {
                if (exp.HasAssignments || exp.Values.Any() || exp.SubExpressions.Any() || qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of ambush modzone.");
                }
                tileMappings.AmbushModzones.Add(new AmbushModzone(oldNum: oldNum, fillzone: fillzone));
            }
            else if (qualifier.ToLower() == "changetrigger")
            {
                var action = qualifierQueue.DequeueOfType(TokenType.String).TryAsString().Value;
                if (exp.Values.Any() || exp.SubExpressions.Any() || qualifierQueue.Any())
                {
                    throw new ParsingException("Invalid structure of changetrigger modzone.");
                }

                var trigger = ParsePositionlessTrigger(exp);
                tileMappings.ChangeTriggerModzones.Add(new ChangeTriggerModzone(oldNum, action, trigger, fillzone));
            }
            else
            {
                throw new ParsingException($"Unknown qualifier '{qualifier}' in modzone.");
            }
        }

        private static void ParseTile(Expression exp, TileMappings tileMappings)
        {
            var oldNum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in tile definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for tile.");
            }
            var tile = ParseTile(exp);
            tile.OldNum = oldNum;

            tileMappings.TileTemplates.Add(tile);
        }

        public static TileTemplate ParseTile(IHaveAssignments block)
        {
            // TODO: Copying these methods is moronic
            var parsedBlock = new TileTemplate();
            block.GetValueFor("TextureEast").SetRequiredString(value => parsedBlock.TextureEast = value, "Tile", "TextureEast");
            block.GetValueFor("TextureNorth").SetRequiredString(value => parsedBlock.TextureNorth = value, "Tile", "TextureNorth");
            block.GetValueFor("TextureWest").SetRequiredString(value => parsedBlock.TextureWest = value, "Tile", "TextureWest");
            block.GetValueFor("TextureSouth").SetRequiredString(value => parsedBlock.TextureSouth = value, "Tile", "TextureSouth");
            block.GetValueFor("BlockingEast").SetOptionalBoolean(value => parsedBlock.BlockingEast = value, "Tile", "BlockingEast");
            block.GetValueFor("BlockingNorth").SetOptionalBoolean(value => parsedBlock.BlockingNorth = value, "Tile", "BlockingNorth");
            block.GetValueFor("BlockingWest").SetOptionalBoolean(value => parsedBlock.BlockingWest = value, "Tile", "BlockingWest");
            block.GetValueFor("BlockingSouth").SetOptionalBoolean(value => parsedBlock.BlockingSouth = value, "Tile", "BlockingSouth");
            block.GetValueFor("OffsetVertical").SetOptionalBoolean(value => parsedBlock.OffsetVertical = value, "Tile", "OffsetVertical");
            block.GetValueFor("OffsetHorizontal").SetOptionalBoolean(value => parsedBlock.OffsetHorizontal = value, "Tile", "OffsetHorizontal");
            block.GetValueFor("DontOverlay").SetOptionalBoolean(value => parsedBlock.DontOverlay = value, "Tile", "DontOverlay");
            block.GetValueFor("Mapped").SetOptionalInteger(value => parsedBlock.Mapped = value, "Tile", "Mapped");
            block.GetValueFor("SoundSequence").SetOptionalString(value => parsedBlock.SoundSequence = value, "Tile", "SoundSequence");
            block.GetValueFor("TextureOverhead").SetOptionalString(value => parsedBlock.TextureOverhead = value, "Tile", "TextureOverhead");
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Tile", "Comment");
            return parsedBlock;
        }

        private static void ParseTrigger(Expression exp, TileMappings tileMappings)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in trigger definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for trigger.");
            }
            var trigger = ParsePositionlessTrigger(exp);
            trigger.OldNum = oldnum;

            tileMappings.TriggerTemplates.Add(trigger);
        }

        private static TriggerTemplate ParsePositionlessTrigger(IHaveAssignments block)
        {
            // HACK: This is copy-pasted from the generated UWMF parser code with the x/y/z stuff removed
            var parsedBlock = new TriggerTemplate();
            block.GetValueFor("Action").SetRequiredString(value => parsedBlock.Action = value, "Trigger", "Action");
            block.GetValueFor("Arg0").SetOptionalInteger(value => parsedBlock.Arg0 = value, "Trigger", "Arg0");
            block.GetValueFor("Arg1").SetOptionalInteger(value => parsedBlock.Arg1 = value, "Trigger", "Arg1");
            block.GetValueFor("Arg2").SetOptionalInteger(value => parsedBlock.Arg2 = value, "Trigger", "Arg2");
            block.GetValueFor("Arg3").SetOptionalInteger(value => parsedBlock.Arg3 = value, "Trigger", "Arg3");
            block.GetValueFor("Arg4").SetOptionalInteger(value => parsedBlock.Arg4 = value, "Trigger", "Arg4");
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
            var oldNum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in sound zone definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for sound zone.");
            }
            var zone = ParseZone(exp);
            zone.OldNum = oldNum;

            tileMappings.ZoneTemplates.Add(zone);
        }

        public static ZoneTemplate ParseZone(IHaveAssignments block)
        {
            // HACK: This is copy-pasted from the generated UWMF parser code
            var parsedBlock = new ZoneTemplate();
            block.GetValueFor("Comment").SetOptionalString(value => parsedBlock.Comment = value, "Zone", "Comment");
            return parsedBlock;
        }

        #endregion Tiles

        #region Things

        private static IEnumerable<IThingMapping> ParseThings(IEnumerable<Expression> expressions)
        {
            foreach (var exp in expressions)
            {
                if (!exp.Name.HasValue)
                {
                    // Must be a thing definition
                    yield return ParseThingDefinition(exp);
                }
                else
                {
                    var name = exp.Name.Value;
                    switch (name.ToLower())
                    {
                        case "elevator":
                            yield return ParseElevator(exp);
                            break;

                        case "trigger":
                            yield return ParseTrigger(exp);
                            break;

                        default:
                            throw new ParsingException($"Unknown expression '{name}' in 'things' section");
                    }
                }
            }
        }

        private static IThingMapping ParseThingDefinition(Expression exp)
        {
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || !exp.Values.Any() || exp.HasAssignments)
            {
                throw new ParsingException("Bad structure for thing definition.");
            }

            var valueQueue = new Queue<Token>(exp.Values);

            var oldnum = valueQueue.Dequeue().AsUshort().Value;
            valueQueue.DequeueOfType(TokenType.Comma);

            var token = valueQueue.DequeueOfType(TokenType.Meta, TokenType.Identifier);
            var hasMeta = false;
            if (token.Type == TokenType.Meta)
            {
                hasMeta = true;
                token = valueQueue.DequeueOfType(TokenType.Identifier);
            }
            var actor = (hasMeta ? "$" : string.Empty) + (string)token.AsIdentifier();
            valueQueue.DequeueOfType(TokenType.Comma);

            var angles = valueQueue.DequeueOfType(TokenType.Integer).AsInt();
            valueQueue.DequeueOfType(TokenType.Comma);

            var flags = ParseThingFlags(valueQueue);

            var minskill = valueQueue.DequeueOfType(TokenType.Integer).AsInt();

            if (valueQueue.Any())
            {
                throw new ParsingException("Unexpected additional values in thing definition.");
            }

            return
                new ThingTemplate(
                    oldNum: oldnum,
                    type: actor,
                    angles: angles,
                    holowall: flags.HasFlag(ThingFlags.Holowall),
                    pathing: flags.HasFlag(ThingFlags.Pathing),
                    ambush: flags.HasFlag(ThingFlags.Ambush),
                    minskill: minskill);
        }

        private static ThingFlags ParseThingFlags(Queue<Token> valueQueue)
        {
            var token = valueQueue.DequeueOfType(TokenType.Identifier, TokenType.Integer);

            if (token.Type == TokenType.Integer)
            {
                valueQueue.DequeueOfType(TokenType.Comma);
                return ThingFlags.None;
            }

            var foundFlags = new List<string>();

            do
            {
                foundFlags.Add(token.AsIdentifier().ToLower());

                token = valueQueue.DequeueOfType(TokenType.Pipe, TokenType.Comma);

                if (token.Type == TokenType.Pipe)
                {
                    token = valueQueue.DequeueOfType(TokenType.Identifier);
                }
                else
                {
                    break;
                }
            } while (true);

            var flags = ThingFlags.None;

            foreach (var f in foundFlags)
            {
                switch (f)
                {
                    case "ambush":
                        flags |= ThingFlags.Ambush;
                        break;

                    case "holowall":
                        flags |= ThingFlags.Holowall;
                        break;

                    case "pathing":
                        flags |= ThingFlags.Pathing;
                        break;

                    default:
                        throw new ParsingException($"Unknown flag in thing definition: {f}");
                }
            }

            return flags;
        }

        [Flags]
        enum ThingFlags
        {
            None = 0,
            Holowall = 1,
            Pathing = 2,
            Ambush = 4,
        }

        private static IThingMapping ParseElevator(Expression exp)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in elevator definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any() || exp.HasAssignments)
            {
                throw new ParsingException("Bad structure for elevator.");
            }
            return new Elevator(oldnum);
        }

        private static IThingMapping ParseTrigger(Expression exp)
        {
            var oldnum = exp.Oldnum.OrElse(() => new ParsingException("No oldnum found in trigger definition."));
            if (exp.Qualifiers.Any() || exp.SubExpressions.Any() || exp.Values.Any())
            {
                throw new ParsingException("Bad structure for trigger.");
            }
            var trigger = ParsePositionlessTrigger(exp);
            trigger.OldNum = oldnum;

            return trigger;
        }

        #endregion Things

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
                var name = exp.Name.Select(id => id.ToLower()).OrElse(() => new ParsingException("Found 'flats' expression without name."));
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
                    if (flatMappings.Ceilings.Any())
                    {
                        throw new ParsingException("Duplicate 'ceiling' expression in flats.");
                    }
                    flatMappings.Ceilings.AddRange(exp.Values.Select(t => t.TryAsString().Value));
                }
                else
                {
                    if (flatMappings.Floors.Any())
                    {
                        throw new ParsingException("Duplicate 'floor' expression in flats.");
                    }
                    flatMappings.Floors.AddRange(exp.Values.Select(t => t.TryAsString().Value));
                }
            }
            return flatMappings;
        }
    }
}