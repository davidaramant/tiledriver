﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed record EdgeSegment(
    EdgeSegmentId Id, 
    EdgeInfo Sides);