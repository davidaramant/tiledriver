// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Gui.Utilities
{
    public static class MapExtensions
    {
        public static TileSpace TileSpaceAt(this MapData mapData, int x, int y) => mapData.PlaneMaps[0].TileSpaces[y * mapData.Height + x];
        public static Tile TileAt(this MapData mapData, int index) => mapData.Tiles.ElementAtOrDefault(index);
        public static Sector SectorAt(this MapData mapData, int index) => mapData.Sectors.ElementAtOrDefault(index);
    }
}
