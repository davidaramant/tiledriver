// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

// TODO: LeftVertex & RightVertex should not be indices because of simplification
public sealed record LineDescription(
    int LeftVertex,
    int RightVertex,
    SectorDescription FrontSector,
    SectorDescription BackSector);