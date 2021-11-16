// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Linq;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public static class CellBoardExtensions
    {
        // Rules taken from http://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
        public static CellBoard GenerateStandardCave(this CellBoard board) =>
            board
                .RunGenerations(4, CellRule.FiveNeighborsOrInEmptyArea)
                .RunGenerations(3, CellRule.FiveOrMoreNeighbors);

        public static CellBoard ScaleAndSmooth(this CellBoard board, int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                board = board.Quadruple().RunGenerations(1);
            }

            return board;
        }

        public static CellBoard TrimToLargestDeadArea(this CellBoard board)
        {
            var (largestArea, newSize) =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(board.Dimensions, p => board[p] == CellType.Dead)
                    .MaxBy(component => component.Area)
                    ?.TrimExcess(1) ??
                throw new InvalidOperationException("This can't happen");

            return new CellBoard(newSize,
                typeAtPosition: p => largestArea.Contains(p) ? CellType.Dead : CellType.Alive);
        }
    }
}