// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using Xunit;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry;
using System.Linq;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class SectorEdgeGraphTests
{
    [Fact]
    public void ShouldConnectTwoEdges()
    {
        var graph = SectorEdgeGraph.FromEdges(new[] {
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.DiagBottomRight),
            BuildEdge(
                new Position(0, 1),
                EdgeSegmentId.DiagTopLeft)});

        graph.EdgeCount.Should().Be(2);
        graph.GetEdgesConnectedTo(new LatticePoint(Position.Origin, SquarePoint.BottomMiddle)).Should().HaveCount(2);
    }

    [Fact]
    public void ShouldSimplifyHorizontalEdges()
    {
        var graph = SectorEdgeGraph.FromEdges(new[] {
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.HorizontalLeft),
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.HorizontalRight),
            BuildEdge(
                new Position(1, 0),
                EdgeSegmentId.HorizontalLeft),
            BuildEdge(
                new Position(1, 0),
                EdgeSegmentId.HorizontalRight)});

        var simpleGraph = graph.Simplify();

        simpleGraph.EdgeCount.Should().Be(1);
        var edge = simpleGraph.GetAllEdges().First();
        edge.Start.Should().Be(new LatticePoint(new Position(0, 0), SquarePoint.LeftMiddle));
        edge.End.Should().Be(new LatticePoint(new Position(1, 0), SquarePoint.RightMiddle));
    }

    private static SectorEdge BuildEdge(Position square, EdgeSegmentId id) =>
        SectorEdge.FromPosition(
                square,
                new EdgeSegment(
                    id,
                    new SectorDescription(1),
                    SectorDescription.OutsideLevel,
                    IsFrontTopOrLeft: false));
}
