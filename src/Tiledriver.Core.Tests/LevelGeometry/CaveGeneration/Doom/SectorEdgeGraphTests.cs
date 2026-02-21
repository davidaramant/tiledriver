using Shouldly;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class SectorEdgeGraphTests
{
	[Fact]
	public void ShouldConnectTwoEdges()
	{
		var graph = SectorEdgeGraph.FromEdges([
			BuildEdge(
				Position.Origin,
				EdgeSegmentId.DiagBottomRight,
				left: SquarePoint.BottomMiddle,
				right: SquarePoint.RightMiddle
			),
			BuildEdge(
				new Position(1, 0),
				EdgeSegmentId.DiagTopLeft,
				left: SquarePoint.LeftMiddle,
				right: SquarePoint.TopMiddle
			),
		]);

		graph.EdgeCount.ShouldBe(2);
		graph.GetEdgesConnectedTo(new LatticePoint(Position.Origin, SquarePoint.RightMiddle)).Count().ShouldBe(2);
	}

	[Fact]
	public void ShouldSimplifyHorizontalEdges()
	{
		var graph = SectorEdgeGraph.FromEdges([
			BuildEdge(
				new Position(0, 0),
				EdgeSegmentId.HorizontalLeft,
				left: SquarePoint.LeftMiddle,
				right: SquarePoint.Center
			),
			BuildEdge(
				new Position(0, 0),
				EdgeSegmentId.HorizontalRight,
				left: SquarePoint.Center,
				right: SquarePoint.RightMiddle
			),
			BuildEdge(
				new Position(1, 0),
				EdgeSegmentId.HorizontalLeft,
				left: SquarePoint.LeftMiddle,
				right: SquarePoint.Center
			),
			BuildEdge(
				new Position(1, 0),
				EdgeSegmentId.HorizontalRight,
				left: SquarePoint.Center,
				right: SquarePoint.RightMiddle
			),
		]);

		var simpleGraph = graph.Simplify();

		simpleGraph.EdgeCount.ShouldBe(1);
		var edge = simpleGraph.GetAllEdges().First();
		edge.Start.ShouldBe(new LatticePoint(new Position(0, 0), SquarePoint.LeftMiddle));
		edge.End.ShouldBe(new LatticePoint(new Position(1, 0), SquarePoint.RightMiddle));
	}

	[Fact]
	public void ShouldSimplifyVerticalEdges()
	{
		var graph = SectorEdgeGraph.FromEdges([
			BuildEdge(
				new Position(0, 0),
				EdgeSegmentId.VerticalTop,
				left: SquarePoint.TopMiddle,
				right: SquarePoint.Center
			),
			BuildEdge(
				new Position(0, 0),
				EdgeSegmentId.VerticalBottom,
				left: SquarePoint.Center,
				right: SquarePoint.BottomMiddle
			),
			BuildEdge(
				new Position(0, -1),
				EdgeSegmentId.VerticalTop,
				left: SquarePoint.TopMiddle,
				right: SquarePoint.Center
			),
			BuildEdge(
				new Position(0, -1),
				EdgeSegmentId.VerticalBottom,
				left: SquarePoint.Center,
				right: SquarePoint.BottomMiddle
			),
		]);

		var simpleGraph = graph.Simplify();

		simpleGraph.EdgeCount.ShouldBe(1);
		var edge = simpleGraph.GetAllEdges().First();
		edge.Start.ShouldBe(new LatticePoint(new Position(0, 0), SquarePoint.TopMiddle));
		edge.End.ShouldBe(new LatticePoint(new Position(0, -1), SquarePoint.BottomMiddle));
	}

	[Fact]
	public void ShouldSimplifyPositiveSlopedEdges()
	{
		var graph = SectorEdgeGraph.FromEdges([
			BuildEdge(
				new Position(0, 0),
				EdgeSegmentId.DiagTopLeft,
				left: SquarePoint.LeftMiddle,
				right: SquarePoint.TopMiddle
			),
			BuildEdge(
				new Position(0, 1),
				EdgeSegmentId.DiagBottomRight,
				left: SquarePoint.BottomMiddle,
				right: SquarePoint.RightMiddle
			),
			BuildEdge(
				new Position(1, 1),
				EdgeSegmentId.DiagTopLeft,
				left: SquarePoint.LeftMiddle,
				right: SquarePoint.TopMiddle
			),
			BuildEdge(
				new Position(1, 2),
				EdgeSegmentId.DiagBottomRight,
				left: SquarePoint.BottomMiddle,
				right: SquarePoint.RightMiddle
			),
		]);

		var simpleGraph = graph.Simplify();

		simpleGraph.EdgeCount.ShouldBe(1);
		var edge = simpleGraph.GetAllEdges().First();
		edge.Start.ShouldBe(new LatticePoint(new Position(0, 0), SquarePoint.LeftMiddle));
		edge.End.ShouldBe(new LatticePoint(new Position(1, 2), SquarePoint.RightMiddle));
	}

	private static SectorEdge BuildEdge(Position square, EdgeSegmentId id, SquarePoint left, SquarePoint right) =>
		SectorEdge.FromPosition(
			square,
			new EdgeSegment(id, new SectorDescription(1), SectorDescription.OutsideLevel, Left: left, Right: right)
		);
}
