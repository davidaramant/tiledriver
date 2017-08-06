// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Scores 0 for valid, scores -1000 for invalid.
    /// </summary>
    public class HasExitRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            if (levelMap.EndingRooms.Any())
                return Constants.ValidScore;

            return Constants.InvalidScore;
        }
    }
}