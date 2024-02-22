// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

/// <summary>
/// Specifies a single segment.
/// </summary>
/// <remarks>
/// +-----+
/// |0/|\2|
/// |/1|3\|
/// +-----+
/// |\7|5/|
/// |6\|/4|
/// +-----+
/// </remarks>
public enum SquareSegment
{
	UpperLeftOuter = 0,
	UpperLeftInner = 1,

	UpperRightOuter = 2,
	UpperRightInner = 3,

	LowerRightOuter = 4,
	LowerRightInner = 5,

	LowerLeftOuter = 6,
	LowerLeftInner = 7,
}
