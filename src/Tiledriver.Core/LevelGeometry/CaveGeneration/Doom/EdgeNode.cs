// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeNode(
    Position Square,
    EdgeSegment Segment)
{
    public bool IsSingleSided => Segment.Sides.Back.IsOutsideLevel;
    public VertexDescription StartPoint => new(Square, Segment.Id.GetPoint(topOrLeftIsFront: Segment.Sides.IsFrontTopOrLeft, leftSide: true));
    public VertexDescription EndPoint => new(Square, Segment.Id.GetPoint(topOrLeftIsFront: Segment.Sides.IsFrontTopOrLeft, leftSide: false));

    public EdgeNode FollowLine(bool goRight) => new(
        Segment.Id switch
        {
            EdgeSegmentId.DiagTopLeft => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Left() : Square.Above(),
            EdgeSegmentId.DiagTopRight => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Above() : Square.Right(),
            EdgeSegmentId.DiagBottomRight => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Below() : Square.Right(),
            EdgeSegmentId.DiagBottomLeft => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Left() : Square.Below(),
            EdgeSegmentId.HorizontalLeft => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Left() : Square,
            EdgeSegmentId.HorizontalRight => Segment.Sides.IsFrontTopOrLeft == goRight ? Square : Square.Right(),
            EdgeSegmentId.VerticalTop => Segment.Sides.IsFrontTopOrLeft == goRight ? Square : Square.Above(),
            EdgeSegmentId.VerticalBottom => Segment.Sides.IsFrontTopOrLeft == goRight ? Square.Below() : Square,
            _ => throw new System.Exception("Impossible")
        },
        new EdgeSegment(Segment.Id.FollowEdge(), Segment.Sides));
}
