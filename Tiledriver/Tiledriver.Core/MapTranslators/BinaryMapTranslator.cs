// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
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

            var hasSectorInfo = binaryMap.Plane2.Any(num => num != 0);
            var sectors = hasSectorInfo ? TranslateSectors(binaryMap) : CreateDefaultSector(mapInfo);

            var triggers = new List<Trigger>();
            var tileSpaces = TranslateTileSpaces(binaryMap, triggers);
            var things = TranslateThings(binaryMap, triggers);

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

        private IEnumerable<Thing> TranslateThings(BinaryMap binaryMap, List<Trigger> triggers)
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

            var oldThings =
                binaryMap.Plane1.
                    Select((oldNum, index) => new
                    {
                        OldNum = oldNum,
                        X = index % binaryMap.Size.Width,
                        Y = index / binaryMap.Size.Width,
                    }).
                    Where(p => p.OldNum != 0);

            foreach (var oldThing in oldThings)
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

                        // TODO: Holowall
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

        private IEnumerable<TileSpace> TranslateTileSpaces(BinaryMap binaryMap, List<Trigger> triggers)
        {
            var length = binaryMap.Size.Width * binaryMap.Size.Height;
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

            for (int i = 0; i < length; i++)
            {
                var x = i % binaryMap.Size.Width;
                var y = i / binaryMap.Size.Width;

                var oldNum = binaryMap.Plane0[i];

                if (tileIndexMapping.TryGetValue(oldNum, out var tileIndex))
                {
                    spaces[i].Tile = tileIndex;
                }

                if (triggerLookup.TryGetValue(oldNum, out var triggerTemplate))
                {
                    var trigger = _autoMapper.Map<Trigger>(triggerTemplate);
                    trigger.X = x;
                    trigger.Y = y;
                    trigger.Z = 0;
                    triggers.Add(trigger);
                }

            }

            return spaces;
        }
    }
}
