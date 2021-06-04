// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections;
using System.Collections.Generic;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public sealed class Room : IEnumerable<Position>
    {
        private readonly HashSet<Position> _tiles = new();

        public Room() { }
        public Room(IEnumerable<Position> tiles) => _tiles.AddRange(tiles);
        public int Area => _tiles.Count;
        public bool Contains(Position p) => _tiles.Contains(p);
        public void Add(Position p) => _tiles.Add(p);
        public IEnumerator<Position> GetEnumerator() => _tiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}