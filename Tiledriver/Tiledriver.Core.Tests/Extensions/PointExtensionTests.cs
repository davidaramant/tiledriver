// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.Extensions;

namespace Tiledriver.Core.Tests.Extensions
{
    [TestFixture]
    public sealed class PointExtensionTests
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
    }
}
