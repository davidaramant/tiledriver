// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.DemoMaps
{
    public static class ThingDemoMap
    {
        private const int HorizontalBuffer = 4;
        private const int VerticalBuffer = 3;

        // TODO: Add arguments for which types of things to include
        public static MapData Create()
        {
            var things = GenerateThings().ToImmutableList();

            int width = things.Max(t => (int)(t.X + 0.5)) + HorizontalBuffer;
            int height = things.Max(t => (int)(t.Y + 0.5)) + VerticalBuffer;

            return new MapData(
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Thing Demo",
                Width: width,
                Height: height,
                Tiles: ImmutableList.Create(
                    new Tile(
                        TextureNorth: "GSTONEA1",
                        TextureSouth: "GSTONEA1",
                        TextureEast: "GSTONEA2",
                        TextureWest: "GSTONEA2"
                    ),
                    new Tile(
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
                    )),
                Sectors: ImmutableList.Create(
                    new Sector(
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    ),
                    new Sector(
                        TextureCeiling: "#00FF00",
                        TextureFloor: "#00FF00",
                        Comment: "Good thing"
                    ),
                    new Sector(
                        TextureCeiling: "#FF0000",
                        TextureFloor: "#FF0000",
                        Comment: "Invalid thing"
                    )),
                Zones: ImmutableList.Create(new Zone()),
                Planes: ImmutableList.Create(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create(CreateGeometry(width: width, height: height, things).ToImmutableArray()),
                Things: things.Add(
                    new Thing(
                        Type: Actor.Player1Start.ClassName,
                        X: 1.5,
                        Y: 1.5,
                        Z: 0,
                        Angle: 0,
                        Ambush: true,
                        Skill1: true,
                        Skill2: true,
                        Skill3: true,
                        Skill4: true)),
                Triggers: ImmutableList.Create(
                    new Trigger(
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
                    ))
            );
        }

        private static IEnumerable<Thing> GenerateThings()
        {
            var indexedActorGroups =
                Actor.GetAll()
                .Where(a => a.ActorCategory != ActorCategory.Special)
                .GroupBy(a => a.ActorCategory)
                .Select((group, index) => new { group, index });

            return indexedActorGroups.SelectMany(iag => GenerateThings(iag.group, iag.index));
        }

        private static IEnumerable<Thing> GenerateThings(IEnumerable<Actor> actors, int thingColumn) =>
            actors.Select((actor, actorIndex) =>
            {
                var x = HorizontalBuffer + (2 * thingColumn);
                var y = VerticalBuffer + actorIndex;

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

        private static IEnumerable<MapSquare> CreateGeometry(int width, int height, IEnumerable<Thing> things)
        {
            MapSquare solidTile = new(Tile: 0, Sector: 0, Zone: -1);

            var size = new Size(width, height);

            var board =
                new Canvas(size)
                    .FillRectangle(size.ToRectangle(), tile: -1)
                    .OutlineRectangle(size.ToRectangle(), tile: 0);

            // Make the starting nook
            board[new Position(2, 1)] = new MapSquare(Tile: 1, Sector: 0, Zone: 0, Tag: 1);
            board[new Position(1, 2)] = solidTile;
            board[new Position(2, 2)] = solidTile;

            // Mark out the things
            foreach (var thing in things)
            {
                int x = (int)thing.X;
                int y = (int)thing.Y;

                var ts = board[new Position(x, y)] with { Sector = 1 };
                board[new Position(x, y)] = ts;
            }

            return board.ToPlaneMap();
        }
    }
}
