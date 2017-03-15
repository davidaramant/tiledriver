using System.Linq;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.Gui.Utilities
{
    public static class MapExtensions
    {
        public static TileSpace TileSpaceAt(this Map map, int x, int y) => map.PlaneMaps[0].TileSpaces[y * map.Height + x];
        public static Tile TileAt(this Map map, int index) => map.Tiles.ElementAtOrDefault(index);
        public static Sector SectorAt(this Map map, int index) => map.Sectors.ElementAtOrDefault(index);
    }
}
