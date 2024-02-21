// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;
using static Tiledriver.Core.Utils.MathUtil;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class SquareLayerTransition
{
    private static readonly IPositionOffsets PositionOffset = CoordinateSystem.BottomLeft;

    public static Func<SquareSegment, int> GetHeightLookup(
        IReadOnlyDictionary<Position, int> interiorDistances,
        Position bottomLeft
    )
    {
        static int Normalize(int distance) =>
            distance switch
            {
                -1 => -1,
                _ => distance / 2
            };

        var upperLeft = Normalize(interiorDistances.GetValueOr(bottomLeft + PositionOffset.Up, -1));
        var upperRight = Normalize(interiorDistances.GetValueOr(bottomLeft + PositionOffset.UpAndRight, -1));
        var lowerLeft = Normalize(interiorDistances.GetValueOr(bottomLeft, -1));
        var lowerRight = Normalize(interiorDistances.GetValueOr(bottomLeft + PositionOffset.Right, -1));

        // Because this is operating on internal distances from walls, there are only 1 or 2 different values for the
        // corners.
        var minHeight = Min(upperLeft, upperRight, lowerLeft, lowerRight);
        var maxHeight = Max(upperLeft, upperRight, lowerLeft, lowerRight);

        var segments = Corner
            .Create(
                topLeft: upperLeft == maxHeight,
                topRight: upperRight == maxHeight,
                bottomLeft: lowerLeft == maxHeight,
                bottomRight: lowerRight == maxHeight
            )
            .ToSquareSegments();

        return seg => segments.HasFlag(seg.ToSquareSegments()) ? maxHeight : minHeight;
    }
}
