// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.MapRanker
{
    public class RuleFactory : IRuleFactory
    {
        public IEnumerable<IRule> Rules
        {
            get
            {
                yield return new HasStartPosition();
                yield return new StartPositionValid();
            }
        }
    }
}