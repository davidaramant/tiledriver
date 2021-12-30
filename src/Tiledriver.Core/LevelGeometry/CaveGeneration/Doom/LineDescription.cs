// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

// TODO: Ideally there should be two texture offsets for front and back...
// Right now it's not possible to have textures on both sides
// ... and the texture offset should probably go on a SideDefDescription anyway

public sealed record LineDescription(
    int LeftVertex,
    int RightVertex,
    SectorDescription FrontSector,
    SectorDescription BackSector,
    int TextureOffset);