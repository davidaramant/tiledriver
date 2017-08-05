// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.MapRanker
{
    public class HasStartPosition : IRule
    {
        public bool Passes(MapData data)
        {
            return data.Things.Count(thing => thing.Type == Actor.Player1Start.ClassName) == 1;
        }
    }
}