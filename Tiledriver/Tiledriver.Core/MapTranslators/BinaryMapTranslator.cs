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
                tiles: new List<Tile> // TODO
                {
                    new Tile
                    (
                        textureNorth: "GSTONEA1",
                        textureSouth: "GSTONEA1",
                        textureEast: "GSTONEA2",
                        textureWest: "GSTONEA2"
                    ),
                },
                sectors: hasSectorInfo ? TranslateSectors(binaryMap) : CreateDefaultSector(mapInfo),
                zones: new List<Zone> // TODO
                {
                    new Zone(),
                },
                planes: new List<Plane> { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(tileSpaces: CreateGeometry(64, 64)), },
                things: TranslateThings(binaryMap),
                triggers: new List<Trigger>()
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

            //yield return new Thing
            //(
            //    type: Actor.Player1Start.ClassName,
            //    x: 1.5,
            //    y: 1.5,
            //    z: 0,
            //    angle: 0,
            //    skill1: true,
            //    skill2: true,
            //    skill3: true,
            //    skill4: true
            //);
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

        public static IEnumerable<TileSpace> CreateGeometry(int width, int height)
        {
            var entries = new TileSpace[height, width];

            Func<TileSpace> solidTile = () => new TileSpace(0, 0, -1);
            Func<TileSpace> emptyTile = () => new TileSpace(-1, 0, 0);

            // ### Build a big empty square

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = solidTile();
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = solidTile();
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = emptyTile();
                }
                entries[row, width - 1] = solidTile();
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = solidTile();
            }


            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    yield return entries[row, col];
                }
            }
        }
    }
}
