// Copyright (c) 2017, Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.MapRanker;

namespace Tiledriver.Core.Tests.MapRanker
{
    [TestFixture()]
    public class StartPositionValidTest
    {
        private StartPositionValid _target;

        [SetUp]
        public void Setup()
        {
            _target = new StartPositionValid();
        }

        [Test]
        public void TrueWhenStartPositionInNegativePlane()
        {
            var data = DemoMap.Create();

            Assert.That(_target.Passes(data), Is.True);
        }

        [Test]
        public void FalseWhenStartPositionMapsToNonNegativeTile()
        {
            var data = DemoMap.Create();
            var startTile = data.PlaneMaps.Single().TileSpaces[data.Width + 1];
            startTile.Tile = 1;

            Assert.That(_target.Passes(data), Is.False);
        }
    }
}