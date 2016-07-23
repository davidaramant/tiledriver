// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core
{
    public static class DemoMap
    {
        public static Map Create()
        {
            var map = new Map
            (
                nameSpace: "Wolf3D",
                tileSize: 64,
                name: "Test Output",
                width: 64,
                height: 64,
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
                planeMaps: new[] { new PlaneMap(CreateGeometry(width: 64, height: 64)) },
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
            map.PlaneMaps[0].TileSpaces[1 * 64 + 2].Tile = 1;
            map.PlaneMaps[0].TileSpaces[1 * 64 + 2].Tag = 1;

            map.PlaneMaps[0].TileSpaces[2 * 64 + 1].Tile = 0;
            map.PlaneMaps[0].TileSpaces[2 * 64 + 2].Tile = 0;


            GenerateThings(map);

            return map;
        }

        private static void GenerateThings(Map map)
        {
            // Decorations
            map.Things.AddRange(Actor.GetAll().Where(a => a.Category == "Decorations").Select((actor, i) =>
            {
                var x = 4 + 4 * (int)(i / 60);
                var y = 2 + (i % 60);

                map.PlaneMaps.First().TileSpaces[y * 64 + x].Sector = actor.Wolf3D ? 1 : 2;

                return new Thing(
                    type: actor.ClassName,
                    x: x + 0.5,
                    y: y + 0.5,
                    z: 0,
                    angle: 0,
                    skill1: true,
                    skill2: true,
                    skill3: true,
                    skill4: true);
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
