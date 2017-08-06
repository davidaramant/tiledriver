// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Scores each boss as a positive 15.
    /// </summary>
    public class CountBossesRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var bossTypes = new[]
            {
                Actor.MechaHitler.ClassName,
                Actor.FakeHitler.ClassName,
                Actor.Hans.ClassName,
                Actor.Gretel.ClassName,
                Actor.FatFace.ClassName,
                Actor.Schabbs.ClassName
            };

            return data.Things.Count(thing => bossTypes.Contains(thing.Type)) * 15;
        }
    }
}