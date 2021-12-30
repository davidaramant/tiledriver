// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record VertexDescription(
    Position Square,
    SquarePoint Point)
{
    public VertexDescription Normalize() =>
        Point switch
        {
            SquarePoint.RightMiddle => new(Square.Right(), SquarePoint.LeftMiddle),
            SquarePoint.BottomMiddle => new(Square.Below(), SquarePoint.TopMiddle),
            _ => new(Square, Point)
        };


}