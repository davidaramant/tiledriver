// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SkiaSharp;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;
using Tiledriver.Core.Wolf3D;
using Size = Tiledriver.Core.LevelGeometry.Size;

namespace Tiledriver.Core.DemoMaps
{
    public static class TileDemoMap
    {
        private const int Columns = 10;
        const int SpaceBetween = 3;
        private static readonly SKColor CeilingColor = new (0xc0, 0xc0, 0xc0);
        private static readonly SKColor FloorColor = new (0xa0, 0xa0, 0xa0);
        public static MapData Create() => CreateMapAndTextures(new TextureQueue());

        public static MapData CreateMapAndTextures(TextureQueue textureQueue)
        {
            var originalTiles = DefaultTile.NamedTiles;

            var rows = (int)Math.Ceiling((double)originalTiles.Count / Columns);

            var mapSize = new Size(
                2 + (SpaceBetween + 1) * Columns + SpaceBetween,
                2 + (SpaceBetween + 1) * rows + SpaceBetween);

            var (planeMap, sectors) = CreateGeometry(originalTiles, mapSize, rows, textureQueue);

            return new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Tile Demo",
                Width: mapSize.Width,
                Height: mapSize.Height,
                Tiles: originalTiles.Select(ot=>ot.Tile).ToImmutableList(),
                Sectors: sectors,
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

        private static (ImmutableArray<MapSquare>, ImmutableList<Sector>) CreateGeometry(
            IReadOnlyList<(string Name, Tile Tile)> tiles,
            Size size,
            int rows,
            TextureQueue textureQueue)
        {
            var boundaryTileIndex = tiles.FindIndex(namedTile => namedTile.Tile == DefaultTile.GrayStone1);
            var sectors = ImmutableList.CreateBuilder<Sector>();

            var defaultSector = new Sector(
                TextureCeiling: Texture.SolidColor(CeilingColor),
                TextureFloor: Texture.SolidColor(FloorColor));
            sectors.Add(defaultSector);

            var board =
                new Canvas(size)
                    .FillRectangle(size.ToRectangle(), tile: -1)
                    .OutlineRectangle(size.ToRectangle(), tile: boundaryTileIndex);

            foreach (var row in Enumerable.Range(0, rows))
            {
                int y = ((SpaceBetween + 1) * row) + (SpaceBetween + 1);
                foreach (var col in Enumerable.Range(0, Columns))
                {
                    int x = ((SpaceBetween + 1) * col) + (SpaceBetween + 1);

                    var tileId = row * Columns + col;

                    if (tileId < tiles.Count)
                    {
                        var namedTile = tiles[tileId];
                        var tile = namedTile.Tile;

                        void AddTexture(string name, PatchRotation rotate, int xDelta, int yDelta)
                        {
                            var textTexture = textureQueue.Add(
                                new RenderedTexture(
                                    FloorColor,
                                    TextColor: SKColors.Black,
                                    Text: name + "\n" + namedTile.Name,
                                    Rotation: rotate));

                            sectors.Add(defaultSector with { TextureFloor = textTexture });
                            board.Set(x + xDelta, y + yDelta, sector: sectors.Count - 1);
                        }

                        AddTexture(tile.TextureNorth.Name, PatchRotation.Rotate180, 0, -1);
                        AddTexture(tile.TextureEast.Name, PatchRotation.Rotate270, 1, 0);
                        AddTexture(tile.TextureSouth.Name, PatchRotation.None, 0, 1);
                        AddTexture(tile.TextureWest.Name, PatchRotation.Rotate90, -1, 0);

                        board.Set(new Position(x, y), tile: tileId);
                    }
                }
            }

            return (board.ToPlaneMap(), sectors.ToImmutable());
        }
    }
}