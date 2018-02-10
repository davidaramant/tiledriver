// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Drawing;
using Tiledriver.Core.Extensions.Collections;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public sealed class Room
    {
        private readonly HashSet<Point> _tiles = new HashSet<Point>();

        public Room() { }
        public Room(IEnumerable<Point> tiles) => _tiles.AddRange(tiles);
        public int Area => _tiles.Count;
        public bool Contains(Point p) => _tiles.Contains(p);
        public void Add(Point p) => _tiles.Add(p);
    }
}