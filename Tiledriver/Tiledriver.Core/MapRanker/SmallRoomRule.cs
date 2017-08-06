// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    public class SmallRoomRule : IRankingRule
    {
        private const int SmallRoomSize = 6;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var smallRoomCount = levelMap.AllRooms.Count(room => room.Locations.Count <= SmallRoomSize);

            return smallRoomCount / 4 * -1;
        }
    }
}