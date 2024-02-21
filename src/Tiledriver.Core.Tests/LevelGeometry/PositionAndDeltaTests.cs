// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry;

public sealed class PositionAndDeltaTests
{
	[Fact]
	public void ShouldCreateDelta()
	{
		var p1 = new Position(1, 2);
		var p2 = new Position(3, 4);

		var delta = p2 - p1;
		var p3 = p1 + delta;

		p3.Should().Be(p2);
	}

	[Fact]
	public void ShouldManipulateDelta()
	{
		var d1 = new PositionDelta(10, 12);
		var d2 = new PositionDelta(5, 6);

		var d3 = d1 - d2;

		d3.Should().Be(d2);
	}
}
