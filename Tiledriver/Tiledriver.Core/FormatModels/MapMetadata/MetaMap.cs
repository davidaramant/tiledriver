// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public sealed class MetaMap
    {
        private readonly TileType[] _tiles;

        public int Width { get; }
        public int Height { get; }

        public int Count => Width * Height;

        public TileType this[int x, int y]
        {
            get => _tiles[y * Width + x];
            set => _tiles[y * Width + x] = value;
        }
        public TileType this[Point p]
        {
            get => this[p.X, p.Y];
            set => this[p.X, p.Y] = value;
        }

        public MetaMap(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new TileType[width * height];
        }
    }
}