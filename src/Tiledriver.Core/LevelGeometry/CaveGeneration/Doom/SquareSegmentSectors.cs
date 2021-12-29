// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed class SquareSegmentSectors
{
    private readonly SectorDescription[] _sectors;

    public SquareSegmentSectors(IEnumerable<SectorDescription> sectors) => _sectors = sectors.ToArray();

    public SectorDescription this[SquareSegment id] => _sectors[(int)id];
    public bool IsUniform => _sectors.Skip(1).All(s => s == _sectors[0]);

    public IEnumerable<InternalEdge> GetEdges() =>
        new InternalEdge[]
        {
                new InternalEdge.DiagTopLeft(
                    UpperLeftOuter:this[SquareSegment.UpperLeftOuter],
                    UpperLeftInner:this[SquareSegment.UpperLeftInner]),
                new InternalEdge.DiagTopRight(
                    UpperRightOuter:this[SquareSegment.UpperRightOuter],
                    UpperRightInner:this[SquareSegment.UpperRightInner]),
                new InternalEdge.DiagBottomRight(
                    LowerRightOuter:this[SquareSegment.LowerRightOuter],
                    LowerRightInner:this[SquareSegment.LowerRightInner]),
                new InternalEdge.DiagBottomLeft(
                    LowerLeftOuter:this[SquareSegment.LowerLeftOuter],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
                new InternalEdge.HorizontalLeft(
                    UpperLeftInner:this[SquareSegment.UpperLeftInner],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
                new InternalEdge.HorizontalRight(
                    UpperRightInner:this[SquareSegment.UpperRightInner],
                    LowerRightInner:this[SquareSegment.LowerRightInner]),
                new InternalEdge.VerticalTop(
                    UpperLeftInner:this[SquareSegment.UpperLeftInner],
                    UpperRightInner:this[SquareSegment.UpperRightInner]),
                new InternalEdge.VerticalBottom(
                    LowerRightInner:this[SquareSegment.LowerRightInner],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
        }.Where(ie => ie.IsValid);
}