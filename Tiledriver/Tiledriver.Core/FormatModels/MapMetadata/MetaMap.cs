// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using System.IO;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public sealed class MetaMap
    {
        private const ulong Version = 0x100;

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

        public void Save(string path)
        {
            using (var fs = File.Open(path, FileMode.Create))
            using (var writer = new BinaryWriter(fs))
            {
                writer.Write(Version);
                writer.Write(Width);
                writer.Write(Height);

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        writer.Write((byte)this[x, y]);
                    }
                }
            }
        }

        public static MetaMap Load(string path)
        {
            using (var fs = File.Open(path, FileMode.Open))
            using (var reader = new BinaryReader(fs))
            {
                if (reader.ReadUInt64() != Version)
                {
                    throw new ParsingException("Unsupported version");
                }
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();

                var map = new MetaMap(width, height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        map[x, y] = (TileType) reader.ReadByte();
                    }
                }

                return map;
            }
        }
    }
}