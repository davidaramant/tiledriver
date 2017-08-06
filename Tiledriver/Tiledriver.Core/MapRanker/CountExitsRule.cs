// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Adds a score of 5 for each reachable exit.
    /// </summary>
    public class CountExitsRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            return levelMap.EndingRooms.Count() * 5;
        }
    }
}