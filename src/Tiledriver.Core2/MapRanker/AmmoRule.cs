// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for every five ammo found
    /// </summary>
    public class AmmoRule : IRankingRule
    {
        private const int AmmoDivisor = 5;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allAmmo = levelMap.AllRooms.Sum(room => room.Ammo);

            return allAmmo / AmmoDivisor;
        }
    }
}