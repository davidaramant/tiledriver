// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class InternalEdgeTests
{
    static TheoryData<InternalEdge, Position> MoveRightWhenTopOrLeftIsFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: true);

        return MoveRight(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsFront))]
    public void ShouldMoveRightWhenTopOrLeftIsFront(InternalEdge edge, Position expectedPosition)
    {
        var (newPosition, newEdge) = edge.FollowLine(goRight: true, Position.Origin);

        newEdge.Id.Should().Be(edge.Id.FollowEdge());
        newPosition.Should().Be(expectedPosition);
    }

    static TheoryData<InternalEdge, Position> MoveLeftWhenTopOrLeftIsFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: true);

        return MoveLeft(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsFront(InternalEdge edge, Position expectedPosition)
    {
        var (newPosition, newEdge) = edge.FollowLine(goRight: false, Position.Origin);

        newEdge.Id.Should().Be(edge.Id.FollowEdge());
        newPosition.Should().Be(expectedPosition);
    }

    static TheoryData<InternalEdge, Position> MoveLeftWhenTopOrLeftIsNotFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: false);

        return MoveLeft(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveLeftWhenTopOrLeftIsNotFront))]
    public void ShouldMoveLeftWhenTopOrLeftIsNotFront(InternalEdge edge, Position expectedPosition)
    {
        var (newPosition, newEdge) = edge.FollowLine(goRight: true, Position.Origin);

        newEdge.Id.Should().Be(edge.Id.FollowEdge());
        newPosition.Should().Be(expectedPosition);
    }

    static TheoryData<InternalEdge, Position> MoveRightWhenTopOrLeftIsNotFront()
    {
        var edgeInfo = new EdgeInfo(Front: new SectorDescription(1), Back: SectorDescription.OutsideLevel, IsFrontTopOrLeft: false);

        return MoveRight(edgeInfo);
    }

    [Theory]
    [MemberData(nameof(MoveRightWhenTopOrLeftIsNotFront))]
    public void ShouldMoveRightWhenTopOrLeftIsNotFront(InternalEdge edge, Position expectedPosition)
    {
        var (newPosition, newEdge) = edge.FollowLine(goRight: false, Position.Origin);

        newEdge.Id.Should().Be(edge.Id.FollowEdge());
        newPosition.Should().Be(expectedPosition);
    }

    static TheoryData<InternalEdge, Position> MoveRight(EdgeInfo edgeInfo)
    {
        return new()
        {
            {
                new InternalEdge(InternalEdgeId.DiagTopLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.DiagBottomRight, edgeInfo),
                new Position(0, 1)
            },
            {
                new InternalEdge(InternalEdgeId.DiagTopRight, edgeInfo),
                new Position(0, -1)
            },
            {
                new InternalEdge(InternalEdgeId.DiagBottomLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.HorizontalLeft, edgeInfo),
                new Position(-1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.HorizontalRight, edgeInfo),
                new Position(0, 0)
            },
            {
                new InternalEdge(InternalEdgeId.VerticalTop, edgeInfo),
                new Position(0, 0)
            },
            {
                new InternalEdge(InternalEdgeId.VerticalBottom, edgeInfo),
                new Position(0, 1)
            },
        };
    }

    static TheoryData<InternalEdge, Position> MoveLeft(EdgeInfo edgeInfo)
    {
        return new()
        {
            {
                new InternalEdge(InternalEdgeId.DiagTopLeft, edgeInfo),
                new Position(0, -1)
            },
            {
                new InternalEdge(InternalEdgeId.DiagBottomRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.DiagTopRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.DiagBottomLeft, edgeInfo),
                new Position(0, 1)
            },
            {
                new InternalEdge(InternalEdgeId.HorizontalLeft, edgeInfo),
                new Position(0, 0)
            },
            {
                new InternalEdge(InternalEdgeId.HorizontalRight, edgeInfo),
                new Position(1, 0)
            },
            {
                new InternalEdge(InternalEdgeId.VerticalTop, edgeInfo),
                new Position(0, -1)
            },
            {
                new InternalEdge(InternalEdgeId.VerticalBottom, edgeInfo),
                new Position(0, 0)
            },
        };
    }
}
