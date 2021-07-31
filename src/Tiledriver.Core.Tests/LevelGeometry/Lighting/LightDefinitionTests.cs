// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.Lighting;

namespace Tiledriver.Core.Tests.LevelGeometry.Lighting
{
    public sealed class LightDefinitionTests
    {
        public sealed class LightDefinitionTestData : TheoryData<Position, LightHeight, (int Floor, int Ceiling)>
        {
            public LightDefinitionTestData()
            {
                // These are not really "tests"
                // It's really checking how the light falls off

                Add(new Position(3, 3), LightHeight.Middle, (6, 6));
                Add(new Position(3, 3), LightHeight.Floor, (6, 4));
                Add(new Position(3, 3), LightHeight.Ceiling, (4, 6));

                Add(new Position(2, 3), LightHeight.Middle, (3, 3));
                Add(new Position(2, 2), LightHeight.Middle, (1, 1));
            }
        }

        [Theory]
        [ClassData(typeof(LightDefinitionTestData))]
        public void ShouldCalculateBrightnessAtLocation(Position location, LightHeight height, (int Floor, int Ceiling) brightness)
        {
            var light = new LightDefinition(
                Center: new Position(3, 3),
                Brightness: 6,
                Radius: 2,
                Height: height);

            light.GetBrightness(location).Should().Be(brightness);
        }
    }
}