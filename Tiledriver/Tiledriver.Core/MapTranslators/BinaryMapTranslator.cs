// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutoMapper;
using Functional.Maybe;
using Tiledriver.Core.Extensions;
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
            // MapInfo has:
            // - specialActions
            // TODO: Support map name lookups.  This requires parsing the LANGUAGE lump

            // Plane 0 - map geometry
            // Plane 1 - things
            // Plane 2 - floor/ceiling

            var zones = new List<Zone> { new Zone() };

            // TODO: Is this Boolean necessary?
            var hasSectorInfo = binaryMap.Plane2.Any(num => num != 0);
            var sectors = hasSectorInfo ? TranslateSectors(binaryMap) : CreateDefaultSector(mapInfo);

            var ambushSpots = new HashSet<Point>();

            var triggers = new List<Trigger>();
            var tileSpaces = TranslateTileSpaces(binaryMap, triggers, ambushSpots);
            var things = TranslateThings(binaryMap, triggers, tileSpaces, ambushSpots);

            return new MapData(
                nameSpace: "Wolf3D",
                tileSize: 64,
                width: binaryMap.Size.Width,
                height: binaryMap.Size.Height,
                name: mapInfo.MapName.OrElse(binaryMap.Name),
                tiles: _translatorInfo.TileMappings.TileTemplates.Select(template => _autoMapper.Map<Tile>(template)),
                sectors: sectors,
                zones: zones,
                planes: new List<Plane> { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(tileSpaces: tileSpaces), },
                things: things,
                triggers: triggers
            );
        }

        private IEnumerable<Thing> TranslateThings(BinaryMap binaryMap, List<Trigger> triggers, TileSpace[] tileSpaces, HashSet<Point> ambushSpots)
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

            foreach (var oldThing in binaryMap.GetAllSpots(planeIndex: 1).Where(spot => spot.OldNum != 0))
            {
                // TODO: Report errors here
                var mapping = _translatorInfo.LookupThingMapping(oldThing.OldNum);

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
                            // TODO: Is this irrelevant for UWMF?  It seems to make the tile instance non-solid.
                            // This would have to modify the tile list to do anything
                        }

                        yield return thing;
                        break;

                    case Elevator elevator:
                        // TODO: thing elevators
                        break;

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

            var tileIndexMapping =
                _translatorInfo.TileMappings.TileTemplates.
                Select((template, index) => new { OldNum = template.OldNum, TileIndex = index }).
                ToDictionary(pair => pair.OldNum, pair => pair.TileIndex);

            var triggerLookup = _translatorInfo.TileMappings.TriggerTemplates.CondenseToDictionary(tt => tt.OldNum, tt => tt);
            var ambushLookup = _translatorInfo.TileMappings.AmbushModzones.CondenseToDictionary(amz => amz.OldNum, amz => amz);
            var changeTriggerLookup = _translatorInfo.TileMappings.ChangeTriggerModzones.CondenseToDictionary(ctmz => ctmz.OldNum, ctmz => ctmz);

            var changeTriggerSpots = new List<(Point, ChangeTriggerModzone)>();

            // TODO: Change Trigger Modzones
            // TODO: Zone Templates

            foreach (var spot in binaryMap.GetAllSpots(planeIndex: 0))
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
                        // TODO: Fillzone
                    }
                    else
                    {
                        // TODO: Non-fillzone
                    }
                }

                if (changeTriggerLookup.TryGetValue(spot.OldNum, out var changeTriggerModzone))
                {
                    changeTriggerSpots.Add((spot.Location, changeTriggerModzone));
                    if (changeTriggerModzone.Fillzone)
                    {
                        // TODO: Fillzone
                    }
                    else
                    {
                        // TODO: non-fillzone
                    }
                }
            }

            foreach (var (location, changeTrigger) in changeTriggerSpots)
            {
                foreach (var adjacentLocation in location.GetAdjacentPoints(binaryMap.Size))
                {
                    
                }
            }

            return spaces;
        }
    }
}
