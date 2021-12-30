// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record VertexDescription(
    Position Square,
    SquarePoint Point)
{
    public static VertexDescription Normalized(Position square, SquarePoint point) =>
        point switch
        {
            SquarePoint.RightMiddle => new(square.Right(), SquarePoint.LeftMiddle),
            SquarePoint.BottomMiddle => new(square.Below(), SquarePoint.TopMiddle),
            _ => new(square, point)
        };
}