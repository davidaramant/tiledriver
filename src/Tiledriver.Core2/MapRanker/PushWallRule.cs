// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for every pushwall.
    /// </summary>
    public class PushWallRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var pushWallCount = 0;
            var doorCount = 0;
            foreach (var room in levelMap.AllRooms)
            {
                foreach (var adjacentPair in room.AdjacentRooms)
                {
                    foreach (var passage in adjacentPair.Key)
                    {
                        if (null != passage.Pushwall)
                            pushWallCount++;
                        else if (null != passage.Door)
                            doorCount++;
                    }
                }
            }

            if (pushWallCount > doorCount)
            {
                return (doorCount - pushWallCount);
            }

            return pushWallCount;
        }
    }
}