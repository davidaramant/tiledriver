// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Common.Reading;
using Tiledriver.Core.FormatModels.MapInfo.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.MapInfo.Reading
{
    public static partial class MapDeclarationParser
    {
        public static IReadOnlyDictionary<string, Map> ReadMapDeclarations(IEnumerator<Token> tokenStream)
        {
            var defaultMap = new DefaultMap(EnsureInventories: ImmutableList<string>.Empty, SpecialActions: ImmutableList<SpecialAction>.Empty);
            var lumpToMap = new Dictionary<string, Map>();

            while (tokenStream.MoveNext())
            {
                switch (tokenStream.Current)
                {
                    case IdentifierToken mapId when mapId.Id == "map":
                        {
                            var header = ParseMapHeader(mapId, tokenStream);
                            var assignmentLookup = GetAssignmentLookup(tokenStream);
                            var map = ParseMap(assignmentLookup, header.mapLump, header.mapName, header.isMapNameLookup, defaultMap);
                            lumpToMap.Add(
                                header.mapLump.ToUpperInvariant(), 
                                map);
                        }
                        break;

                    case IdentifierToken defaultMapId when defaultMapId.Id == "defaultMap":
                        {
                            var assignmentLookup = GetAssignmentLookup(tokenStream);
                            defaultMap = ParseDefaultMap(assignmentLookup);
                        }
                        break;

                    case IdentifierToken addDefaultMapId when addDefaultMapId.Id == "addDefaultMap":
                        {
                            var assignmentLookup = GetAssignmentLookup(tokenStream);
                            var addDefaultMap = ParseAddDefaultMap(assignmentLookup);
                            defaultMap = UpdateDefaultMap(defaultMap, addDefaultMap);
                        }
                        break;
                }
            }

            return lumpToMap;
        }

        private static (string mapLump, string? mapName, bool isMapNameLookup) ParseMapHeader(
            IdentifierToken context,
            IEnumerator<Token> tokenStream)
        {
            var mapLump = tokenStream.ExpectNext<StringToken>().Value;
            string? mapName = null;
            bool isMapNameLookup = false;

            var next = tokenStream.GetNext();

            if (next is StringToken nameToken)
            {
                mapName = nameToken.Value;
            }
            else if (next is IdentifierToken lookupToken)
            {
                if (lookupToken.Id.ToLower() != "lookup")
                {
                    throw ParsingException.CreateError(lookupToken, "lookup");
                }

                isMapNameLookup = true;
                mapName = tokenStream.ExpectNext<StringToken>().Value;
            }
            else
            {
                throw new ParsingException("Messed up map definition: " + context);
            }

            return (mapLump, mapName, isMapNameLookup);
        }

        private static partial Map ParseMap(
            ILookup<Identifier, VariableAssignment> assignmentLookup,
            string mapLump,
            string? mapName,
            bool isMapNameLookup,
            DefaultMap defaultMap);
        private static partial DefaultMap ParseDefaultMap(ILookup<Identifier, VariableAssignment> assignmentLookup);
        private static partial AddDefaultMap ParseAddDefaultMap(ILookup<Identifier, VariableAssignment> assignmentLookup);
        private static partial DefaultMap UpdateDefaultMap(DefaultMap defaultMap, AddDefaultMap addDefaultMap);

        private static ILookup<Identifier, VariableAssignment> GetAssignmentLookup(IEnumerator<Token> tokenStream)
        {
            var assignments = ParseBlock(tokenStream);
            return assignments.ToLookup(a => a.Id.Id, a => a);
        }

        private static IEnumerable<VariableAssignment> ParseBlock(IEnumerator<Token> tokenStream)
        {
            tokenStream.ExpectNext<OpenBraceToken>();

            while (true)
            {
                switch (tokenStream.GetNextAndSkipNewlines())
                {
                    case IdentifierToken id:
                        var next = tokenStream.GetNext();
                        var valueBuilder = ImmutableList.CreateBuilder<Token>();

                        switch (next)
                        {
                            case NewLineToken:
                                yield return new VariableAssignment(id, valueBuilder.ToImmutable());
                                break;

                            case EqualsToken:
                                next = tokenStream.GetNext();
                                if (next == null || next is NewLineToken)
                                {
                                    throw new ParsingException($"Messed up line staring with {id}");
                                }
                                valueBuilder.Add(next);

                                while (next == tokenStream.GetNext())
                                {
                                    switch (next)
                                    {
                                        case NewLineToken:
                                            yield return new VariableAssignment(id, valueBuilder.ToImmutable());
                                            break;

                                        case null:
                                            throw new ParsingException("Unexpected end of file");

                                        default:
                                            valueBuilder.Add(next);
                                            break;
                                    }
                                }

                                break;

                            default:
                                throw new ParsingException($"Messed up line starting with {id}");
                        }

                        break;
                    case CloseBraceToken cbt:
                        yield break;

                    default:
                        throw new ParsingException("Error parsing MapInfo");
                }
            }
        }

        private static VariableAssignment[] GetAssignments(
            ILookup<Identifier, VariableAssignment> assignmentLookup,
            string formatName)
        {
            var id = new Identifier(formatName);
            return assignmentLookup.Contains(id) ? assignmentLookup[id].ToArray() : new VariableAssignment[0];
        }

        private static VariableAssignment? GetSingleAssignment(
            ILookup<Identifier, VariableAssignment> assignmentLookup,
            string formatName)
        {
            var assignments = GetAssignments(assignmentLookup, formatName);

            return assignments.Length switch
            {
                0 => null,
                1 => assignments[0],
                _ => throw new ParsingException($"Multiple assignments to {formatName}")
            };
        }

        private static ImmutableList<string> ReadListAssignment(
            ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName)
        {
            var assignment = GetSingleAssignment(assignmentLookup, formatName);
            if (assignment == null)
            {
                return ImmutableList<string>.Empty;
            }

            var valueQueue = new Queue<Token>(assignment.Values);

            var strings = ImmutableList.CreateBuilder<string>();

            if (valueQueue.Any())
            {
                var token = valueQueue.Dequeue();
                if (token is not StringToken st)
                {
                    throw new ParsingException("Expected string token, got " + token);
                }

                strings.Add(st.Value);

                while (valueQueue.Any())
                {
                    token = valueQueue.Dequeue();
                    if (token is not CommaToken)
                    {
                        throw ParsingException.CreateError(token, "comma");
                    }

                    token = valueQueue.Dequeue();
                    if (token is not StringToken stringToken)
                    {
                        throw ParsingException.CreateError(token, "string");
                    }

                    strings.Add(stringToken.Value);
                }
            }

            return strings.ToImmutable();
        }

        private static ImmutableList<SpecialAction> ReadSpecialActionAssignments(ILookup<Identifier, VariableAssignment> assignmentLookup)
        {
            var id = new Identifier("SpecialAction");
            var assignments = assignmentLookup.Contains(id)
                ? assignmentLookup[id]
                : Enumerable.Empty<VariableAssignment>();

            TToken MustGet<TToken>(IdentifierToken id, Queue<Token> valueQueue) where TToken : Token
            {
                if (!valueQueue.Any())
                {
                    throw new ParsingException("Messed up SpecialAction: " + id);
                }

                var token = valueQueue.Dequeue();
                if (token is not TToken t)
                {
                    throw ParsingException.CreateError(token, typeof(TToken).Name);
                }

                return t;
            }

            return assignments.Select(
                va =>
                {
                    var valueQueue = new Queue<Token>(va.Values);
                    var actorClassToken = MustGet<StringToken>(va.Id, valueQueue);
                    MustGet<CommaToken>(va.Id, valueQueue);
                    var specialToken = MustGet<StringToken>(va.Id, valueQueue);

                    var args = new int[5];
                    int i = 0;
                    while (valueQueue.Any())
                    {
                        MustGet<CommaToken>(va.Id, valueQueue);
                        var argToken = MustGet<IntegerToken>(va.Id, valueQueue);
                        args[i] = argToken.Value;

                        i++;

                        if (i == 5)
                        {
                            throw new ParsingException("Too many arguments to SpecialAction: " + id);
                        }
                    }

                    return new SpecialAction(
                        actorClassToken.Value,
                        specialToken.Value,
                        args[0],
                        args[1],
                        args[2],
                        args[3],
                        args[4]);

                }).ToImmutableList();
        }

        private static TToken? GetSingleToken<TToken>(ILookup<Identifier, VariableAssignment> assignmentLookup,
            string formatName) where TToken : Token
        {
            var assignment = GetSingleAssignment(assignmentLookup, formatName);
            if (assignment == null)
            {
                return null;
            }

            if (assignment.Values.Count != 1)
            {
                throw new ParsingException($"Messed up assignment: {assignment.Id}");
            }

            var token = assignment.Values[0];
            if (token is not TToken t)
            {
                throw ParsingException.CreateError(token, "string");
            }

            return t;
        }

        private static string? ReadStringAssignment(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName) =>
            GetSingleToken<StringToken>(assignmentLookup, formatName)?.Value;

        private static int? ReadIntAssignment(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName) =>
            GetSingleToken<IntegerToken>(assignmentLookup, formatName)?.Value;

        private static bool? ReadBoolAssignment(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName) =>
            GetSingleToken<BooleanToken>(assignmentLookup, formatName)?.Value;

        private static bool? ReadFlag(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName)
        {
            var assignment = GetSingleAssignment(assignmentLookup, formatName);
            if (assignment == null)
            {
                return null;
            }

            if (assignment.Values.Count != 0)
            {
                throw new ParsingException($"Messed up flag: {assignment.Id}");
            }

            return true;
        }

        private static ExitFadeInfo? ReadExitFadeInfoAssignment(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName)
        {
            var assignment = GetSingleAssignment(assignmentLookup, formatName);
            if (assignment == null)
            {
                return null;
            }

            if (assignment.Values.Count != 3)
            {
                throw new ParsingException($"Messed up ExitFade definition: {assignment.Id}");
            }

            if (assignment.Values[0] is not StringToken colorToken)
            {
                throw ParsingException.CreateError(assignment.Values[0], "string");
            }
            if (assignment.Values[1] is not CommaToken)
            {
                throw ParsingException.CreateError(assignment.Values[1], "comma");
            }
            if (assignment.Values[2] is not IntegerToken durationToken)
            {
                throw ParsingException.CreateError(assignment.Values[2], "integer");
            }

            return new ExitFadeInfo(colorToken.Value, durationToken.Value);
        }

        private static NextMapInfo? ReadNextMapInfoAssignment(ILookup<Identifier, VariableAssignment> assignmentLookup, string formatName)
        {
            var assignment = GetSingleAssignment(assignmentLookup, formatName);
            if (assignment == null)
            {
                return null;
            }

            switch (assignment.Values.Count)
            {
                case 1:
                    if (assignment.Values[0] is not StringToken mapNameToken)
                    {
                        throw ParsingException.CreateError(assignment.Values[0], "string");
                    }

                    return NextMapInfo.Map(mapNameToken.Value);

                case 3:
                    if (assignment.Values[0] is not IdentifierToken idToken)
                    {
                        throw ParsingException.CreateError(assignment.Values[0], "identifier");
                    }

                    if (idToken.Id.ToLower() != "endsequence")
                    {
                        throw new ParsingException($"Expected EndSequence, but got something else: {idToken}");
                    }
                    if (assignment.Values[1] is not CommaToken)
                    {
                        throw ParsingException.CreateError(assignment.Values[1], "comma");
                    }
                    if (assignment.Values[2] is not StringToken sequenceNameToken)
                    {
                        throw ParsingException.CreateError(assignment.Values[2], "string");
                    }

                    return NextMapInfo.EndSequence(sequenceNameToken.Value);

                default:
                    throw new ParsingException($"Messed up NewMapInfo: {assignment.Id}");
            }
        }
    }
}