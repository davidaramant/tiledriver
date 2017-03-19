// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.Extensions;
using Tiledriver.Core.FormatModels.Common;

namespace Tiledriver.Core.Tests.Extensions
{
    [TestFixture]
    public sealed class DirectionExtensionTests
    {
        [TestCase(0, 0, 3, 3, 2, TestName = "Top Left")]
        [TestCase(1, 0, 3, 3, 3, TestName = "Top Middle")]
        [TestCase(2, 0, 3, 3, 2, TestName = "Top Right")]
        [TestCase(0, 1, 3, 3, 3, TestName = "Middle Left")]
        [TestCase(1, 1, 3, 3, 4, TestName = "Middle Middle")]
        [TestCase(2, 1, 3, 3, 3, TestName = "Middle Right")]
        [TestCase(0, 2, 3, 3, 2, TestName = "Bottom Left")]
        [TestCase(1, 2, 3, 3, 3, TestName = "Bottom Middle")]
        [TestCase(2, 2, 3, 3, 2, TestName = "Bottom Right")]
        public void ShouldReturnValidAdjacentPoints(
            int x, int y,
            int width, int height,
            int expectedAdjacent)
        {
            var location = new Point(x, y);
            var bounds = new Size(width, height);

            Assert.That(location.GetAdjacentPoints(bounds).ToArray(), Has.Length.EqualTo(expectedAdjacent));
        }

        [Test]
        public void ShouldReturnPointsInDefaultOrder()
        {
            VerifyDirections(
                clockWise: false, 
                start: Direction.East, 
                expectedDirections: new[] { Direction.East, Direction.North, Direction.West, Direction.South, });
        }

        [Test]
        public void ShouldReturnPointsInReversedOrder()
        {
            VerifyDirections(
                clockWise: true,
                start: Direction.West,
                expectedDirections: new[] { Direction.West, Direction.North, Direction.East, Direction.South, });
        }

        [Test]
        public void ShouldGetDirectionsInCounterClockwiseOrder()
        {
            var actual = DirectionExtensions.GetDirections(start: Direction.North,clockWise:false).ToArray();
            var expected = new[] {Direction.North, Direction.West, Direction.South, Direction.East,};

            Assert.That(actual,Is.EqualTo(expected));
        }

        [Test]
        public void ShouldGetDirectionsInClockwiseOrder()
        {
            var actual = DirectionExtensions.GetDirections(start: Direction.North, clockWise: true).ToArray();
            var expected = new[] { Direction.North, Direction.East, Direction.South, Direction.West, };

            Assert.That(actual, Is.EqualTo(expected));
        }

        private static void VerifyDirections(bool clockWise, Direction start, IEnumerable<Direction> expectedDirections)
        {
            var directions =
                new Point(1, 1).
                    GetAdjacentPoints(new Size(3, 3), clockWise: clockWise, start: start).
                    Select(tuple => tuple.direction).
                    ToArray();

            Assert.That(
                directions,
                Is.EqualTo(expectedDirections.ToArray()),
                "Did not return correct directions.");
        }
    }
}
