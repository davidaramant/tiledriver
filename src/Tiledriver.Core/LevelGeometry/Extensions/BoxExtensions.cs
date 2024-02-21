// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.Extensions
{
    public static class BoxExtensions
    {
        public static IEnumerable<Position> GetAllPositions(this IBox box) =>
            from y in Enumerable.Range(0, box.Height)
            from x in Enumerable.Range(0, box.Width)
            select new Position(x, y);

        /// <summary>
        /// Returns all the positions inside the box except for the max horizontal/vertical edges.
        /// </summary>
        public static IEnumerable<Position> GetAllPositionsExclusiveMax(this IBox box) =>
            from y in Enumerable.Range(0, box.Height - 1)
            from x in Enumerable.Range(0, box.Width - 1)
            select new Position(x, y);
    }
}
