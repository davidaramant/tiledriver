using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom.SquareModel;

public sealed class SquareSegmentsExtensionsTests
{
    [Theory]
    [InlineData(SquareSegment.LowerLeftInner, SquareSegments.LowerLeftInner)]
    [InlineData(SquareSegment.LowerLeftOuter, SquareSegments.LowerLeftOuter)]
    [InlineData(SquareSegment.LowerRightInner, SquareSegments.LowerRightInner)]
    [InlineData(SquareSegment.LowerRightOuter, SquareSegments.LowerRightOuter)]
    [InlineData(SquareSegment.UpperLeftInner, SquareSegments.UpperLeftInner)]
    [InlineData(SquareSegment.UpperLeftOuter, SquareSegments.UpperLeftOuter)]
    [InlineData(SquareSegment.UpperRightInner, SquareSegments.UpperRightInner)]
    [InlineData(SquareSegment.UpperRightOuter, SquareSegments.UpperRightOuter)]
    public void ShouldConvertSquareSegmentEnumToSegments(SquareSegment segment, SquareSegments expectedSegments) =>
        segment.ToSquareSegments().Should().Be(expectedSegments);
}
