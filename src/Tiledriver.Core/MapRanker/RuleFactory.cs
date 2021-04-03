// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tiledriver.Core.MapRanker
{
    public class RuleFactory : IRuleFactory
    {
        public IEnumerable<IRankingRule> Rules
        {
            get
            {
                return Assembly
                    .GetAssembly(typeof(RuleFactory))
                    .GetTypes()
                    .Where(type => typeof(IRankingRule).IsAssignableFrom(type))
                    .Where(type => !type.IsInterface)
                    .Select(type => (IRankingRule) Activator.CreateInstance(type));
            }
        }
    }
}