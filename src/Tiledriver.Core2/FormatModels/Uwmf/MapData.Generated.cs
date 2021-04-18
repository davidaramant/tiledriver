// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    [GeneratedCode("DataModelGenerator", "1.0.0.0")]
    public sealed partial record MapData(
        string NameSpace,
        int TileSize,
        string Name,
        int Width,
        int Height,
        ImmutableList<Tile> Tiles,
        ImmutableList<Sector> Sectors,
        ImmutableList<Zone> Zones,
        ImmutableList<Plane> Planes,
        ImmutableList<ImmutableArray<TileSpace>> PlaneMaps,
        ImmutableList<Thing> Things,
        ImmutableList<Trigger> Triggers,
        string Comment = ""
    );
}
