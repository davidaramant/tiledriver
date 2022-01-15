// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

// TODO: Capture texture offsets (front + back? would they differ?)

public sealed record LineDescription(
    int LeftVertex,
    int RightVertex,
    SectorDescription FrontSector,
    SectorDescription BackSector,
    int TextureXOffset = 0)
{
    public bool IsTwoSided => !BackSector.IsOutsideLevel;
}