﻿/*
** DemoMap.cs
**
**---------------------------------------------------------------------------
** Copyright (c) 2016, David Aramant
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
**
*/

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
            const int width = 128;
            const int height = 128;

            var map = new Map
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

        private static void GenerateThings(Map map)
        {
            // Decorations
            foreach (var indexedActorGroup in Actor.GetAll().Where(a => a.Category != "Special").GroupBy(a => a.Category).Select((group, index) => new { group, index }))
            {
                DrawActors(map, indexedActorGroup.group, indexedActorGroup.index * 8);
            }
            //DrawActors(map, Actor.GetAll().Where(a => a.Category == "Decorations"), 0);
        }

        private static void DrawActors(Map map, IEnumerable<Actor> actors, int offset)
        {
            map.Things.AddRange(actors.Select((actor, actorIndex) =>
            {
                var x = 4 + offset;
                var y = 2 + actorIndex;

                map.PlaneMaps[0].TileSpaces[y * map.Width + x].Sector = actor.Wolf3D ? 1 : 2;

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
