// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class EdgeNodeTests
{
    static TheoryData<EdgeSegment, Position> MoveRightWhenTopOrLeftIsFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: true);

        return MoveRight(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsFront))]
    public void ShouldMoveRightWhenTopOrLeftIsFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: true);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveLeftWhenTopOrLeftIsFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: true);

        return MoveLeft(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: false);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveLeftWhenTopOrLeftIsNotFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: false);

        return MoveLeft(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsNotFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsNotFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: true);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveRightWhenTopOrLeftIsNotFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: false);

        return MoveRight(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsNotFront))]
    public void ShouldMoveRightWhenTopOrLeftIsNotFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: false);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveRight(EdgeInfo edgeInfo)
    {
        return new()
        {
            {
                new EdgeSegment(EdgeSegmentId.DiagTopLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomRight, edgeInfo),
                new Position(0, 1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagTopRight, edgeInfo),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalRight, edgeInfo),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalTop, edgeInfo),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalBottom, edgeInfo),
                new Position(0, 1)
            },
        };
    }

    static TheoryData<EdgeSegment, Position> MoveLeft(EdgeInfo edgeInfo)
    {
        return new()
        {
            {
                new EdgeSegment(EdgeSegmentId.DiagTopLeft, edgeInfo),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagTopRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomLeft, edgeInfo),
                new Position(0, 1)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalLeft, edgeInfo),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalTop, edgeInfo),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalBottom, edgeInfo),
                new Position(0, 0)
            },
        };
    }
}
