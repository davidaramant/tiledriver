// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    public sealed class MapTranslatorInfoTests
    {
        [Fact]
        public void ShouldOverrideSimpleEntries()
        {
            var info = CreateInfo(
                thingMappings: new IThingMapping[]
                {
                    new Elevator(1),
                    CreateThingTemplate(oldNum:1),
                });

            info.LookupThingMapping(1).Should().BeOfType<ThingTemplate>();
        }

        [Fact]
        public void ShouldExpandThingsWithAngles()
        {
            var info = CreateInfo(
                thingMappings: new IThingMapping[]
                {
                    CreateThingTemplate(oldNum:1, angles:8),
                });

            foreach (var oldNum in Enumerable.Range(1, 8).Select(i => (ushort)i))
            {
                info.LookupThingMapping(oldNum).Should().BeOfType<ThingTemplate>();
            }
        }

        [Fact]
        public void ShouldOverwriteExpandedThing()
        {
            var info = CreateInfo(
                thingMappings: new IThingMapping[]
                {
                    CreateThingTemplate(oldNum:1, angles:8),
                    new Elevator(1),
                });

            info.LookupThingMapping(1).Should().BeOfType<Elevator>();
            foreach (var oldNum in Enumerable.Range(2, 7).Select(i => (ushort)i))
            {
                info.LookupThingMapping(oldNum).Should().BeOfType<ThingTemplate>();
            }
        }

        private static MapTranslatorInfo CreateInfo(IEnumerable<IThingMapping> thingMappings = null)
        {
            return new MapTranslatorInfo(
                tileMappings: new TileMappings(),
                thingMappings: thingMappings ?? Enumerable.Empty<IThingMapping>(),
                flatMappings: new FlatMappings(),
                enableLightLevels: false);
        }

        private static ThingTemplate CreateThingTemplate(ushort oldNum, int angles = 0)
        {
            return new ThingTemplate(
                oldNum: oldNum,
                type: "Actor",
                angles: angles,
                holowall: false,
                pathing: false,
                ambush: false,
                minskill: 0);
        }
    }
}