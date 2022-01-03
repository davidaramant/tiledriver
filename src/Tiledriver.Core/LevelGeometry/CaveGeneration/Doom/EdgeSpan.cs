// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSpan(
    LatticePoint Start,
    LatticePoint End,
    EdgeSegment Segment);