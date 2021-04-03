// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Bonus points for weapons
    /// </summary>
    public class WeaponsRule : IRankingRule
    {
        private const int GatlingScore = 8;
        private const int MachineScore = 3;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allWeapons = levelMap.AllRooms.SelectMany(room => room.Weapons).ToList();

            var score = 0;

            if (allWeapons.Any(weapon => weapon.Type == Actor.GatlingGunUpgrade.ClassName))
                score += GatlingScore;

            if (allWeapons.Any(weapon => weapon.Type == Actor.MachineGun.ClassName))
                score += MachineScore;

            return score;
        }
    }
}