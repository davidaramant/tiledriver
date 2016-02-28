using System.Drawing;
using NUnit.Framework;
using Tiledriver.Generator;

namespace Tiledriver.Tests.Generator
{
    [TestFixture]
    public sealed class RectangleExtensionsTests
    {
        [Test, Combinatorial]
        public void ShouldReturnNullIfOtherRectangleIsNotInPlane([Values(-10, 10)]int xDelta, [Values(-10, 10)]int yDelta)
        {
            var source = new Rectangle(x: 10, y: 10, width: 10, height: 10);

            var other = new Rectangle(x: source.X + xDelta, y: source.Y + yDelta, width: 10, height: 10);
            Assert.That(source.StraightDistanceFrom(other), Is.Null,
                $"X-Delta: {xDelta}, Y-Delta: {yDelta}");
        }

        [Test]
        public void ShouldReturnDistanceToRectangleOnTheLeft([Values(-4, 0, 4)]int yDelta)
        {
            var source = new Rectangle(x: 10, y: 10, width: 10, height: 10);

            var other = new Rectangle(x: 0, y: source.Y + yDelta, width: 5, height: 10);

            Assert.That(source.StraightDistanceFrom(other), Is.EqualTo(source.Left - other.Right), $"Y-Delta: {yDelta}");
        }

        [Test]
        public void ShouldReturnDistanceToRectangleOnTheRight([Values(-4, 0, 4)]int yDelta)
        {
            var source = new Rectangle(x: 10, y: 10, width: 10, height: 10);

            var other = new Rectangle(x: 25, y: source.Y + yDelta, width: 5, height: 10);

            Assert.That(source.StraightDistanceFrom(other), Is.EqualTo(other.Right - other.Left), $"Y-Delta: {yDelta}");
        }

        [Test]
        public void ShouldReturnDistanceToRectangleOnTheTop([Values(-4, 0, 4)]int xDelta)
        {
            var source = new Rectangle(x: 10, y: 10, width: 10, height: 10);

            var other = new Rectangle(x: source.X + xDelta, y: 0, width: 10, height: 5);

            Assert.That(source.StraightDistanceFrom(other), Is.EqualTo(source.Top - other.Bottom), $"X-Delta: {xDelta}");
        }

        [Test]
        public void ShouldReturnDistanceToRectangleOnTheBottom([Values(-5, 0, 5)]int xDelta)
        {
            var source = new Rectangle(x: 10, y: 10, width: 10, height: 10);

            var other = new Rectangle(x: source.X + xDelta, y: 25, width: 10, height: 5);

            Assert.That(source.StraightDistanceFrom(other), Is.EqualTo(other.Top - source.Bottom), $"X-Delta: {xDelta}");
        }
    }
}
