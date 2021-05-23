// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for each enemy type that is at least 5% of the total enemy
    /// number.
    /// </summary>
    public class EnemyVarietyRule : IRankingRule
    {
        private const double AcceptablePercentage = 0.05;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allEnemies = levelMap.AllRooms.SelectMany(room => room.Enemies).ToList();
            var enemiesByType = allEnemies.GroupBy(enemy => enemy.Type);
            
            var significantCount = enemiesByType.Count(group => (double) group.Count() / allEnemies.Count >= AcceptablePercentage);

            return significantCount;
        }
    }
}