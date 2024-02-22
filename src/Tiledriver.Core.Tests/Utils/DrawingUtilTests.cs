// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Utils;
using Xunit;

namespace Tiledriver.Core.Tests.Utils;

public sealed class DrawingUtilTests
{
	public sealed class BresenhamTestData : TheoryData<Position, IEnumerable<Position>>
	{
		public BresenhamTestData()
		{
			// Right, left, Up, Down
			Add(new Position(4, 2), new Position[] { new(2, 2), new(3, 2), new(4, 2) });
			Add(new Position(0, 2), new Position[] { new(2, 2), new(1, 2), new(0, 2) });
			Add(new Position(2, 0), new Position[] { new(2, 2), new(2, 1), new(2, 0) });
			Add(new Position(2, 4), new Position[] { new(2, 2), new(2, 3), new(2, 4) });

			// UpRight, UpLeft, DownLeft, DownRight
			Add(new Position(4, 0), new Position[] { new(2, 2), new(3, 1), new(4, 0) });
			Add(new Position(0, 0), new Position[] { new(2, 2), new(1, 1), new(0, 0) });
			Add(new Position(0, 4), new Position[] { new(2, 2), new(1, 3), new(0, 4) });
			Add(new Position(4, 4), new Position[] { new(2, 2), new(3, 3), new(4, 4) });
		}
	}

	[Theory]
	[ClassData(typeof(BresenhamTestData))]
	public void ShouldDrawLineFromStartToEnd(Position destination, IEnumerable<Position> expected)
	{
		var center = new Position(2, 2);

		DrawingUtil.BresenhamLine(center, destination).Should().BeEquivalentTo(expected);
	}
}
