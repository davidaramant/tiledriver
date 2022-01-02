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

    public Dictionary<EdgeSegmentId, EdgeSegment> GetInternalEdges()
    {
        var lookup = new Dictionary<EdgeSegmentId, EdgeSegment>();

        void AddEdge(EdgeSegmentId id, SquareSegment topOrLeftId, SquareSegment bottomOrRightId)
        {
            var topOrLeft = this[topOrLeftId];
            var bottomOrRight = this[bottomOrRightId];

            if (topOrLeft != bottomOrRight)
            {
                lookup.Add(id, EdgeSegment.Construct(id, topOrLeft, bottomOrRight));
            }
        }

        AddEdge(EdgeSegmentId.DiagTopLeft, topOrLeftId: SquareSegment.UpperLeftOuter, bottomOrRightId: SquareSegment.UpperLeftInner);
        AddEdge(EdgeSegmentId.DiagTopRight, topOrLeftId: SquareSegment.UpperRightOuter, bottomOrRightId: SquareSegment.UpperRightInner);
        AddEdge(EdgeSegmentId.DiagBottomRight, topOrLeftId: SquareSegment.LowerRightInner, bottomOrRightId: SquareSegment.LowerRightOuter);
        AddEdge(EdgeSegmentId.DiagBottomLeft, topOrLeftId: SquareSegment.LowerLeftInner, bottomOrRightId: SquareSegment.LowerLeftOuter);
        AddEdge(EdgeSegmentId.HorizontalLeft, topOrLeftId: SquareSegment.UpperLeftInner, bottomOrRightId: SquareSegment.LowerLeftInner);
        AddEdge(EdgeSegmentId.HorizontalRight, topOrLeftId: SquareSegment.UpperRightInner, bottomOrRightId: SquareSegment.LowerRightInner);
        AddEdge(EdgeSegmentId.VerticalTop, topOrLeftId: SquareSegment.UpperLeftInner, bottomOrRightId: SquareSegment.UpperRightInner);
        AddEdge(EdgeSegmentId.VerticalBottom, topOrLeftId: SquareSegment.LowerLeftInner, bottomOrRightId: SquareSegment.LowerRightInner);

        return lookup;
    }
}