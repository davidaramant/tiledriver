using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tiledriver.Generator;
using Tiledriver.Generator.SimpleGeometry;

namespace Tiledriver.Tests.Generator.SimpleGeometry
{
    [TestFixture]
    public sealed class RoomNodeTests
    {
        [Test]
        public void ShouldFindValidEastStartingPoint([Values(0, 1, 2)] int seed)
        {
            var room = new RoomNode(bounds: new Rectangle(x: 39, y: 28, width: 12, height: 13), type: RoomType.Room);

            var random = new Random(seed);

            var result = room.GetStartingPointFacing(Direction.East, random);

            Assert.That(result.X, Is.EqualTo(room.Bounds.RightEdge()), "Should be on right edge.");
            Assert.That(result.Y, Is.AtLeast(room.Bounds.TopEdge()), "Point was too high.");
            Assert.That(result.Y, Is.AtMost(room.Bounds.BottomEdge()), "Point was too low.");
        }

        [Test]
        public void ShouldFindValidWestStartingPoint([Values(0, 1, 2)] int seed)
        {
            var room = new RoomNode(bounds: new Rectangle(x: 39, y: 28, width: 12, height: 13), type: RoomType.Room);

            var random = new Random(seed);

            var result = room.GetStartingPointFacing(Direction.West, random);

            Assert.That(result.X, Is.EqualTo(room.Bounds.LeftEdge()), "Should be on left edge.");
            Assert.That(result.Y, Is.AtLeast(room.Bounds.TopEdge()), "Point was too high.");
            Assert.That(result.Y, Is.AtMost(room.Bounds.BottomEdge()), "Point was too low.");
        }

        [Test]
        public void ShouldFindValidNorthStartingPoint([Values(0, 1, 2)] int seed)
        {
            var room = new RoomNode(bounds: new Rectangle(x: 39, y: 28, width: 12, height: 13), type: RoomType.Room);

            var random = new Random(seed);

            var result = room.GetStartingPointFacing(Direction.North, random);

            Assert.That(result.Y, Is.EqualTo(room.Bounds.TopEdge()), "Should be on top edge.");
            Assert.That(result.X, Is.AtLeast(room.Bounds.LeftEdge()), "Point was too far left.");
            Assert.That(result.X, Is.AtMost(room.Bounds.RightEdge()), "Point was too far right.");
        }


        [Test]
        public void ShouldFindValidSouthStartingPoint([Values(0, 1, 2)] int seed)
        {
            var room = new RoomNode(bounds: new Rectangle(x: 39, y: 28, width: 12, height: 13), type: RoomType.Room);

            var random = new Random(seed);

            var result = room.GetStartingPointFacing(Direction.South, random);

            Assert.That(result.Y, Is.EqualTo(room.Bounds.BottomEdge()), "Should be on bottom edge.");
            Assert.That(result.X, Is.AtLeast(room.Bounds.LeftEdge()), "Point was too far left.");
            Assert.That(result.X, Is.AtMost(room.Bounds.RightEdge()), "Point was too far right.");
        }
    }
}
