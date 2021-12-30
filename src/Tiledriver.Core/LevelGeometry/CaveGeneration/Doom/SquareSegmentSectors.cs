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

    private EdgeInfo? GetEdge(SquareSegment topOrLeftId, SquareSegment bottomOrRightId)
    {
        var topOrLeft = this[topOrLeftId];
        var bottomOrRight = this[bottomOrRightId];

        return topOrLeft != bottomOrRight ? new EdgeInfo(topOrLeft, bottomOrRight) : null;
    }

    public InternalEdges GetInternalEdges() => new()
    {
        DiagTopLeft = GetEdge(topOrLeftId: SquareSegment.UpperLeftOuter, bottomOrRightId: SquareSegment.UpperLeftInner),
        DiagTopRight = GetEdge(topOrLeftId: SquareSegment.UpperRightOuter, bottomOrRightId: SquareSegment.UpperRightInner),
        DiagBottomRight = GetEdge(topOrLeftId: SquareSegment.LowerRightInner, bottomOrRightId: SquareSegment.LowerRightOuter),
        DiagBottomLeft = GetEdge(topOrLeftId: SquareSegment.LowerLeftInner, bottomOrRightId: SquareSegment.LowerLeftOuter),
        
        HorizontalLeft = GetEdge(topOrLeftId: SquareSegment.UpperLeftInner, bottomOrRightId: SquareSegment.LowerLeftInner),
        HorizontalRight = GetEdge(topOrLeftId: SquareSegment.UpperRightInner, bottomOrRightId: SquareSegment.LowerRightInner),
        VerticalTop = GetEdge(topOrLeftId: SquareSegment.UpperLeftInner, bottomOrRightId: SquareSegment.UpperRightInner),
        VerticalBottom = GetEdge(topOrLeftId: SquareSegment.LowerLeftInner, bottomOrRightId: SquareSegment.LowerRightInner),
    };
}