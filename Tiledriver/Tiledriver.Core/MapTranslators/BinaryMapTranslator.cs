// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using AutoMapper;
using Functional.Maybe;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.MapInfos;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Xlat;
using Tiledriver.Core.Wolf3D;

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

            var triggers = new List<Trigger>();
            var tileSpaces = TranslateTileSpaces(binaryMap, triggers);
            var things = TranslateThings(binaryMap, triggers, tileSpaces);

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

        private IEnumerable<Thing> TranslateThings(BinaryMap binaryMap, List<Trigger> triggers, TileSpace[] tileSpaces)
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
                            x: oldThing.X + 0.5,
                            y: oldThing.Y + 0.5,
                            z: 0,
                            angle: TranslateAngle(thingTemplate, oldThing.OldNum),
                            ambush: thingTemplate.Ambush,
                            patrol: thingTemplate.Pathing,
                            skill1: thingTemplate.Minskill <= 1,
                            skill2: thingTemplate.Minskill <= 1,
                            skill3: thingTemplate.Minskill <= 2,
                            skill4: thingTemplate.Minskill <= 3);

                        if (thingTemplate.Holowall)
                        {
                            // TODO: Do something here
                        }

                        // TODO: Ambush can also be set from tiles

                        yield return thing;
                        break;

                    case Elevator elevator:
                        // TODO: thing elevators
                        break;

                    case TriggerTemplate triggerTemplate:
                        var trigger = _autoMapper.Map<Trigger>(triggerTemplate);
                        trigger.X = oldThing.X;
                        trigger.Y = oldThing.Y;
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

        private TileSpace[] TranslateTileSpaces(BinaryMap binaryMap, List<Trigger> triggers)
        {
            var spaces =
                Enumerable.Range(1, binaryMap.Size.Height * binaryMap.Size.Width).
                Select(_ => new TileSpace(tile: -1, sector: 0, zone: -1)).
                ToArray();

            var tileIndexMapping =
                _translatorInfo.TileMappings.TileTemplates.
                Select((template, index) => new { OldNum = template.OldNum, TileIndex = index }).
                ToDictionary(pair => pair.OldNum, pair => pair.TileIndex);

            var triggerLookup = _translatorInfo.TileMappings.TriggerTemplates.ToDictionary(t => t.OldNum, t => t);

            // TODO: Ambush Modzones
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
                    trigger.X = spot.X;
                    trigger.Y = spot.Y;
                    trigger.Z = 0;
                    triggers.Add(trigger);
                }

            }

            return spaces;
        }
    }
}
