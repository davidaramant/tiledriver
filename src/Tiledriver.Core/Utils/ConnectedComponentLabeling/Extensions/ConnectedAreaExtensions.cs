// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Extensions;

namespace Tiledriver.Core.Utils.ConnectedComponentLabeling.Extensions
{
    public static class ConnectedAreaExtensions
    {
        public static int CountAdjacentWalls(this ConnectedArea area, Position p) =>
            (area.Contains(p.Above()) ? 0 : 1) +
            (area.Contains(p.Below()) ? 0 : 1) +
            (area.Contains(p.Left()) ? 0 : 1) +
            (area.Contains(p.Right()) ? 0 : 1);
    }
}
