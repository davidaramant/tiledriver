// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public static class InternalEdgeIdExtensions
{
    public static InternalEdgeId FollowEdge(this InternalEdgeId id) => id switch
    {
        InternalEdgeId.DiagTopLeft => InternalEdgeId.DiagBottomRight,
        InternalEdgeId.DiagTopRight => InternalEdgeId.DiagBottomLeft,
        InternalEdgeId.DiagBottomLeft => InternalEdgeId.DiagTopRight,
        InternalEdgeId.DiagBottomRight => InternalEdgeId.DiagTopLeft,
        InternalEdgeId.HorizontalLeft => InternalEdgeId.HorizontalRight,
        InternalEdgeId.HorizontalRight => InternalEdgeId.HorizontalLeft,
        InternalEdgeId.VerticalTop => InternalEdgeId.VerticalBottom,
        InternalEdgeId.VerticalBottom => InternalEdgeId.VerticalTop,
        _ => throw new System.Exception("Not possible"),
    };
}
