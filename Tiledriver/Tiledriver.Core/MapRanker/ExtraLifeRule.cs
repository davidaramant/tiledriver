// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Two points for every (reachable) extra life.
    /// </summary>
    public class ExtraLifeRule : IRankingRule
    {
        private const int PointsPerLife = 2;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allLives = levelMap.AllRooms.Sum(x => x.Lives);

            return allLives / PointsPerLife;
        }
    }
}