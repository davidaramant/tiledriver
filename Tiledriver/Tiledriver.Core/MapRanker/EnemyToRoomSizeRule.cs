// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Any room that has at least 15% of the total rooms space filled with
    /// enemies is a negative point.
    /// </summary>
    public class EnemyToRoomSizeRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var score = 0;
            foreach (var room in levelMap.AllRooms)
            {
                if ((double) room.Enemies.Count() / room.Locations.Count >= 0.15)
                {
                    score--;
                }
            }

            return score;
        }
    }
}