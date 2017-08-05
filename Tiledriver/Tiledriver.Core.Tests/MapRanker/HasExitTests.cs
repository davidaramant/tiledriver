// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.MapRanker;
using System.Collections.Generic;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests.MapRanker
{
    [TestFixture]
    public class HasExitTests
    {
        private HasExit _target;
        private MapData _data;

        [SetUp]
        public void FixtureSetup()
        {
            var exitTypes = new List<string>(new string[] { "Exit_Normal", "Exit_Secret", "Exit_VictorySpin", "Exit_Victory" });
            _data = DemoMap.Create();
            _data.Triggers.RemoveAll(t => exitTypes.Contains(t.Action));
            _data.Things.RemoveAll(t => t.Type == Actor.MechaHitler.ClassName);

            _target = new HasExit();
        }

        [TestCase("Exit_Normal")]
        [TestCase("Exit_Secret")]
        [TestCase("Exit_VictorySpin")]
        [TestCase("Exit_Victory")]
        public void TrueWhenMapHasExit(string exit)
        {
            _data.Triggers.Add(new Trigger(0, 0, 0, exit));

            Assert.That(_target.Passes(_data), Is.True);

        }

        [Test]
        public void FalseWhenMapHasNoExit()
        {
            Assert.That(_target.Passes(_data), Is.False);
        }

        [Test]
        public void TrueWhenMapHasMechaHitler()
        {
            _data.Things.Add(new Thing(Actor.MechaHitler.ClassName, 2, 2, 0, 0));
            Assert.That(_target.Passes(_data), Is.True);
        }
    }
}