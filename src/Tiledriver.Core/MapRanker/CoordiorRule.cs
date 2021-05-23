// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Lose points for too many corridors
    /// </summary>
    public class CoordiorRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var locations = levelMap.AllRooms.SelectMany(r => r.Locations).ToList();

            var percentCooridor = locations.Count(loc => loc.Cooridor) / (double) locations.Count();

            return (int)(percentCooridor * -200);
        }
    }
}