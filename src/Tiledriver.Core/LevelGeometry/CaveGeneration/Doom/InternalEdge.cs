// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record InternalEdge(InternalEdgeId Id, EdgeInfo Sides)
{
    public bool IsSingleSided => Sides.Back.IsOutsideLevel;

    public (Position, InternalEdge) FollowLine(bool goRight, Position square) =>
        (Id switch
            {
                InternalEdgeId.DiagTopLeft => Sides.IsFrontTopOrLeft == goRight ? square.Left() : square.Above(),
                InternalEdgeId.DiagTopRight => Sides.IsFrontTopOrLeft == goRight ? square.Above() : square.Right(),
                InternalEdgeId.DiagBottomRight => Sides.IsFrontTopOrLeft == goRight ? square.Below() : square.Right(),
                InternalEdgeId.DiagBottomLeft => Sides.IsFrontTopOrLeft == goRight ? square.Left() : square.Below(),
                InternalEdgeId.HorizontalLeft => Sides.IsFrontTopOrLeft == goRight ? square.Left() : square,
                InternalEdgeId.HorizontalRight => Sides.IsFrontTopOrLeft == goRight ? square : square.Right(),
                InternalEdgeId.VerticalTop => Sides.IsFrontTopOrLeft == goRight ? square : square.Above(),
                InternalEdgeId.VerticalBottom => Sides.IsFrontTopOrLeft == goRight ? square.Below() : square,
                _ => throw new System.Exception("Impossible")
            }, 
        new InternalEdge(Id.FollowEdge(), Sides));
}
