// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Drawing;

namespace Tiledriver.Core.Extensions
{
    public static class PointExtensions
    {
        public static IEnumerable<Point> GetAdjacentPoints(this Point location, Size bounds)
        {
            if (location.X > 0)
                yield return new Point(location.X - 1, location.Y);

            if (location.Y < bounds.Height - 1)
                yield return new Point(location.X, location.Y + 1);

            if (location.X < bounds.Width - 1)
                yield return new Point(location.X + 1, location.Y);

            if (location.Y > 0)
                yield return new Point(location.X, location.Y - 1);
        }
    }
}
