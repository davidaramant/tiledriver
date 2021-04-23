// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Scores 0 for valid, -1000 for invalid
    /// </summary>
    public class HasStartRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            if (data.Things.Any(thing => thing.Type == Actor.Player1Start.ClassName))
                return Constants.ValidScore;

            return Constants.InvalidScore;
        }
    }
}