// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

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
        public int Tile { get; }
        public MapTileType Type { get; }

        private MapTile(int? tag, int tile, MapTileType type)
        {
            Tag = tag;
            Tile = tile;
            Type = type;
        }

        public static readonly MapTile EmptyTile = new MapTile(tag: null, tile: 0, type: MapTileType.EmptySpace);
        public static readonly MapTile NullTile = new MapTile(tag: null, tile: 0, type: MapTileType.Null);

        public static MapTile Textured(int theme, int? tag = null)
        {
            return new MapTile(tag: tag, tile: theme, type: MapTileType.Textured);
        }
    }
}