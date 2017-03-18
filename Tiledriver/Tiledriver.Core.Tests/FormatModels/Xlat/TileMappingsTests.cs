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
        public void ShouldMergeAmbushModzoneMappings()
        {
            var tm1 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>
                {
                    {1,new AmbushModzone(fillzone:false) },
                    {2,new AmbushModzone(fillzone:false) },
                },
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            var tm2 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>
                {
                    {2,new AmbushModzone(fillzone:true) },
                    {3,new AmbushModzone(fillzone:true) },
                },
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            tm1.Add(tm2);

            CheckMerge(
                tm1.AmbushModzones,
                amz => amz.Fillzone,
                new[] { false, true, true });
        }

        [Test]
        public void ShouldMergeChangeTriggerModzoneMappings()
        {
            var tm1 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>
                {
                    {1,new ChangeTriggerModzone("A1",new TriggerTemplate()) },
                    {2,new ChangeTriggerModzone("A2",new TriggerTemplate()) },
                },
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            var tm2 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>
                {
                    {2,new ChangeTriggerModzone("A2new",new TriggerTemplate()) },
                    {3,new ChangeTriggerModzone("A3",new TriggerTemplate()) },
                },
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            tm1.Add(tm2);

            CheckMerge(
                tm1.ChangeTriggerModzones,
                ctmz => ctmz.Action,
                new[] { "A1", "A2new", "A3" });
        }

        [Test]
        public void ShouldMergeTileMappings()
        {
            var tm1 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>
                {
                    {1,new Tile(textureEast:"T1",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever") },
                    {2,new Tile(textureEast:"T2",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever") },
                },
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            var tm2 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>
                {
                    {2,new Tile(textureEast:"T2new",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever") },
                    {3,new Tile(textureEast:"T3",textureNorth:"whatever",textureSouth:"whatever",textureWest:"whatever") },
                },
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>());

            tm1.Add(tm2);

            CheckMerge(
                tm1.Tiles,
                tile => tile.TextureEast,
                new[] { "T1", "T2new", "T3" });
        }

        [Test]
        public void ShouldMergePositionlessTriggerMappings()
        {
            var tm1 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>
                {
                    { 1, new TriggerTemplate(oldNum:1,action:"Action1") },
                    { 2, new TriggerTemplate(oldNum:2,action:"Action2") }
                },
                zones: new Dictionary<ushort, Zone>());

            var tm2 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>
                {
                    { 2, new TriggerTemplate(oldNum:2,action:"ActionNew2") },
                    { 3, new TriggerTemplate(oldNum:3,action:"Action3") }
                },
                zones: new Dictionary<ushort, Zone>());

            tm1.Add(tm2);

            CheckMerge(
                tm1.TriggerTemplates,
                trigger => trigger.Action,
                new[] { "Action1", "ActionNew2", "Action3" });
        }

        [Test]
        public void ShouldMergeZoneMappings()
        {
            var tm1 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>
                {
                    {1,new Zone(comment:"C1") },
                    {2,new Zone(comment:"C2") },
                });

            var tm2 = new TileMappings(
                ambushModzones: new Dictionary<ushort, AmbushModzone>(),
                changeTriggerModzones: new Dictionary<ushort, ChangeTriggerModzone>(),
                tiles: new Dictionary<ushort, Tile>(),
                triggerTemplates: new Dictionary<ushort, TriggerTemplate>(),
                zones: new Dictionary<ushort, Zone>
                {
                    {2,new Zone(comment:"C2new") },
                    {3,new Zone(comment:"C3") },
                });

            tm1.Add(tm2);

            CheckMerge(
                tm1.Zones,
                zone => zone.Comment,
                new[] { "C1", "C2new", "C3" });
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
    }
}