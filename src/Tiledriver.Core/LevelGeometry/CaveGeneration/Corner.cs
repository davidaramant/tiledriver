// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Tiledriver.Core.LevelGeometry.CoordinateSystems;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration;

public static class Corner
{
    public static Corners Create(bool topLeft, bool topRight, bool bottomLeft, bool bottomRight) =>
        (topLeft ? Corners.UpperLeft : Corners.None) |
        (bottomLeft ? Corners.LowerLeft : Corners.None) |
        (topRight ? Corners.UpperRight : Corners.None) |
        (bottomRight ? Corners.LowerRight : Corners.None);

    public static Corners CreateFromLowerLeft(Position lowerLeft, Func<Position, bool> on) =>
        Create(
            topLeft: on(lowerLeft + CoordinateSystem.BottomLeft.Up),
            topRight: on(lowerLeft + CoordinateSystem.BottomLeft.UpAndRight),
            bottomLeft: on(lowerLeft),
            bottomRight: on(lowerLeft + CoordinateSystem.BottomLeft.Right)
        );

    public static Corners CreateFromUpperLeft(Position upperLeft, Func<Position, bool> on) =>
        Create(
            topLeft: on(upperLeft),
            topRight: on(upperLeft + CoordinateSystem.TopLeft.Right),
            bottomLeft: on(upperLeft + CoordinateSystem.TopLeft.Down),
            bottomRight: on(upperLeft + CoordinateSystem.TopLeft.DownAndRight)
        );

    public static SquareSegments ToSquareSegments(this Corners corners) => corners switch
    {
        Corners.None => SquareSegments.None,

        Corners.LowerLeft => SquareSegments.Corner_LowerLeft,
        Corners.LowerRight => SquareSegments.Corner_LowerRight,
        Corners.UpperRight => SquareSegments.Corner_UpperRight,
        Corners.UpperLeft => SquareSegments.Corner_UpperLeft,

        Corners.Upper => SquareSegments.Corners_Upper,
        Corners.Lower => SquareSegments.Corners_Lower,
        Corners.Left => SquareSegments.Corners_Left,
        Corners.Right => SquareSegments.Corners_Right,

        Corners.AllButLowerLeft => SquareSegments.Corners_AllButLowerLeft,
        Corners.AllButLowerRight => SquareSegments.Corners_AllButLowerRight,
        Corners.AllButUpperLeft => SquareSegments.Corners_AllButUpperLeft,
        Corners.AllButUpperRight => SquareSegments.Corners_AllButUpperRight,

        Corners.UpperLeftAndLowerRight => SquareSegments.Corners_UpperLeftAndLowerRight,
        Corners.UpperRightAndLowerLeft => SquareSegments.Corners_UpperRightAndLowerLeft,

        _ => SquareSegments.All
    };
}
