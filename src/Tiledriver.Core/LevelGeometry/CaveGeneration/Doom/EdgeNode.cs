// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeNode(
    Position Square,
    EdgeSegment Segment)
{
    public bool IsSingleSided => Segment.Back.IsOutsideLevel;
    public LatticePoint StartPoint => new(Square, Segment.Id.GetPoint(topOrLeftIsFront: Segment.IsFrontTopOrLeft, leftSide: true));
    public LatticePoint EndPoint => new(Square, Segment.Id.GetPoint(topOrLeftIsFront: Segment.IsFrontTopOrLeft, leftSide: false));

    public LatticePoint GetPointAtEnd(bool leftSide) => leftSide ? StartPoint : EndPoint;

    public EdgeNode FollowLine(bool goRight) => new(
        Segment.Id switch
        {
            EdgeSegmentId.DiagTopLeft => Segment.IsFrontTopOrLeft == goRight ? Square.Left() : Square.Above(),
            EdgeSegmentId.DiagTopRight => Segment.IsFrontTopOrLeft == goRight ? Square.Above() : Square.Right(),
            EdgeSegmentId.DiagBottomRight => Segment.IsFrontTopOrLeft == goRight ? Square.Below() : Square.Right(),
            EdgeSegmentId.DiagBottomLeft => Segment.IsFrontTopOrLeft == goRight ? Square.Left() : Square.Below(),
            EdgeSegmentId.HorizontalLeft => Segment.IsFrontTopOrLeft == goRight ? Square.Left() : Square,
            EdgeSegmentId.HorizontalRight => Segment.IsFrontTopOrLeft == goRight ? Square : Square.Right(),
            EdgeSegmentId.VerticalTop => Segment.IsFrontTopOrLeft == goRight ? Square : Square.Above(),
            EdgeSegmentId.VerticalBottom => Segment.IsFrontTopOrLeft == goRight ? Square.Below() : Square,
            _ => throw new System.Exception("Impossible")
        },
        Segment with { Id = Segment.Id.FollowEdge() });
}
