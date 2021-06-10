// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Utils
{
    public static class DrawingUtil
    {
        /// <summary>
        /// Returns the sequence of points between <paramref name="position1"/> and <paramref name="position2"/>
        /// </summary>
        /// <remarks>
        /// The points are not returned in any particular order.
        /// </remarks>
        public static IEnumerable<Position> BresenhamLine(Position position1, Position position2)
            => BresenhamLine(position1.X, position1.Y, position2.X, position2.Y)
                .Select(p => new Position(p.X, p.Y));

        /// <summary>
        /// Returns the sequence of points between (x0, y0) to (x1, y1)
        /// </summary>
        /// <remarks>
        /// The points are not returned in any particular order.
        /// </remarks>
        public static IEnumerable<(int X, int Y)> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            static void Swap(ref int a, ref int b)
            {
                int c = a;
                a = b;
                b = c;
            }

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltaX = x1 - x0;
            int deltaY = Math.Abs(y1 - y0);
            int error = 0;
            int y = y0;

            int yStep = y0 < y1 ? 1 : -1;

            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                    yield return (y, x);
                else
                    yield return (x, y);

                error += deltaY;
                if (2 * error >= deltaX)
                {
                    y += yStep;
                    error -= deltaX;
                }
            }
        }
    }
}