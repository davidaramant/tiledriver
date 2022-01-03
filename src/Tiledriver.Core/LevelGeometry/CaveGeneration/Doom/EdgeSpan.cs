// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSpan(
    LatticePoint Start,
    LatticePoint End,
    EdgeSegment Segment,
    int NumSegments)
{
    public bool IsSingleSided => Segment.Back.IsOutsideLevel;
    public LatticePoint GetPointAtEnd(bool leftSide) => leftSide ? Start : End;

    public static EdgeSpan FromPosition(Position square, EdgeSegment segment) => new(
            Start: new(square, segment.Id.GetPoint(topOrLeftIsFront: segment.IsFrontTopOrLeft, leftSide: true)),
            End: new(square, segment.Id.GetPoint(topOrLeftIsFront: segment.IsFrontTopOrLeft, leftSide: false)),
            Segment: segment,
            NumSegments: 1);
}