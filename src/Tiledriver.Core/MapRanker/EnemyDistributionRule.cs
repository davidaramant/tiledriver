// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for each 10% of rooms with enemies.
    /// </summary>
    public class EnemyDistributionRule : IRankingRule
    {
        private const double TippingPoint = 0.6;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var roomCount = levelMap.AllRooms.Count();
            var roomsWithEnemies = levelMap.AllRooms.Count(room => room.Enemies.Any());

            var percentageWithEnemies = (double) roomsWithEnemies / roomCount;

            if (percentageWithEnemies <= TippingPoint)
            {
                return (int) (percentageWithEnemies * 10);
            }

            return (int) ((percentageWithEnemies - TippingPoint) * -10 + TippingPoint * 10);
        }
    }
}