// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSegment(
    EdgeSegmentId Id,
    SectorDescription Front,
    SectorDescription Back,
    SquarePoint Left,
    SquarePoint Right);