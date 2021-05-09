// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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
                        break;
                    case IdentifierToken defaultMapId when defaultMapId.Id == "defaultMap":
                        var assignmentLookup = GetAssignmentLookup(tokenStream);
                        defaultMap = ParseDefaultMap(assignmentLookup);
                        break;
                    case IdentifierToken addDefaultMapId when addDefaultMapId.Id == "addDefaultMap":
                        break;
                }
            }

            throw new NotImplementedException();
        }

        private static partial DefaultMap ParseDefaultMap(ILookup<IdentifierToken, VariableAssignment> assignmentLookup);
        private static partial AddDefaultMap ParseAddDefaultMap(ILookup<IdentifierToken, VariableAssignment> assignmentLookup);

        private static ILookup<IdentifierToken, VariableAssignment> GetAssignmentLookup(IEnumerator<Token> tokenStream)
        {
            var assignments = ParseBlock(tokenStream);
            return assignments.ToLookup(a => a.Id, a => a);
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

        private static ImmutableList<string> ReadListAssignment(
            ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }

        private static ImmutableList<SpecialAction> ReadSpecialActionAssignments(ILookup<IdentifierToken, VariableAssignment> assignmentLookup)
        {
            throw new NotImplementedException();
        }

        private static string? ReadStringAssignment(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }

        private static int? ReadIntAssignment(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }
        
        private static bool? ReadBoolAssignment(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }

        private static bool? ReadFlag(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }

        private static ExitFadeInfo? ReadExitFadeInfoAssignment(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }
        
        private static NextMapInfo? ReadNextMapInfoAssignment(ILookup<IdentifierToken, VariableAssignment> assignmentLookup, string formatName)
        {
            throw new NotImplementedException();
        }
    }
}