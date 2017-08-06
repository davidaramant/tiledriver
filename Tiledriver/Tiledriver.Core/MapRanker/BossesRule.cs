// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Scores each boss as a positive 60.
    /// </summary>
    public class BossesRule : IRankingRule
    {
        private const int PointMultiplier = 60;

        public int Rank(MapData data, LevelMap levelMap)
        {
            return levelMap.AllRooms.SelectMany(room => room.Bosses).Count() * PointMultiplier;
        }
    }
}