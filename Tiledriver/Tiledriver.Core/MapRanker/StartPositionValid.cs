// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    public class StartPositionValid : IRule
    {
        public bool Passes(MapData data)
        {
            var startPostion = data.Things.Single(x => x.Type == Actor.Player1Start.ClassName);
            var index = data.Width * (int) startPostion.Y + (int) startPostion.X;
            var tile = data.PlaneMaps.Single().TileSpaces[index];

            if (tile.Tile == -1)
                return true;

            return false;
        }
    }
}