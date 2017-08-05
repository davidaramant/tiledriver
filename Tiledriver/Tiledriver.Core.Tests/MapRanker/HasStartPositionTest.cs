// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.MapRanker;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests.MapRanker
{
    [TestFixture]
    public class HasStartPositionTest
    {
        private readonly HasStartPosition _target;

        public HasStartPositionTest()
        {
            _target = new HasStartPosition();
        }

        [Test]
        public void TrueWhenMapHasStartPosition()
        {
            var data = DemoMap.Create();

            Assert.That(_target.Passes(data), Is.True);

        }

        [Test]
        public void FalseWhenMapIsMissingStartPosition()
        {
            var data = DemoMap.Create();
            data.Things.Remove(data.Things.Single(t => t.Type == Actor.Player1Start.ClassName));

            Assert.That(_target.Passes(data), Is.False);
        }

        [Test]
        public void FalseWhenMapHasMoreThanOneStartPosition()
        {
            var data = DemoMap.Create();
            var newThing = new Thing(Actor.Player1Start.ClassName, 23.5, 45.5, 0, 0);
            data.Things.Add(newThing);

            Assert.That(_target.Passes(data), Is.False);
        }
    }
}