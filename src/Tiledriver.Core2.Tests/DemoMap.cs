// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests
{
    public static class DemoMap
    {
        public static MapData CreateThingDemoMap()
        {
            const int width = 128;
            const int height = 128;

            var things = GenerateThings().ToArray();

            var map = new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Thing Demo",
                Width: width,
                Height: height,
                Tiles: new[]
                {
                    new Tile
                    (
                        TextureNorth: "GSTONEA1",
                        TextureSouth: "GSTONEA1",
                        TextureEast: "GSTONEA2",
                        TextureWest: "GSTONEA2"
                    ),
                    new Tile
                    (
                        TextureNorth: "SLOT1_2",
                        TextureSouth: "SLOT1_2",
                        TextureWest: "DOOR1_2",
                        TextureEast: "DOOR1_2",
                        OffsetVertical: true,
                        OffsetHorizontal: false,
                        BlockingNorth: true,
                        BlockingSouth: true,
                        BlockingWest: true,
                        BlockingEast: true
                    ),
                }.ToImmutableList(),
                Sectors: new[]
                {
                    new Sector
                    (
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    ),
                    new Sector
                    (
                        TextureCeiling: "#00FF00",
                        TextureFloor: "#00FF00",
                        Comment:"Good thing"
                    ),
                    new Sector
                    (
                        TextureCeiling: "#FF0000",
                        TextureFloor: "#FF0000",
                        Comment:"Invalid thing"
                    ),
                }.ToImmutableList(),
                Zones: new[]
                {
                    new Zone(),
                }.ToImmutableList(),
                Planes: new[]
                {
                    new Plane(Depth: 64)
                }.ToImmutableList(),
                PlaneMaps: new[]
                {
                    new PlaneMap(CreateGeometry(width: width, height: height, things).ToImmutableList())
                }.ToImmutableList(),
                Things: things.Concat(new[] { new Thing
                (
                    Type: Actor.Player1Start.ClassName,
                    X: 1.5,
                    Y: 1.5,
                    Z: 0,
                    Angle: 0,
                    Skill1: true,
                    Skill2: true,
                    Skill3: true,
                    Skill4: true
                )}).ToImmutableList(),
                Triggers: new[]
                {
                    new Trigger
                    (
                        X: 2,
                        Y: 1,
                        Z: 0,
                        ActivateNorth: true,
                        ActivateSouth: true,
                        ActivateWest: true,
                        ActivateEast: true,
                        Action: "Door_Open",
                        Arg0: 1,
                        Arg1: 16,
                        Arg2: 300,
                        Arg3: 0,
                        Arg4: 0,
                        PlayerUse: true,
                        MonsterUse: true,
                        PlayerCross: false,
                        Repeatable: true,
                        Secret: false
                    ),
                }.ToImmutableList()
            );

            return map;
        }

        private static IEnumerable<Thing> GenerateThings()
        {
            var indexedActorGroups =
                Actor.GetAll()
                .Where(a => a.Category != "Special")
                .GroupBy(a => a.Category)
                .Select((group, index) => new { group, index });

            return indexedActorGroups.SelectMany(iag => GenerateThings(iag.group, iag.index));

        }

        private static IEnumerable<Thing> GenerateThings(IEnumerable<Actor> actors, int xOffset)
        {
            return actors.Select((actor, actorIndex) =>
            {
                var x = 4 + xOffset;
                var y = 2 + actorIndex;

                return new Thing(
                    Type: actor.ClassName,
                    X: x + 0.5,
                    Y: y + 0.5,
                    Z: 0,
                    Angle: 0,
                    Skill1: true,
                    Skill2: true,
                    Skill3: true,
                    Skill4: true,
                    Ambush: true);
            });
        }

        private static IEnumerable<TileSpace> CreateGeometry(int width, int height, IEnumerable<Thing> things)
        {
            var entries = new TileSpace[height, width];

            TileSpace solidTile = new(Tile: 0, Sector: 0, Zone: -1);
            TileSpace emptyTile = new(Tile: -1, Sector: 0, Zone: 0);

            // ### Build a big empty square

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = solidTile;
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = solidTile;
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = emptyTile;
                }
                entries[row, width - 1] = solidTile;
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = solidTile;
            }

            // Make the starting nook
            entries[1, 2] = new TileSpace(Tile: 1, Sector: 0, Zone: 0, Tag: 1);
            entries[2, 1] = solidTile;
            entries[2, 2] = solidTile;

            // Mark out the things
            foreach (var thing in things)
            {
                int x = (int)thing.X;
                int y = (int)thing.Y;

                var ts = entries[y, x] with { Sector = 1 };
                entries[y, x] = ts;
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
