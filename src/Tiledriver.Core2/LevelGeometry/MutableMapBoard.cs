// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry
{
    public sealed class MutableMapBoard : IMapBoard
    {
        private readonly int[] _tiles;
        private readonly int[] _sectors;
        private readonly int[] _zones;
        private readonly int[] _tags;

        public Size Dimensions { get; }

        public TileSpace this[MapPosition pos]
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

        public MutableMapBoard(Size dimensions, IEnumerable<TileSpace> tileSpaces)
        {
            Dimensions = dimensions;

            var size = dimensions.Width * dimensions.Height;

            _tiles = new int[size];
            _sectors = new int[size];
            _zones = new int[size];
            _tags = new int[size];

            int index = 0;
            foreach (var ts in tileSpaces)
            {
                _tiles[index] = ts.Tile;
                _sectors[index] = ts.Sector;
                _zones[index] = ts.Zone;
                _tags[index] = ts.Tag;

                index++;
            }
        }

        private int GetIndex(MapPosition pos) => pos.Y * Dimensions.Width + pos.X;
    }
}