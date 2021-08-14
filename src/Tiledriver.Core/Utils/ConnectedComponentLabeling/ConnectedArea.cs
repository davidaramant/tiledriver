﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling
{
    public sealed class ConnectedArea : IEnumerable<Position>
    {
        private readonly HashSet<Position> _tiles;

        public ConnectedArea(IEnumerable<Position> tiles) => _tiles = tiles.ToHashSet();
        public int Area => _tiles.Count;
        public bool Contains(Position p) => _tiles.Contains(p);
        public IEnumerator<Position> GetEnumerator() => _tiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}