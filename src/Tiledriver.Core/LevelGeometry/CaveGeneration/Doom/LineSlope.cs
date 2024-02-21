// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public enum LineSlope
{
    Horizontal,
    Vertical,

    /// <summary>
    /// As X increases, Y increases
    /// </summary>
    IncreasingY,

    /// <summary>
    /// As X increases, Y decreases
    /// </summary>
    DecreasingY
}
