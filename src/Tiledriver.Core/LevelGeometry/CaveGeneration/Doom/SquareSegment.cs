// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class SquareSegment
{
    public static SquareSegments Create(bool topLeft, bool topRight, bool bottomLeft, bool bottomRight) =>
        (topLeft ? SquareSegments.Corner_UpperLeft : SquareSegments.None) |
        (bottomLeft ? SquareSegments.Corner_LowerLeft : SquareSegments.None) |
        (topRight ? SquareSegments.Corner_UpperRight : SquareSegments.None) |
        (bottomRight ? SquareSegments.Corner_LowerRight : SquareSegments.None);

    public static SquareSegments Create(Position p, Func<Position, bool> on) =>
        Create(
            topLeft: on(p),
            topRight: on(p.Right()),
            bottomLeft: on(p.Below()),
            bottomRight: on(p.BelowRight())
        );
}
