// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Gui.Utilities
{
    public static class MapExtensions
    {
        public static TileSpace TileSpaceAt(this Map map, int x, int y) => map.PlaneMaps[0].TileSpaces[y * map.Height + x];
        public static Tile TileAt(this Map map, int index) => map.Tiles.ElementAtOrDefault(index);
        public static Sector SectorAt(this Map map, int index) => map.Sectors.ElementAtOrDefault(index);
    }
}
