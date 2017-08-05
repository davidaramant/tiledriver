// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.MapRanker;
using System.Collections.Generic;

namespace Tiledriver.Core.Tests.MapRanker
{
    [TestFixture]
    public class HasExitTests
    {
        private HasExit _target;
        private MapData data;

        [SetUp]
        public void FixtureSetup()
        {
            var exitTypes = new List<string>(new string[] { "Exit_Normal", "Exit_Secret", "Exit_VictorySpin", "Exit_Victory" });
            data = DemoMap.Create();
            data.Triggers.RemoveAll(t => exitTypes.Contains(t.Action));

            _target = new HasExit();
        }

        [TestCase("Exit_Normal")]
        [TestCase("Exit_Secret")]
        [TestCase("Exit_VictorySpin")]
        [TestCase("Exit_Victory")]
        public void TrueWhenMapHasExit(string exit)
        {
            data.Triggers.Add(new Trigger(0, 0, 0, exit));

            Assert.That(_target.Passes(data), Is.True);

        }

        [Test]
        public void FalseWhenMapHasNoExit()
        {
            Assert.That(_target.Passes(data), Is.False);
        }
    }
}