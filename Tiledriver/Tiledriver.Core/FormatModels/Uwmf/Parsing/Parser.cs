// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Parsing.Syntax;

namespace Tiledriver.Core.FormatModels.Uwmf.Parsing
{
    public static partial class Parser
    {
        public static Map Parse(UwmfSyntaxTree syntaxTree)
        {
            var map = new Map();

            SetGlobalAssignments(map, syntaxTree);
            SetBlocks(map, syntaxTree);
            map.PlaneMaps.AddRange(syntaxTree.ArrayBlocks.Select(ParsePlaneMap));

            map.CheckSemanticValidity();

            return map;
        }

        static partial void SetGlobalAssignments(Map map, UwmfSyntaxTree tree);
        static partial void SetBlocks(Map map, UwmfSyntaxTree tree);

        private static PlaneMap ParsePlaneMap(ArrayBlock arrayBlock)
        {
            return new PlaneMap(
                arrayBlock.Select(tuple =>
                {
                    if (tuple.Count < 3 || tuple.Count > 4)
                    {
                        throw new ParsingException("Invalid number of entries inside a tilespace.");
                    }

                    return new TileSpace(
                        tile: tuple[0],
                        sector: tuple[1],
                        zone: tuple[2],
                        tag: tuple.ElementAtOrDefault(3));
                }));
        }
    }
}