// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    public class HasExit : IRule
    {
        public bool Passes(MapData data)
        {
            var exitTypes = new List<string>( new []{ "Exit_Normal", "Exit_Secret", "Exit_VictorySpin", "Exit_Victory" });
            if (data.Triggers.Any(trigger => exitTypes.Contains(trigger.Action)))
                return true;

            if (data.Things.Any(thing => thing.Type == Actor.MechaHitler.ClassName))
                return true;

            return false;
        }
    }
}
