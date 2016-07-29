// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry
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
            return new MapTile(tag: tag, theme: theme, type: MapTileType.Textured);
        }
    }
}