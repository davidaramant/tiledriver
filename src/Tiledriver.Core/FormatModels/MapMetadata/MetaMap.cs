// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.IO;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.LevelGeometry;

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
        public TileType this[Position p]
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

        private MetaMap(int width, int height, TileType[] tiles)
        {
            Width = width;
            Height = height;
            _tiles = tiles;
        }

        public void Save(string path)
        {
            using var fs = File.Open(path, FileMode.Create);
            using var writer = new BinaryWriter(fs);
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

        public MetaMap Rotate90()
        {
            System.Diagnostics.Debug.Assert(Width == 64 && Height == 64, "This is hardcoded to work with 64x64 maps");
            var rotated = new TileType[Width * Height];

            for (int row = 0; row < 64; ++row)
            {
                for (int col = 0; col < 64; ++col)
                {
                    rotated[row * 64 + col] = _tiles[(64 - col - 1) * 64 + row];
                }
            }

            return new MetaMap(Width, Height, rotated);
        }

        public MetaMap Mirror()
        {
            System.Diagnostics.Debug.Assert(Width == 64 && Height == 64, "This is hardcoded to work with 64x64 maps");
            var mirrored = new TileType[Width * Height];

            for (int row = 0; row < 64; ++row)
            {
                for (int col = 0; col < 32; ++col)
                {
                    mirrored[row * 64 + col] = _tiles[row * 64 + (63 - col)];
                    mirrored[row * 64 + (63 - col)] = _tiles[row * 64 + col];
                }
            }

            return new MetaMap(Width, Height, mirrored);
        }

        public static MetaMap Load(string path)
        {
            using var fs = File.Open(path, FileMode.Open);
            using var reader = new BinaryReader(fs);
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
                    map[x, y] = (TileType)reader.ReadByte();
                }
            }

            return map;
        }
    }
}