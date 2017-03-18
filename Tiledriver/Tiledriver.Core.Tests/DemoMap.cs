// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests
{
    public static class DemoMap
    {
        public static MapData Create()
        {
            const int width = 128;
            const int height = 128;

            var map = new MapData
            (
                nameSpace: "Wolf3D",
                tileSize: 64,
                name: "Test Output",
                width: width,
                height: height,
                comment: "",
                tiles: new[]
                {
                    new Tile
                    (
                        textureNorth: "GSTONEA1",
                        textureSouth: "GSTONEA1",
                        textureEast: "GSTONEA2",
                        textureWest: "GSTONEA2"
                    ),
                    new Tile
                    (
                        textureNorth: "SLOT1_2",
                        textureSouth: "SLOT1_2",
                        textureWest: "DOOR1_2",
                        textureEast: "DOOR1_2",
                        offsetVertical: true,
                        offsetHorizontal: false,
                        blockingNorth: true,
                        blockingSouth: true,
                        blockingWest: true,
                        blockingEast: true
                    ),
                },
                sectors: new[]
                {
                    new Sector
                    (
                        textureCeiling: "#C0C0C0",
                        textureFloor: "#A0A0A0"
                    ),
                    new Sector
                    (
                        textureCeiling: "#00FF00",
                        textureFloor: "#00FF00",
                        comment:"Good thing"
                    ),
                    new Sector
                    (
                        textureCeiling: "#FF0000",
                        textureFloor: "#FF0000",
                        comment:"Invalid thing"
                    ),
                },
                zones: new[]
                {
                    new Zone(),
                },
                planes: new[] { new Plane(depth: 64) },
                planeMaps: new[] { new PlaneMap(CreateGeometry(width: width, height: height)) },
                things: new[]
                {
                    new Thing
                    (
                        type: Actor.Player1Start.ClassName,
                        x: 1.5,
                        y: 1.5,
                        z: 0,
                        angle: 0,
                        skill1: true,
                        skill2: true,
                        skill3: true,
                        skill4: true
                    ),
                },
                triggers: new[]
                {
                    new Trigger
                    (
                        x: 2,
                        y: 1,
                        z: 0,
                        activateNorth: true,
                        activateSouth: true,
                        activateWest: true,
                        activateEast: true,
                        action: "Door_Open",
                        arg0: 1,
                        arg1: 16,
                        arg2: 300,
                        arg3: 0,
                        arg4: 0,
                        playerUse: true,
                        monsterUse: true,
                        playerCross: false,
                        repeatable: true,
                        secret: false
                    ),
                }
            );

            // Make the starting nook
            map.PlaneMaps[0].TileSpaces[1 * map.Width + 2].Tile = 1;
            map.PlaneMaps[0].TileSpaces[1 * map.Width + 2].Tag = 1;

            map.PlaneMaps[0].TileSpaces[2 * map.Width + 1].Tile = 0;
            map.PlaneMaps[0].TileSpaces[2 * map.Width + 2].Tile = 0;


            GenerateThings(map);

            return map;
        }

        private static void GenerateThings(MapData mapData)
        {
            // Decorations
            foreach (var indexedActorGroup in Actor.GetAll().Where(a => a.Category != "Special").GroupBy(a => a.Category).Select((group, index) => new { group, index }))
            {
                DrawActors(mapData, indexedActorGroup.group, indexedActorGroup.index * 8);
            }
            //DrawActors(map, Actor.GetAll().Where(a => a.Category == "Decorations"), 0);
        }

        private static void DrawActors(MapData mapData, IEnumerable<Actor> actors, int offset)
        {
            mapData.Things.AddRange(actors.Select((actor, actorIndex) =>
            {
                var x = 4 + offset;
                var y = 2 + actorIndex;

                mapData.PlaneMaps[0].TileSpaces[y * mapData.Width + x].Sector = actor.Wolf3D ? 1 : 2;

                return new Thing(
                    type: actor.ClassName,
                    x: x + 0.5,
                    y: y + 0.5,
                    z: 0,
                    angle: 0,
                    skill1: true,
                    skill2: true,
                    skill3: true,
                    skill4: true,
                    skill5: true,
                    ambush: true);
            }));
        }

        private static IEnumerable<TileSpace> CreateGeometry(int width, int height)
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
