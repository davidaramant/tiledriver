// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.Utils.CellularAutomata
{
    public static class CellBoardExtensions
    {
        // Rules taken from http://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
        public static CellBoard GenerateStandardCave(this CellBoard board) =>
            board
                .RunGenerations(4, CellRule.FiveNeighborsOrInEmptyArea)
                .RunGenerations(3, CellRule.FiveOrMoreNeighbors);

        public static CellBoard ScaleAndSmooth(this CellBoard board) => board.Quadruple().RunGenerations(1);
    }
}