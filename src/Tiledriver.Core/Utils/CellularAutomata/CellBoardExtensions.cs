// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public static class CellRule
    {
        public static bool FiveNeighborsOrInEmptyArea(Func<int, int> countAlive) => countAlive(1) >= 5 || countAlive(2) <= 2;
        public static bool FiveNeighbors(Func<int, int> countAlive) => countAlive(1) >= 5;
    }

    public static class CellBoardExtensions
    {
        // Rules taken from http://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
        public static CellBoard GenerateStandardCave(this CellBoard board) =>
            board
                .RunGenerations(4, CellRule.FiveNeighborsOrInEmptyArea)
                .RunGenerations(3, CellRule.FiveNeighbors);
    }
}