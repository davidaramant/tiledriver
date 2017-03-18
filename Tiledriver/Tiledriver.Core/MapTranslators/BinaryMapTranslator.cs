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

            var hasSectorInfo = binaryMap.Plane2.Any(num => num != 0);

            return new MapData(
                nameSpace: "Wolf3D",
                tileSize: 64,
                width: binaryMap.Size.Width,
                height: binaryMap.Size.Height,
                name: mapInfo.MapName.OrElse(binaryMap.Name),
                tiles: _translatorInfo.TileMappings.Tiles.Values,
                sectors: hasSectorInfo ? TranslateSectors(binaryMap) : CreateDefaultSector(mapInfo),
                zones: new List<Zone> // TODO
                {
                    new Zone(),
                },
                planes: new List<Plane> { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(tileSpaces: TranslateTileSpaces(binaryMap, mapInfo)), },
                things: TranslateThings(binaryMap),
                triggers: new List<Trigger>() // TODO
            );
        }

        private IEnumerable<Thing> TranslateThings(BinaryMap binaryMap)
        {
            var oldThings =
                binaryMap.Plane1.
                    Select((oldNum, index) => new
                    {
                        OldNum = oldNum,
                        X = index % binaryMap.Size.Width,
                        Y = index / binaryMap.Size.Width,
                    }).
                    Where(p => p.OldNum != 0);

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

            foreach (var oldThing in oldThings)
            {
                // TODO: Report errors here
                var mapping = _translatorInfo.LookupThingMapping(oldThing.OldNum);

                switch (mapping)
                {
                    case ThingTemplate thingTemplate:
                        var thing = new Thing(
                            type: thingTemplate.Type,
                            x: oldThing.X,
                            y: oldThing.Y,
                            z: 0,
                            angle: TranslateAngle(thingTemplate, oldThing.OldNum),
                            ambush: thingTemplate.Ambush,
                            patrol: thingTemplate.Pathing,
                            skill1: thingTemplate.Minskill <= 1,
                            skill2: thingTemplate.Minskill <= 1,
                            skill3: thingTemplate.Minskill <= 2,
                            skill4: thingTemplate.Minskill <= 3);

                        yield return thing;
                        break;

                    case Elevator elevator:
                        // TODO: thing elevators
                        break;

                    case TriggerTemplate triggerTemplate:
                        // TODO: thing triggers
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

        private IEnumerable<TileSpace> TranslateTileSpaces(BinaryMap binaryMap, Map mapInfo)
        {
            var length = binaryMap.Size.Width * binaryMap.Size.Height;
            var spaces = 
                Enumerable.Range(1, binaryMap.Size.Height * binaryMap.Size.Width).
                Select(_ => new TileSpace(tile: -1, sector: 0, zone: 0)).
                ToArray();

            for (int i = 0; i < length; i++)
            {
                var oldNum = binaryMap.Plane0[i];

                if (_translatorInfo.TileMappings.Tiles.TryGetValue(oldNum, out var tile))
                {
                    // TODO: This is godawful
                    var l = _translatorInfo.TileMappings.Tiles.Values.ToList();
                    var index = l.IndexOf(tile);
                    spaces[i].Tile = index;
                }
            }

            return spaces;
        }
    }
}
