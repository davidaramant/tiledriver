// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling
{
    public sealed class ConnectedArea : IEnumerable<Position>
    {
        private readonly HashSet<Position> _positions;

        public ConnectedArea(IEnumerable<Position> tiles) => _positions = tiles.ToHashSet();

        public int Area => _positions.Count;

        public bool Contains(Position p) => _positions.Contains(p);

        public IEnumerator<Position> GetEnumerator() => _positions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns a new area with excess space removed.
        /// </summary>
        /// <param name="border">How thick of a border to leave.</param>
        /// <returns>The new area and the resulting size.</returns>
        public (ConnectedArea TrimmedArea, Size Dimensions) TrimExcess(int border = 0)
        {
            int minX = int.MaxValue;
            int maxX = 0;
            int minY = int.MaxValue;
            int maxY = 0;

            foreach (var pos in _positions)
            {
                minX = Math.Min(minX, pos.X);
                maxX = Math.Max(maxX, pos.X);
                minY = Math.Min(minY, pos.Y);
                maxY = Math.Max(maxY, pos.Y);
            }

            var adjustment = new PositionDelta(border - minX, border - minY);

            return (
                new ConnectedArea(_positions.Select(p => p + adjustment)),
                new Size((maxX - minX + 1) + 2 * border, (maxY - minY + 1) + 2 * border)
            );
        }
    }
}
