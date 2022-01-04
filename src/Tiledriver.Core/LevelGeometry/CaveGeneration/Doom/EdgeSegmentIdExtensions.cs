// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class EdgeSegmentIdExtensions
{
    public static EdgeSegmentId FollowEdge(this EdgeSegmentId id) =>
        id switch
        {
            EdgeSegmentId.DiagTopLeft => EdgeSegmentId.DiagBottomRight,
            EdgeSegmentId.DiagTopRight => EdgeSegmentId.DiagBottomLeft,
            EdgeSegmentId.DiagBottomLeft => EdgeSegmentId.DiagTopRight,
            EdgeSegmentId.DiagBottomRight => EdgeSegmentId.DiagTopLeft,
            EdgeSegmentId.HorizontalLeft => EdgeSegmentId.HorizontalRight,
            EdgeSegmentId.HorizontalRight => EdgeSegmentId.HorizontalLeft,
            EdgeSegmentId.VerticalTop => EdgeSegmentId.VerticalBottom,
            EdgeSegmentId.VerticalBottom => EdgeSegmentId.VerticalTop,
            _ => throw new System.Exception("Not possible"),
        };

    public static SquarePoint GetPoint(this EdgeSegmentId id, bool topOrLeftIsFront, bool leftSide) =>
        id switch
        {
            EdgeSegmentId.DiagTopLeft => topOrLeftIsFront == leftSide ? SquarePoint.TopMiddle : SquarePoint.LeftMiddle,
            EdgeSegmentId.DiagTopRight => topOrLeftIsFront == leftSide ? SquarePoint.RightMiddle : SquarePoint.TopMiddle,
            EdgeSegmentId.DiagBottomLeft => topOrLeftIsFront == leftSide ? SquarePoint.BottomMiddle : SquarePoint.LeftMiddle,
            EdgeSegmentId.DiagBottomRight => topOrLeftIsFront == leftSide ? SquarePoint.RightMiddle : SquarePoint.BottomMiddle,
            EdgeSegmentId.HorizontalLeft => topOrLeftIsFront == leftSide ? SquarePoint.Center : SquarePoint.LeftMiddle,
            EdgeSegmentId.HorizontalRight => topOrLeftIsFront == leftSide ? SquarePoint.RightMiddle : SquarePoint.Center,
            EdgeSegmentId.VerticalTop => topOrLeftIsFront == leftSide ? SquarePoint.TopMiddle : SquarePoint.Center,
            EdgeSegmentId.VerticalBottom => topOrLeftIsFront == leftSide ? SquarePoint.Center : SquarePoint.BottomMiddle,
            _ => throw new System.NotImplementedException(),
        };

    public static LineSlope GetLineSlope(this EdgeSegmentId id) =>
        id switch
        {
            EdgeSegmentId.DiagTopLeft => LineSlope.DecreasingY,
            EdgeSegmentId.DiagTopRight => LineSlope.IncreasingY,
            EdgeSegmentId.DiagBottomLeft => LineSlope.IncreasingY,
            EdgeSegmentId.DiagBottomRight => LineSlope.DecreasingY,
            EdgeSegmentId.HorizontalLeft => LineSlope.Horizontal,
            EdgeSegmentId.HorizontalRight => LineSlope.Horizontal,
            EdgeSegmentId.VerticalTop => LineSlope.Vertical,
            EdgeSegmentId.VerticalBottom => LineSlope.Vertical,
            _ => throw new System.NotImplementedException(),
        };
}
