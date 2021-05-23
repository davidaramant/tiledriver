// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Penalties for rooms that have more than 1/6 of the locations covered
    /// with enemies.
    /// </summary>
    public class EnemyToRoomSizeRule : IRankingRule
    {
        private const int EnemyToSizeDivisor = 6;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var score = 0;
            foreach (var room in levelMap.AllRooms)
            {
                var tolerableEnemyCount = (int) Math.Ceiling((double) room.Locations.Count / EnemyToSizeDivisor);
                var excessEnemies = room.Enemies.Count - tolerableEnemyCount;

                if (excessEnemies > 0)
                {
                    var penalizeTime = 1 + excessEnemies / tolerableEnemyCount;
                    score -= penalizeTime * -1;
                }
            }

            return score;
        }
    }
}