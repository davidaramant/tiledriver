// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    [TestFixture]
    public sealed class TileMappingsTests
    {
        [Test]
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

            Assert.That(tm1.AmbushModzones, Has.Count.EqualTo(4));
        }

        [Test]
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

            Assert.That(tm1.ChangeTriggerModzones, Has.Count.EqualTo(4));
        }

        [Test]
        public void ShouldMergeTileMappings()
        {
            var tm1 = Create(
                tileTemplates: new []
                {
                    new TileTemplate(oldNum:1,textureEast:"T1",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                    new TileTemplate(oldNum:2,textureEast:"T2",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                });

            var tm2 = Create(
                tileTemplates: new []
                {
                    new TileTemplate(oldNum:2,textureEast:"T2new",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                    new TileTemplate(oldNum:3,textureEast:"T3",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever"),
                });

            tm1.Add(tm2);

            Assert.That(tm1.TileTemplates,Has.Count.EqualTo(4));
        }

        [Test]
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

            Assert.That(tm1.TriggerTemplates, Has.Count.EqualTo(4));
        }

        [Test]
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

            Assert.That(tm1.ZoneTemplates, Has.Count.EqualTo(4));
        }

        private static void CheckMerge<T, TCompare>(
            Dictionary<ushort, T> dict,
            Func<T, TCompare> compareSelector,
            IEnumerable<TCompare> expected)
        {
            Assert.That(
                dict,
                Has.Count.EqualTo(3),
                "Unexpected number of entries.");

            Assert.That(
                dict.Keys,
                Is.EquivalentTo(new ushort[] { 1, 2, 3 }),
                "Unexpected keys.");

            Assert.That(
                dict.Values.Select(compareSelector),
                Is.EquivalentTo(expected),
                "Unexpected values.");
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