// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    public class Passage
    {
        private readonly MapLocation _loc;

        public Trigger Door => _loc.Actions.Single(t=>t.Action == "Door_Open");

        public Passage(MapLocation loc)
        {
            _loc = loc;
        }
    }
}