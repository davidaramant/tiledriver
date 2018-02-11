// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Uwmf.Extensions
{
    public static class UwmfExtensions
    {
        public static TileSpace TileSpaceAt(this MapData map, Point p) => map.PlaneMaps[0].TileSpaces[p.Y * map.Width + p.X];
    }
}