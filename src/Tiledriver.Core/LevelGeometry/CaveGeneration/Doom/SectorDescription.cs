// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record SectorDescription(int HeightLevel) : IComparable<SectorDescription>
{
	public static readonly SectorDescription OutsideLevel = new(-1);
	public bool IsOutsideLevel => this == OutsideLevel;

	/// <summary>
	/// "Smaller" descriptions should be the front a line
	/// </summary>
	/// <returns>-1 if this instance is "more front" than <paramref name="other"/>; otherwise 1</returns>
	public int CompareTo(SectorDescription? other)
	{
		if (IsOutsideLevel)
			return 1;

		if (other?.IsOutsideLevel ?? false)
			return -1;

		return HeightLevel < (other?.HeightLevel ?? -1) ? -1 : 1;
	}
}
