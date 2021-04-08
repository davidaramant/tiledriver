// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    public sealed class TileMappingsTests
    {
        [Fact]
        public void ShouldAppendAmbushModzoneMappings()
        {
            var tm1 = Create(
                ambushModzones: new[]
                {
                    new AmbushModzone(oldNum:1,fillzone:false),
                    new AmbushModzone(oldNum:2,fillzone:false),
                });

            var tm2 = Create(
                ambushModzones: new[]
                {
                    new AmbushModzone(oldNum:2,fillzone:true),
                    new AmbushModzone(oldNum:3,fillzone:true),
                });

            tm1.Add(tm2);

            tm1.AmbushModzones.Should().HaveCount(4);
        }

        [Fact]
        public void ShouldAppendChangeTriggerModzoneMappings()
        {
            var tm1 = Create(
                changeTriggerModzones: new[]
                {
                   new ChangeTriggerModzone(1,"A1",new TriggerTemplate()),
                   new ChangeTriggerModzone(2,"A2",new TriggerTemplate()),
                });

            var tm2 = Create(
                changeTriggerModzones: new[]
                {
                    new ChangeTriggerModzone(2,"A2new",new TriggerTemplate()),
                    new ChangeTriggerModzone(3, "A3",new TriggerTemplate()),
                });

            tm1.Add(tm2);

            tm1.ChangeTriggerModzones.Should().HaveCount(4);
        }

        [Fact]
        public void ShouldMergeTileMappings()
        {
            var tm1 = Create(
                tileTemplates: new[]
                {
                    new TileTemplate(oldNum:1,textureEast:"T1",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                    new TileTemplate(oldNum:2,textureEast:"T2",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                });

            var tm2 = Create(
                tileTemplates: new[]
                {
                    new TileTemplate(oldNum:2,textureEast:"T2new",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                    new TileTemplate(oldNum:3,textureEast:"T3",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                });

            tm1.Add(tm2);

            tm1.TileTemplates.Should().HaveCount(4);
        }

        [Fact]
        public void ShouldMergeTriggerTemplateMappings()
        {
            var tm1 = Create(
                triggerTemplates: new[]
                {
                    new TriggerTemplate(oldNum:1,action:"Action1"),
                    new TriggerTemplate(oldNum:2,action:"Action2")
                });

            var tm2 = Create(
                triggerTemplates: new[]
                {
                    new TriggerTemplate(oldNum:2,action:"ActionNew2"),
                    new TriggerTemplate(oldNum:3,action:"Action3")
                });

            tm1.Add(tm2);

            tm1.TriggerTemplates.Should().HaveCount(4);
        }

        [Fact]
        public void ShouldMergeZoneMappings()
        {
            var tm1 = Create(
                zonesTemplates: new[]
                {
                    new ZoneTemplate(oldNum:1,comment:"C1"),
                    new ZoneTemplate(oldNum:2,comment:"C2"),
                });

            var tm2 = Create(
                zonesTemplates: new[]
                {
                    new ZoneTemplate(oldNum:2,comment:"C2new"),
                    new ZoneTemplate(oldNum:3,comment:"C3"),
                });

            tm1.Add(tm2);

            tm1.ZoneTemplates.Should().HaveCount(4);
        }

        private static void CheckMerge<T, TCompare>(
            Dictionary<ushort, T> dict,
            Func<T, TCompare> compareSelector,
            IEnumerable<TCompare> expected)
        {
            dict.Should().HaveCount(3);

            dict.Keys.Should().BeEquivalentTo(new ushort[] { 1, 2, 3 });

            dict.Values.Select(compareSelector).Should().BeEquivalentTo(expected);
        }

        private static TileMappings Create(
            IEnumerable<AmbushModzone> ambushModzones = null,
            IEnumerable<ChangeTriggerModzone> changeTriggerModzones = null,
            IEnumerable<TileTemplate> tileTemplates = null,
            IEnumerable<TriggerTemplate> triggerTemplates = null,
            IEnumerable<ZoneTemplate> zonesTemplates = null)
        {
            return new TileMappings(
                ambushModzones: ambushModzones ?? Enumerable.Empty<AmbushModzone>(),
                changeTriggerModzones: changeTriggerModzones ?? Enumerable.Empty<ChangeTriggerModzone>(),
                tileTemplates: tileTemplates ?? Enumerable.Empty<TileTemplate>(),
                triggerTemplates: triggerTemplates ?? Enumerable.Empty<TriggerTemplate>(),
                zoneTemplates: zonesTemplates ?? Enumerable.Empty<ZoneTemplate>());
        }
    }
}