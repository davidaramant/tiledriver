// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.MapRanker
{
    public interface IRuleFactory
    {
        IEnumerable<IRankingRule> Rules { get; }
    }
}