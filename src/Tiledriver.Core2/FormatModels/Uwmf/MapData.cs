// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial record MapData
    {
        public TileSpace TileSpaceAt(Point p) => PlaneMaps[0].TileSpaces[p.Y * Width + p.X];
    }
}