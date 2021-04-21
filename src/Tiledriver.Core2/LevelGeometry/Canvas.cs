// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry
{
    public sealed class Canvas : IBoard
    {
        private readonly int[] _tiles;
        private readonly int[] _sectors;
        private readonly int[] _zones;
        private readonly int[] _tags;

        public Size Dimensions { get; }

        public TileSpace this[Position pos]
        {
            get
            {
                var index = GetIndex(pos);
                return new TileSpace(_tiles[index], _sectors[index], _zones[index], _tags[index]);
            }
            set
            {
                var index = GetIndex(pos);
                _tiles[index] = value.Tile;
                _sectors[index] = value.Sector;
                _zones[index] = value.Zone;
                _tags[index] = value.Tag;
            }
        }

        public Canvas(Size dimensions)
        {
            Dimensions = dimensions;

            var size = dimensions.Width * dimensions.Height;

            _tiles = new int[size];
            _sectors = new int[size];
            _zones = new int[size];
            _tags = new int[size];

            Array.Fill(_tiles, -1);
            Array.Fill(_tags, -1);
        }

        public Canvas Fill(IEnumerable<TileSpace> tileSpaces)
        {
            int index = 0;
            foreach (var ts in tileSpaces)
            {
                _tiles[index] = ts.Tile;
                _sectors[index] = ts.Sector;
                _zones[index] = ts.Zone;
                _tags[index] = ts.Tag;

                index++;
            }

            return this;
        }

        public Canvas Fill(int? tile = null, int? sector = null, int? zone = null, int? tag = null) =>
            Fill(new Rectangle(new Position(0, 0), Dimensions), tile, sector, zone, tag);

        public Canvas Fill(Rectangle area, int? tile = null, int? sector = null, int? zone = null, int? tag = null)
        {
            foreach (var row in Enumerable.Range(area.TopLeft.Y, area.Size.Height))
            {
                var startIndex = GetIndex(area.TopLeft.X, row);

                if (tile != null)
                {
                    Array.Fill(_tiles, (int)tile, startIndex, area.Size.Width);
                }
                if (sector != null)
                {
                    Array.Fill(_sectors, (int)sector, startIndex, area.Size.Width);
                }
                if (zone != null)
                {
                    Array.Fill(_zones, (int)zone, startIndex, area.Size.Width);
                }
                if (tag != null)
                {
                    Array.Fill(_tags, (int)tag, startIndex, area.Size.Width);
                }
            }

            return this;
        }

        public Canvas Set(Position pos, int? tile = null, int? sector = null, int? zone = null, int? tag = null)
        {
            var startIndex = GetIndex(pos);

            if (tile != null)
            {
                _tiles[startIndex] = (int)tile;
            }
            if (sector != null)
            {
                _sectors[startIndex] = (int)sector;
            }
            if (zone != null)
            {
                _zones[startIndex] = (int)zone;
            }
            if (tag != null)
            {
                _tags[startIndex] = (int)tag;
            }

            return this;
        }

        public ImmutableArray<TileSpace> ToPlaneMap()
        {
            var tileSpaces = new TileSpace[_tiles.Length];

            for (int i = 0; i < _tiles.Length; i++)
            {
                tileSpaces[i] = new TileSpace(_tiles[i], _sectors[i], _zones[i], _tags[i]);
            }

            return tileSpaces.ToImmutableArray();
        }

        public Canvas ToCanvas() => new Canvas(Dimensions).Fill(ToPlaneMap());

        private int GetIndex(Position pos) => GetIndex(pos.X, pos.Y);
        private int GetIndex(int x, int y) => y * Dimensions.Width + x;
    }
}