// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for every 8 health items
    /// </summary>
    public class HealthRule : IRankingRule
    {
        private const int HealthDivisor = 8;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allHealth = levelMap.AllRooms.SelectMany(room => room.Health);

            return allHealth.Count() / HealthDivisor;
        }
    }
}