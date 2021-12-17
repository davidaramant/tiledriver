// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

/// <summary>
/// Specifies a single segment.
/// </summary>
/// <remarks>
/// +-----+
/// |1/|\3|
/// |/2|4\|
/// +-----+
/// |\8|6/|
/// |7\|/5|
/// +-----+
/// </remarks>
public enum SquareSegment
{
    UpperLeftOuter = 1,
    UpperLeftInner = 2,

    UpperRightOuter = 3,
    UpperRightInner = 4,

    LowerRightOuter = 5,
    LowerRightInner = 6,

    LowerLeftOuter = 7,
    LowerLeftInner = 8,
}
