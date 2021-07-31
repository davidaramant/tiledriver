// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry.Extensions
{
    public static class SizeExtensions
    {
        public static IEnumerable<Position> GetAllPositions(this Size area)
        {
            for (int row = 0; row < area.Height; row++)
            {
                for (int col = 0; col < area.Width; col++)
                {
                    yield return new Position(col, row);
                }
            }
        }
    }
}
