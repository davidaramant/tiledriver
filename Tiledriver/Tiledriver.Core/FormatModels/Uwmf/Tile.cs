// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial class Tile
    {
        public void SetAllBlocking(bool enabled)
        {
            BlockingEast = BlockingNorth = BlockingWest = BlockingSouth = enabled;
        }
    }
}