// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Xunit;

namespace Tiledriver.Core.Tests.Utils.ConnectedComponentLabeling
{
    public class ConnectedAreaTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ShouldTrimArea(int border)
        {
            var area = new ConnectedArea(new Position[]
            {
                new(11, 11),
                new(12, 11),
                new(12, 12)
            });

            var (trimmed, dimensions) = area.TrimExcess(border);

            dimensions.Should().Be(new Size(2 + 2 * border, 2 + 2 * border));
            trimmed.Should().BeEquivalentTo(new Position[] {
                new(0 + border, 0 + border),
                new(1 + border, 0 + border),
                new(1 + border, 1 + border) });
        }

        [Fact]
        public void ShouldMovePointsOutWhenNewBorderIsLarger()
        {
            var area = new ConnectedArea(new Position[]
            {
                new(0, 0),
                new(1, 0),
                new(1, 1)
            });

            var (trimmed, dimensions) = area.TrimExcess(2);

            dimensions.Should().Be(new Size(6, 6));
            trimmed.Should().BeEquivalentTo(new Position[] {
                new(2, 2),
                new(3, 2),
                new(3, 3) });
        }
    }
}