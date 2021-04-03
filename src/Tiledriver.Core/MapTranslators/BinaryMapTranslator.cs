// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutoMapper;
using Functional.Maybe;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.Extensions.Directions;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfos;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;

namespace Tiledriver.Core.MapTranslators
{
    public sealed class BinaryMapTranslator
    {
        private readonly MapTranslatorInfo _translatorInfo;
        private readonly IMapper _autoMapper;

        public BinaryMapTranslator(MapTranslatorInfo translatorInfo, IMapper autoMapper)
        {
            _translatorInfo = translatorInfo;
            _autoMapper = autoMapper;
        }

        public MapData Translate(BinaryMap binaryMap, Map mapInfo)
        {
            // TODO: MapInfo - specialActions


            // TODO: Support map name lookups.  This requires parsing the LANGUAGE lump

            // Plane 0 - map geometry
            // Plane 1 - things
            // Plane 2 - floor/ceiling

            // TODO: Is this Boolean necessary?
            var hasSectorInfo = binaryMap.GetRawPlaneData(BinaryMapPlaneId.Sector).Any(num => num != 0);
            var sectors = hasSectorInfo ? TranslateSectors(binaryMap) : CreateDefaultSector(mapInfo);
            var tiles = _translatorInfo.TileMappings.GetCondensedTileTemplates().Select(template => _autoMapper.Map<Tile>(template)).ToList();

            var ambushSpots = new HashSet<Point>();

            var triggers = new List<Trigger>();
            var tileSpaces = TranslateTileSpaces(binaryMap, triggers, ambushSpots).ToArray();
            var things = TranslateThings(binaryMap, triggers, tileSpaces, ambushSpots, tiles).ToArray();

            return new MapData(
                nameSpace: "Wolf3D",
                tileSize: 64,
                width: binaryMap.Size.Width,
                height: binaryMap.Size.Height,
                name: mapInfo.MapName.OrElse(binaryMap.Name),
                tiles: tiles,
                sectors: sectors,
                zones: _translatorInfo.TileMappings.ZoneTemplates.
                    Select(zt => new Zone(comment: zt.Comment, unknownProperties: zt.UnknownProperties)),
                planes: new List<Plane> { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(tileSpaces: tileSpaces), },
                things: things,
                triggers: triggers
            );
        }

        private IEnumerable<Thing> TranslateThings(
            BinaryMap binaryMap,
            List<Trigger> triggers,
            TileSpace[] tileSpaces,
            HashSet<Point> ambushSpots,
            List<Tile> tiles)
        {
            int TranslateAngle(ThingTemplate template, ushort oldNum)
            {
                if (template.Angles == 0)
                    return 0;

                var angle = (oldNum - template.OldNum) * (360 / template.Angles);

                if (template.Type == "$Player1Start")
                {
                    angle = (360 + 360 / template.Angles) - angle;
                }

                return angle;
            }

            foreach (var oldThing in binaryMap.GetAllSpots(BinaryMapPlaneId.Thing).Where(spot => spot.OldNum != 0))
            {
                var possibleMapping = _translatorInfo.TryLookupThingMapping(oldThing.OldNum);

                // TODO: Report errors here
                var mapping = possibleMapping.OrElse(() => new ThingTemplate(
                    oldNum: oldThing.OldNum,
                    type: $"Unknown {oldThing.OldNum}",
                    angles: 0,
                    holowall: false,
                    pathing: false,
                    ambush: false,
                    minskill: 0));

                switch (mapping)
                {
                    case ThingTemplate thingTemplate:
                        var thing = new Thing(
                            type: thingTemplate.Type,
                            x: oldThing.Location.X + 0.5,
                            y: oldThing.Location.Y + 0.5,
                            z: 0,
                            angle: TranslateAngle(thingTemplate, oldThing.OldNum),
                            ambush: thingTemplate.Ambush || ambushSpots.Contains(oldThing.Location),
                            patrol: thingTemplate.Pathing,
                            skill1: thingTemplate.Minskill <= 1,
                            skill2: thingTemplate.Minskill <= 1,
                            skill3: thingTemplate.Minskill <= 2,
                            skill4: thingTemplate.Minskill <= 3);

                        if (thingTemplate.Holowall)
                        {
                            void MakeTilePassthrough(TileSpace space)
                            {
                                if (!space.HasTile)
                                    return;

                                var tileClone = tiles[space.Tile].Clone();
                                tileClone.SetAllBlocking(enabled: false);
                                space.Tile = tiles.Count;
                                tiles.Add(tileClone);
                            }

                            var tileSpace = tileSpaces[oldThing.Index];

                            if (tileSpace.HasTile)
                            {
                                MakeTilePassthrough(tileSpace);

                                // If we created a holowall and we path into another wall it should also become non-solid.
                                if (thingTemplate.Pathing)
                                {
                                    var thingDirection = (Direction)(thing.Angle / 90);
                                    int PositionToIndex(Point position) => position.Y * binaryMap.Size.Width + position.X;

                                    oldThing.Location.GetAdjacentPoints(binaryMap.Size).
                                        FirstMaybe(pair => pair.direction == thingDirection).
                                        Select(pair => PositionToIndex(pair.point)).
                                        Select(index => tileSpaces[index]).
                                        Do(MakeTilePassthrough);
                                }
                            }
                        }

                        yield return thing;
                        break;

                    case Elevator elevator:
                        throw new NotImplementedException("TODO: Thing elevators");

                    case TriggerTemplate triggerTemplate:
                        var trigger = _autoMapper.Map<Trigger>(triggerTemplate);
                        trigger.X = oldThing.Location.X;
                        trigger.Y = oldThing.Location.Y;
                        trigger.Z = 0;
                        triggers.Add(trigger);
                        break;

                    default:
                        throw new InvalidOperationException("Unknown mapping type");
                }
            }
        }

