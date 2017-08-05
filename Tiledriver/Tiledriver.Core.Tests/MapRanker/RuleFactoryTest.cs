// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.MapRanker;

namespace Tiledriver.Core.Tests.MapRanker
{
    [TestFixture]
    public class RuleFactoryTest
    {
        private RuleFactory _target;

        [SetUp]
        public void Setup()
        {
            _target = new RuleFactory();
        }

        [Test]
        public void ShouldBuildRulesInCorrectOrder()
        {
            var rules = _target.Rules.ToList();

            Assert.That(rules.Count, Is.EqualTo(2));
            Assert.That(rules[0], Is.InstanceOf<HasStartPosition>());
            Assert.That(rules[1], Is.InstanceOf<StartPositionValid>());
        }
    }
}