// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    public class UnopenableDoorsRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var unopenableDoors = levelMap.AllRooms.Sum(room => room.UnopenableDoors);

            return unopenableDoors * -2;
        }
    }
}