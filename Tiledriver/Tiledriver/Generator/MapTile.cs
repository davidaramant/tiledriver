using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public enum MapTileType
    {
        Null,
        EmptySpace,
        Textured
    }

    public sealed class MapTile
    {
        public int? Tag { get; }
        public TileTheme Theme { get; }
        public MapTileType Type { get; }

        private MapTile(int? tag, TileTheme theme, MapTileType type)
        {
            Tag = tag;
            Theme = theme;
            Type = type;
        }

        public static readonly MapTile EmptyTile = new MapTile(tag: null, theme: null, type: MapTileType.EmptySpace);
        public static readonly MapTile NullTile = new MapTile(tag: null, theme: null, type: MapTileType.Null);

        public static MapTile Textured(TileTheme theme, int? tag = null)
        {
            return new MapTile(tag:tag,theme:theme,type:MapTileType.Textured);
        }
    }
}
