// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Utils.CellularAutomata;

public static class CellRule
{
	public delegate bool IsNextGenAlive(CellType current, Func<int, int> countAlive);

	public static bool FiveNeighborsOrInEmptyArea(CellType current, Func<int, int> countAlive) =>
		countAlive(1) >= 5 || countAlive(2) <= 2;

	public static bool FiveOrMoreNeighbors(CellType current, Func<int, int> countAlive) => countAlive(1) >= 5;

	public static bool B678_S345678(CellType current, Func<int, int> countAlive) =>
		current switch
		{
			CellType.Dead => countAlive(1) > 5,
			CellType.Alive => countAlive(1) > 2,
			_ => throw new Exception("Can't happen"),
		};
}
