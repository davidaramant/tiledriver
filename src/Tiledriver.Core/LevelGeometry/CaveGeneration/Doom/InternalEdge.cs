// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public abstract record InternalEdge(bool IsValid)
{
    public sealed record DiagTopLeft(
        SectorDescription UpperLeftOuter,
        SectorDescription UpperLeftInner)
        : InternalEdge(UpperLeftOuter != UpperLeftInner);
    public sealed record DiagTopRight(
        SectorDescription UpperRightOuter,
        SectorDescription UpperRightInner)
        : InternalEdge(UpperRightOuter != UpperRightInner);
    public sealed record DiagBottomRight(
        SectorDescription LowerRightOuter,
        SectorDescription LowerRightInner)
        : InternalEdge(LowerRightOuter != LowerRightInner);
    public sealed record DiagBottomLeft(
        SectorDescription LowerLeftOuter,
        SectorDescription LowerLeftInner)
        : InternalEdge(LowerLeftOuter != LowerLeftInner);
    public sealed record HorizontalLeft(
        SectorDescription UpperLeftInner,
        SectorDescription LowerLeftInner)
        : InternalEdge(UpperLeftInner != LowerLeftInner);
    public sealed record HorizontalRight(
        SectorDescription UpperRightInner,
        SectorDescription LowerRightInner)
        : InternalEdge(UpperRightInner != LowerRightInner);
    public sealed record VerticalTop(
        SectorDescription UpperLeftInner,
        SectorDescription UpperRightInner)
        : InternalEdge(UpperLeftInner != UpperRightInner);
    public sealed record VerticalBottom(
        SectorDescription LowerRightInner,
        SectorDescription LowerLeftInner)
        : InternalEdge(LowerRightInner != LowerLeftInner);
}