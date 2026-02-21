using Shouldly;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class LatticePointTests
{
	[Fact]
	public void ShouldNormalizeBottomMiddlePoint()
	{
		var below = new LatticePoint(new Position(0, 0), SquarePoint.TopMiddle);
		var above = new LatticePoint(new Position(0, 1), SquarePoint.BottomMiddle);

		below.ShouldBe(above);
	}

	[Fact]
	public void ShouldNormalizeRightMiddlePoint()
	{
		var rightMiddleBefore = new LatticePoint(new Position(0, 0), SquarePoint.RightMiddle);
		var leftMiddleAfter = new LatticePoint(new Position(1, 0), SquarePoint.LeftMiddle);

		rightMiddleBefore.ShouldBe(leftMiddleAfter);
	}
}
