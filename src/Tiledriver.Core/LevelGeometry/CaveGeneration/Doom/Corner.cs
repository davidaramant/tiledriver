// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

static class Corner
{
    public static Corners Create(bool topLeft, bool topRight, bool bottomLeft, bool bottomRight) =>
        (topLeft ? Corners.TopLeft : Corners.None) |
        (bottomLeft ? Corners.BottomLeft : Corners.None) |
        (topRight ? Corners.TopRight : Corners.None) |
        (bottomRight ? Corners.BottomRight : Corners.None);

    public static Corners Create(Position p, Func<Position, bool> on) =>
        Create(
            topLeft: on(p),
            topRight: on(p.Right()),
            bottomLeft: on(p.Below()),
            bottomRight: on(p.BelowRight())
        );
}
