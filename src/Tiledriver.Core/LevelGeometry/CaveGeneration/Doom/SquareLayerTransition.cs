// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.LevelGeometry.Extensions;
using static Tiledriver.Core.Utils.MathUtil;
namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class SquareLayerTransition
{
    public static Func<SquareSegment, int> GetHeightLookup(
        IReadOnlyDictionary<Position, int> interiorDistances,
        Position position)
    {
        var upperLeft = interiorDistances.GetValueOr(position, -1);
        var upperRight = interiorDistances.GetValueOr(position.Right(), -1);
        var lowerLeft = interiorDistances.GetValueOr(position.Below(), -1);
        var lowerRight = interiorDistances.GetValueOr(position.BelowRight(), -1);

        // Because this is operating on internal distances from walls, there are only 1 or 2 different values for the
        // corners.
        var minHeight = Min(upperLeft, upperRight, lowerLeft, lowerRight);
        var maxHeight = Max(upperLeft, upperRight, lowerLeft, lowerRight);

        var segments = Corner.Create(
            topLeft: upperLeft == maxHeight,
            topRight: upperRight == maxHeight,
            bottomLeft: lowerLeft == maxHeight,
            bottomRight: lowerRight == maxHeight).ToSquareSegments();

        return seg => segments.HasFlag(seg.ToSquareSegments()) ? maxHeight : minHeight;
    }
}
