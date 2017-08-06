// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Negative points for the percentage of "boring" reachable tiles over 65%
    /// </summary>
    public class BoringTileRule : IRankingRule
    {
        private const double AcceptablePercentage = 0.70;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var allLocations = levelMap.AllRooms.SelectMany(room => room.Locations).Count();
            var boringLocations = levelMap.AllRooms.Sum(room => room.BoringTiles);

            var boringPercentage = boringLocations / (double)allLocations;

            Console.WriteLine("Boring Percent: {0}", boringPercentage);

            if (boringPercentage <= AcceptablePercentage)
                return 0;

            return (int) ((boringPercentage - AcceptablePercentage) * -100);
        }
    }
}