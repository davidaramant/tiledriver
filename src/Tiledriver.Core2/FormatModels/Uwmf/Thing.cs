// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial record Thing
    {
        public Point TilePosition() => new Point((int)X, (int)Y);
    }
}