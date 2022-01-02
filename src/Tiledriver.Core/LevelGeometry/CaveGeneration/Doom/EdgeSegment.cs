// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSegment(
    EdgeSegmentId Id,
    SectorDescription Front,
    SectorDescription Back,
    bool IsFrontTopOrLeft)
{
    public static EdgeSegment Construct(
        EdgeSegmentId id,
        SectorDescription topOrLeft,
        SectorDescription bottomOrRight)
    {
        var topIsSmallest = topOrLeft.CompareTo(bottomOrRight) < 0;
        return new(
            id,
            Front: topIsSmallest ? topOrLeft : bottomOrRight,
            Back: topIsSmallest ? bottomOrRight : topOrLeft,
            IsFrontTopOrLeft: topIsSmallest);
    }
}