// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// Three points for each (reachable) locked door.
    /// </summary>
    public class LockedDoorsRule : IRankingRule
    {
        private const int PointsPerLockedDoor = 3;

        public int Rank(MapData data, LevelMap levelMap)
        {
            var lockedDoorCount = 0;
            foreach (var room in levelMap.AllRooms)
            {
                foreach (var adjacentPair in room.AdjacentRooms)
                {
                    foreach (var passage in adjacentPair.Key)
                    {
                        var doorAction = passage.Door;
                        if (null == doorAction)
                            continue;

                        var lockLevel = (LockLevel) doorAction.Arg3;

                        switch (lockLevel)
                        {
                            case LockLevel.Gold:
                            case LockLevel.Silver:
                            case LockLevel.Both:
                                lockedDoorCount++;
                                break;
                        }
                    }
                }
            }

            return lockedDoorCount * PointsPerLockedDoor;
        }
    }
}