// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using System;
using Xunit;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry;
using System.Linq;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

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

    [Fact]
    public void ShouldSimplifyVerticalEdges()
    {
        var graph = SectorEdgeGraph.FromEdges(new[] {
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.VerticalTop),
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.VerticalBottom),
            BuildEdge(
                new Position(0, 1),
                EdgeSegmentId.VerticalTop),
            BuildEdge(
                new Position(0, 1),
                EdgeSegmentId.VerticalBottom)});

        var simpleGraph = graph.Simplify();

        simpleGraph.EdgeCount.Should().Be(1);
        var edge = simpleGraph.GetAllEdges().First();
        edge.Start.Should().Be(new LatticePoint(new Position(0, 1), SquarePoint.BottomMiddle));
        edge.End.Should().Be(new LatticePoint(new Position(0, 0), SquarePoint.TopMiddle));
    }

    [Fact]
    public void ShouldSimplifyPositiveSlopedEdges()
    {
        var graph = SectorEdgeGraph.FromEdges(new[] {
            BuildEdge(
                new Position(0, 0),
                EdgeSegmentId.DiagTopRight),
            BuildEdge(
                new Position(1, 0),
                EdgeSegmentId.DiagBottomLeft),
            BuildEdge(
                new Position(1, 1),
                EdgeSegmentId.DiagTopRight),
            BuildEdge(
                new Position(2, 1),
                EdgeSegmentId.DiagBottomLeft)});

        var simpleGraph = graph.Simplify();

        simpleGraph.EdgeCount.Should().Be(1);
        var edge = simpleGraph.GetAllEdges().First();
        edge.Start.Should().Be(new LatticePoint(new Position(0, 0), SquarePoint.TopMiddle));
        edge.End.Should().Be(new LatticePoint(new Position(2, 1), SquarePoint.BottomMiddle));
    }

    private static SectorEdge BuildEdge(Position square, EdgeSegmentId id) =>
        throw new NotImplementedException();
        //SectorEdge.FromPosition(
        //        square,
        //        new EdgeSegment(
        //            id,
        //            new SectorDescription(1),
        //            SectorDescription.OutsideLevel,
        //            IsFrontTopOrLeft: false));
}
