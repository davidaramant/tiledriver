// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.MapRanker
{
    public class Ranker
    {
        private readonly  IRuleFactory _factory;

        public Ranker()
            : this(new RuleFactory())
        {
        }

        public Ranker(IRuleFactory factory)
        {
            _factory = factory;
        }

        public int RankLevel(MapData data)
        {
            foreach (var rule in _factory.Rules)
            {
                if (!rule.Passes(data))
                    return -1;
            }

            return 1;
        }
    }
}
