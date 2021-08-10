// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.DemoMaps
{
    public static class TexturesDemoMap
    {
        private const int NumGradients = 10;
        private const int TotalNumLevels = NumGradients + 1 + NumGradients;
        private const int HorizontalBuffer = 5;
        private const int Height = 13;

        public static MapData Create() => CreateMapAndTextures(new TextureQueue());

        public static MapData CreateMapAndTextures(TextureQueue textureQueue)
        {
            var mapSize = new Size(
                HorizontalBuffer + TotalNumLevels + HorizontalBuffer,
                Height);

            var (planeMap, tiles) = CreateGeometry(mapSize, textureQueue);

            return new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Textures Demo",
                Width: mapSize.Width,
                Height: mapSize.Height,
                Tiles: tiles,
                Sectors: ImmutableList.Create<Sector>().Add(
                    new Sector(
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    )),
                Zones: ImmutableList.Create<Zone>().Add(new Zone()),
                Planes: ImmutableList.Create<Plane>().Add(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create<ImmutableArray<MapSquare>>()
                    .Add(planeMap),
                Things: ImmutableList.Create<Thing>().Add(new Thing(
                    Type: Actor.Player1Start.ClassName,
                    X: 1.5,
                    Y: 1.5,
                    Z: 0,
                    Angle: 0,
                    Skill1: true,
                    Skill2: true,
                    Skill3: true,
                    Skill4: true
                )),
                Triggers: ImmutableList<Trigger>.Empty
            );
        }

        private static (ImmutableArray<MapSquare>, ImmutableList<Tile>) CreateGeometry(Size size, TextureQueue textureQueue)
        {
            var baseTile = DefaultTile.GrayStone1;
            var nsTexture = baseTile.TextureNorth.Name;
            var ewTexture = baseTile.TextureEast.Name;

            var tiles = new List<Tile> { baseTile };

            var board =
                new Canvas(size)
                    .FillRectangle(size.ToRectangle(), tile: -1)
                    .OutlineRectangle(size.ToRectangle(), tile: 0);

            var middleX = HorizontalBuffer + NumGradients;
            var middleY = Height / 2;

            board.Set(middleX, middleY, tile: 1);

            foreach (var darkLevel in Enumerable.Range(1, NumGradients))
            {
                board.Set(middleX - darkLevel, middleY, tile: tiles.Count);

                var newNS = nsTexture + "Dark" + darkLevel;
                var newEW = ewTexture + "Dark" + darkLevel;

                tiles.Add(baseTile with
                {
                    TextureNorth = newNS,
                    TextureSouth = newNS,
                    TextureEast = newEW,
                    TextureWest = newEW,
                });

                textureQueue.Add(new CompositeTexture(newNS, 64, 64, ImmutableList.Create(
                    new Patch(nsTexture, 0, 0),
                    new Patch(nsTexture, 0, 0, Blend: new ColorBlend("000000", Alpha: 0.1 * darkLevel)))));
                textureQueue.Add(new CompositeTexture(newEW, 64, 64, ImmutableList.Create(
                    new Patch(ewTexture, 0, 0),
                    new Patch(ewTexture, 0, 0, Blend: new ColorBlend("000000", Alpha: 0.1 * darkLevel)))));
            }

            foreach (var lightLevel in Enumerable.Range(1, NumGradients))
            {
                board.Set(middleX + lightLevel, middleY, tile: tiles.Count);

                var newNS = nsTexture + "Light" + lightLevel;
                var newEW = ewTexture + "Light" + lightLevel;

                tiles.Add(baseTile with
                {
                    TextureNorth = newNS,
                    TextureSouth = newNS,
                    TextureEast = newEW,
                    TextureWest = newEW,
                });

                textureQueue.Add(new CompositeTexture(newNS, 64, 64, ImmutableList.Create(
                    new Patch(nsTexture, 0, 0),
                    new Patch(nsTexture, 0, 0, Blend: new ColorBlend("FFFFFF", Alpha: 0.1 * lightLevel)))));
                textureQueue.Add(new CompositeTexture(newEW, 64, 64, ImmutableList.Create(
                    new Patch(ewTexture, 0, 0),
                    new Patch(ewTexture, 0, 0, Blend: new ColorBlend("FFFFFF", Alpha: 0.1 * lightLevel)))));
            }

            return (board.ToPlaneMap(), tiles.ToImmutableList());
        }
    }
}