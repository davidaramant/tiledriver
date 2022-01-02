// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class EdgeNodeTests
{
    static TheoryData<EdgeSegment, Position> MoveRightWhenTopOrLeftIsFront() =>
        MoveRight(front: new SectorDescription(1), back: SectorDescription.OutsideLevel, isFrontTopOrLeft: true);

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsFront))]
    public void ShouldMoveRightWhenTopOrLeftIsFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: true);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveLeftWhenTopOrLeftIsFront() => 
        MoveLeft(front: new SectorDescription(1), back: SectorDescription.OutsideLevel, isFrontTopOrLeft: true);

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: false);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveLeftWhenTopOrLeftIsNotFront() => 
        MoveLeft(front: new SectorDescription(1), back: SectorDescription.OutsideLevel, isFrontTopOrLeft: false);

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsNotFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsNotFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: true);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveRightWhenTopOrLeftIsNotFront() => 
        MoveRight(front: new SectorDescription(1), back: SectorDescription.OutsideLevel, isFrontTopOrLeft: false);

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsNotFront))]
    public void ShouldMoveRightWhenTopOrLeftIsNotFront(EdgeSegment edge, Position expectedPosition)
    {
        var node = new EdgeNode(Position.Origin, edge);
        var newNode = node.FollowLine(goRight: false);

        newNode.Segment.Id.Should().Be(edge.Id.FollowEdge());
        newNode.Square.Should().Be(expectedPosition);
    }

    static TheoryData<EdgeSegment, Position> MoveRight(SectorDescription front, SectorDescription back, bool isFrontTopOrLeft)
    {
        return new()
        {
            {
                new EdgeSegment(EdgeSegmentId.DiagTopLeft, front, back, isFrontTopOrLeft),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomRight, front, back, isFrontTopOrLeft),
                new Position(0, 1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagTopRight, front, back, isFrontTopOrLeft),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomLeft, front, back, isFrontTopOrLeft),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalLeft, front, back, isFrontTopOrLeft),
                new Position(-1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalRight, front, back, isFrontTopOrLeft),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalTop, front, back, isFrontTopOrLeft),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalBottom, front, back, isFrontTopOrLeft),
                new Position(0, 1)
            },
        };
    }

    static TheoryData<EdgeSegment, Position> MoveLeft(SectorDescription front, SectorDescription back, bool isFrontTopOrLeft)
    {
        return new()
        {
            {
                new EdgeSegment(EdgeSegmentId.DiagTopLeft, front, back, isFrontTopOrLeft),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomRight, front, back, isFrontTopOrLeft),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagTopRight, front, back, isFrontTopOrLeft),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.DiagBottomLeft, front, back, isFrontTopOrLeft),
                new Position(0, 1)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalLeft, front, back, isFrontTopOrLeft),
                new Position(0, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.HorizontalRight, front, back, isFrontTopOrLeft),
                new Position(1, 0)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalTop, front, back, isFrontTopOrLeft),
                new Position(0, -1)
            },
            {
                new EdgeSegment(EdgeSegmentId.VerticalBottom, front, back, isFrontTopOrLeft),
                new Position(0, 0)
            },
        };
    }
}