        private static IEnumerable<Sector> TranslateSectors(BinaryMap binaryMap)
        {
            throw new NotImplementedException("TODO: Translate sectors");
        }

        private static IEnumerable<Sector> CreateDefaultSector(Map mapInfo)
        {
            // TODO: What exception should happen here?  Some kind of DataDefinitionException?
            return new[]
            {
                new Sector(
                    textureCeiling:mapInfo.DefaultCeiling.OrElse(()=>new Exception()),
                    textureFloor:mapInfo.DefaultFloor.OrElse(()=>new Exception())),
            };
        }

        private TileSpace[] TranslateTileSpaces(BinaryMap binaryMap, List<Trigger> triggers, HashSet<Point> ambushSpots)
        {
            var spaces =
                Enumerable.Range(1, binaryMap.Size.Height * binaryMap.Size.Width).
                Select(_ => new TileSpace(tile: -1, sector: 0, zone: -1)).
                ToArray();

            var zoneLookup =
                _translatorInfo.TileMappings.ZoneTemplates.
                Select((zt, index) => new { zt, index }).
                ToDictionary(pair => pair.zt.OldNum, pair => pair.index);

            var tileIndexMapping =_translatorInfo.TileMappings.GetTileIndexLookup();

            var triggerLookup = _translatorInfo.TileMappings.TriggerTemplates.CondenseToDictionary(tt => tt.OldNum, tt => tt);
            var ambushLookup = _translatorInfo.TileMappings.AmbushModzones.CondenseToDictionary(amz => amz.OldNum, amz => amz);
            var changeTriggerLookup = _translatorInfo.TileMappings.ChangeTriggerModzones.CondenseToDictionary(ctmz => ctmz.OldNum, ctmz => ctmz);

            var changeTriggerSpots = new List<(Point, ChangeTriggerModzone)>();

            var zoneFillSpots = new List<Point>();

            foreach (var spot in binaryMap.GetAllSpots(BinaryMapPlaneId.Geometry))
            {
                if (tileIndexMapping.TryGetValue(spot.OldNum, out var tileIndex))
                {
                    spaces[spot.Index].Tile = tileIndex;
                }

                if (triggerLookup.TryGetValue(spot.OldNum, out var triggerTemplate))
                {
                    var trigger = _autoMapper.Map<Trigger>(triggerTemplate);
                    trigger.X = spot.Location.X;
                    trigger.Y = spot.Location.Y;
                    trigger.Z = 0;
                    triggers.Add(trigger);
                }

                if (ambushLookup.TryGetValue(spot.OldNum, out var ambushModzone))
                {
                    ambushSpots.Add(spot.Location);
                    if (ambushModzone.Fillzone)
                    {
                        zoneFillSpots.Add(spot.Location);
                    }
                }

                if (changeTriggerLookup.TryGetValue(spot.OldNum, out var changeTriggerModzone))
                {
                    changeTriggerSpots.Add((spot.Location, changeTriggerModzone));
                    if (changeTriggerModzone.Fillzone)
                    {
                        zoneFillSpots.Add(spot.Location);
                    }
                }

                if (zoneLookup.TryGetValue(spot.OldNum, out var zoneIndex))
                {
                    spaces[spot.Index].Zone = zoneIndex;
                }
            }

            int PositionToIndex(Point position) => position.Y * binaryMap.Size.Width + position.X;
            // For fill zone spots, set the zone to the first valid adjacent zone (if any)
            foreach (var fillSpot in zoneFillSpots)
            {
                var fillSpotIndex = PositionToIndex(fillSpot);

                fillSpot.GetAdjacentPoints(binaryMap.Size, start: Direction.East, clockWise: false)
                    .Select(adjacent => spaces[PositionToIndex(adjacent.point)].Zone)
                    .FirstMaybe(zoneId => zoneId != -1)
                    .Do(zoneId => spaces[fillSpotIndex].Zone = zoneId);
            }

            foreach (var (location, changeTrigger) in changeTriggerSpots)
            {
                foreach (var (directionFromLocation, candidatePosition) in
                    location.GetAdjacentPoints(binaryMap.Size, start: Direction.West, clockWise: true))
                {
                    var directionToLocation = directionFromLocation.Reverse();

                    // This logic depends on mutating the trigger template in the below loop
                    if (!changeTrigger.TriggerTemplate.ActivatesIn(directionToLocation))
                        continue;

                    foreach (var existingTrigger in triggers.ToArray().Where(t =>
                                    t.X == candidatePosition.X &&
                                    t.Y == candidatePosition.Y &&
                                    t.Action == changeTrigger.Action))
                    {
                        existingTrigger.SetActivation(directionToLocation, false);

                        // Mutating trigger template!!!
                        changeTrigger.TriggerTemplate.SetAllActivations(false);
                        changeTrigger.TriggerTemplate.SetActivation(directionToLocation, true);

                        var newTrigger = _autoMapper.Map<Trigger>(changeTrigger.TriggerTemplate);
                        newTrigger.X = existingTrigger.X;
                        newTrigger.Y = existingTrigger.Y;
                        newTrigger.Z = existingTrigger.Z;

                        triggers.Add(newTrigger);
                    }
                }
            }

            return spaces;
        }
    }
}
