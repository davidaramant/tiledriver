﻿// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for each passageway
    /// </summary>
    public class PassagesRule : IRankingRule
    {
        public int Rank(MapData data, LevelMap levelMap)
        {
            var passagesBetweenRooms = levelMap.AllRooms.Sum(room => room.AdjacentRooms.Count) / 2;

            return passagesBetweenRooms;
        }
    }
}