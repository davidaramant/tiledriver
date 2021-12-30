// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed class InternalEdges
{
    public EdgeInfo? DiagTopLeft { get; set; }
    public EdgeInfo? DiagTopRight { get; set; }
    public EdgeInfo? DiagBottomLeft { get; set; }
    public EdgeInfo? DiagBottomRight { get; set; }
    public EdgeInfo? HorizontalLeft { get; set; }
    public EdgeInfo? HorizontalRight { get; set; }
    public EdgeInfo? VerticalTop { get; set; }
    public EdgeInfo? VerticalBottom { get; set; }

    public bool NoneLeft =>
        DiagTopLeft == null &&
        DiagTopRight == null &&
        DiagBottomLeft == null &&
        DiagBottomRight == null &&
        HorizontalLeft == null &&
        HorizontalRight == null &&
        VerticalTop == null &&
        VerticalBottom == null;
}
