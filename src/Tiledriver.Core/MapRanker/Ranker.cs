// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Mapping;

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
            var mapper = new LevelMapper();
            var levelMap = mapper.Map(data);

            var consolidatedScore = 0;
            foreach (var rule in _factory.Rules)
            {
                var ruleScore = rule.Rank(data, levelMap);
                consolidatedScore += ruleScore;

                Console.WriteLine($"DEBUG: {rule.GetType().Name} - {ruleScore}");
            }

            return consolidatedScore;
        }
    }
}
