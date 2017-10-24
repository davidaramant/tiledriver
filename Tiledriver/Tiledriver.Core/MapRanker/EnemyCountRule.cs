﻿// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

namespace Tiledriver.Core.MapRanker
{
    /// <summary>
    /// One point for every 3 enemies in the level
    /// </summary>
    public class EnemyCountRule : IRankingRule
    {
        private const int EnemyDivisor = 3;

        public int Rank(MapData data, LevelMap levelMap)
        {
            return levelMap.AllRooms.Sum(room => room.Enemies.Count()) / EnemyDivisor;
        }
    }
}