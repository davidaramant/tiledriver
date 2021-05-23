// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for every 20 treasures (with special handing for 1-19 treasures).
    /// </summary>
    public class TreasureRule : IRankingRule
    {
        private const int TreasureDivisor = 20;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allTreasure = levelMap.AllRooms.SelectMany(room => room.Treasure);
            var treasureCount = allTreasure.Count();

            if (treasureCount < 1)
                return 0;

            if (treasureCount < TreasureDivisor)
                return 1;

            return treasureCount / TreasureDivisor;
        }
    }
}