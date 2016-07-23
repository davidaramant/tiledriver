﻿using System.Linq;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.Utilities
{
    public static class MapExtensions
    {
        public static TileSpace TileSpaceAt(this Map map, int x, int y) => map.PlaneMaps[0].TileSpaces[y * map.Height + x];
        public static Tile TileAt(this Map map, int index) => map.Tiles.ElementAtOrDefault(index);
    }
}
