// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.Tests.FormatModels.Xlat
{
    [TestFixture]
    public sealed class ThingMappingsTests
    {
        [Test]
        public void ShouldConcatElevatorMappings()
        {
            var tm1 = new ThingMappings(
                elevators: new ushort[] { 1, 2, 3 },
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>(),
                thingDefinitions: new Dictionary<ushort, ThingDefinition>());

            var tm2 = new ThingMappings(
                elevators: new ushort[] { 3, 4, 5 },
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>(),
                thingDefinitions: new Dictionary<ushort, ThingDefinition>());

            tm1.Add(tm2);

            Assert.That(
                tm1.Elevators,
                Is.EquivalentTo(new ushort[] { 1, 2, 3, 3, 4, 5 }),
                "Did not concatenate elevators.");
        }

        [Test]
        public void ShouldMergeTriggerMappings()
        {
            var tm1 = new ThingMappings(
                elevators: Enumerable.Empty<ushort>(),
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>
                {
                    { 1, new PositionlessTrigger(oldNum:1,action:"Action1") },
                    { 2, new PositionlessTrigger(oldNum:2,action:"Action2") }
                },
                thingDefinitions: new Dictionary<ushort, ThingDefinition>());

            var tm2 = new ThingMappings(
                elevators: Enumerable.Empty<ushort>(),
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>
                {
                    { 2, new PositionlessTrigger(oldNum:2,action:"ActionNew2") },
                    { 3, new PositionlessTrigger(oldNum:3,action:"Action3") }
                },
                thingDefinitions: new Dictionary<ushort, ThingDefinition>());

            tm1.Add(tm2);

            Assert.That(
                tm1.PositionlessTriggers,
                Has.Count.EqualTo(3),
                "Unexpected number of triggers.");

            Assert.That(
                tm1.PositionlessTriggers.Keys,
                Is.EquivalentTo(new ushort[] { 1, 2, 3 }),
                "Unexpected trigger keys.");

            Assert.That(
                tm1.PositionlessTriggers.Values.Select(t => t.Action),
                Is.EquivalentTo(new[] { "Action1", "ActionNew2", "Action3" }),
                "Unexpected trigger values.");
        }

        [Test]
        public void ShouldMergeThingMappings()
        {
            var tm1 = new ThingMappings(
                elevators: Enumerable.Empty<ushort>(),
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>(),
                thingDefinitions: new Dictionary<ushort, ThingDefinition>
                {
                    {1,new ThingDefinition(oldNum:1,actor:"A1",angles:0,holowall:false,pathing:false,ambush:false,minskill:0) },
                    {2,new ThingDefinition(oldNum:2,actor:"A2",angles:0,holowall:false,pathing:false,ambush:false,minskill:0) },
                });

            var tm2 = new ThingMappings(
                elevators: Enumerable.Empty<ushort>(),
                positionlessTriggers: new Dictionary<ushort, PositionlessTrigger>(),
                thingDefinitions: new Dictionary<ushort, ThingDefinition>
                {
                    {2,new ThingDefinition(oldNum:2,actor:"A2new",angles:0,holowall:false,pathing:false,ambush:false,minskill:0) },
                    {3,new ThingDefinition(oldNum:3,actor:"A3",angles:0,holowall:false,pathing:false,ambush:false,minskill:0) },
                });

            tm1.Add(tm2);

            Assert.That(
                tm1.ThingDefinitions,
                Has.Count.EqualTo(3),
                "Unexpected number of thing definitions.");

            Assert.That(
                tm1.ThingDefinitions.Keys,
                Is.EquivalentTo(new ushort[] { 1, 2, 3 }),
                "Unexpected thing definition keys.");

            Assert.That(
                tm1.ThingDefinitions.Values.Select(t => t.Actor),
                Is.EquivalentTo(new[] { "A1", "A2new", "A3" }),
                "Unexpected thing definition values.");
        }
    }
}