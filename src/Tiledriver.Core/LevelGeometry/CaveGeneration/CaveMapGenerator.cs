// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public static class CaveMapGenerator
    {
        public static MapData Create(int seed, string texturePrefix, TextureQueue textureQueue)
        {
            var random = new Random(seed);

            var caveBoard =
                new CellBoard(new Size(128, 128))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 3)
                    .GenerateStandardCave();

            var caveArea =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(caveBoard.Dimensions, p => caveBoard[p] == CellType.Dead)
                    .OrderByDescending(a => a.Area)
                    .First();

            var interior = caveArea.DetermineDistanceToEdges(Neighborhood.VonNeumann);

            var alternateMaterial =
                new CellBoard(new Size(caveBoard.Dimensions.Width + 1, caveBoard.Dimensions.Height + 1))
                    .Fill(random, probabilityAlive: 0.5)
                    .MakeBorderAlive(thickness: 3)
                    .RunGenerations(6);

            var (planeMap, sectors, tiles) =
                CreateGeometry(caveBoard.Dimensions, caveArea, alternateMaterial, textureQueue);

            var playerPosition = caveArea.First();

            return new MapData(
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Procedural Cave",
                Width: caveBoard.Dimensions.Width,
                Height: caveBoard.Dimensions.Height,
                Tiles: tiles,
                Sectors: sectors,
                Zones: ImmutableArray.Create(new Zone()),
                Planes: ImmutableArray.Create(new Plane(Depth: 64)),
                PlaneMaps: ImmutableArray.Create(planeMap),
                Things: ImmutableArray.Create(
                    new Thing(
                    Type: Actor.Player1Start.ClassName,
                        X: playerPosition.X + 0.5,
                        Y: playerPosition.Y + 0.5,
                        Z: 0,
                        Angle: 0,
                        Ambush: true,
                        Skill1: true,
                        Skill2: true,
                        Skill3: true,
                        Skill4: true)),
                Triggers: ImmutableArray<Trigger>.Empty);
        }

        static (ImmutableArray<MapSquare>, ImmutableArray<Sector>, ImmutableArray<Tile>) CreateGeometry(
            Size size,
            ConnectedArea cave,
            CellBoard alternateMaterial,
            TextureQueue textureQueue)
        {
            var planeMap = new Canvas(size);

            string GetTextureName(Corners corners, int light)
            {
                string name = $"t{(int)corners:D2}";
                textureQueue.Add(new CompositeTexture(name, 256, 256,
                    ImmutableArray.Create(new Patch($"TILE{(int)corners:D2}", 0, 0)),
                    XScale: 4, YScale: 4));
                return name;

            }

            var sectorSequence = new ModelSequence<SectorDescription, Sector>(description =>
                new Sector(
                    TextureCeiling: GetTextureName(description.Ceiling,description.CeilingLight),
                    TextureFloor: GetTextureName(description.Floor,description.FloorLight)));
            var tileSequence = new ModelSequence<TileDescription, Tile>(description =>
                new Tile(
                    TextureEast: GetTextureName(description.EastCorners,description.EastLight),
                    TextureNorth: GetTextureName(description.NorthCorners,description.NorthLight),
                    TextureWest: GetTextureName(description.WestCorners,description.WestLight),
                    TextureSouth: GetTextureName(description.SouthCorners,description.SouthLight)));

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    int tileId = -1;
                    if (!cave.Contains(new Position(x, y)))
                    {
                        tileId = tileSequence.GetIndex(new TileDescription(
                            NorthCorners: Corners.None,
                            EastCorners: Corners.None,
                            SouthCorners: Corners.None,
                            WestCorners: Corners.None,
                            NorthLight: 0,
                            EastLight: 0,
                            SouthLight: 0,
                            WestLight: 0));
                    }

                    int sectorId = sectorSequence.GetIndex(new SectorDescription(
                        Floor: Corners.None,
                        Ceiling: Corners.None,
                        FloorLight: 0,
                        CeilingLight: 0));

                    planeMap.Set(x, y,
                        tile: tileId,
                        sector: sectorId,
                        zone: 0);
                }
            }

            return (
                planeMap.ToPlaneMap(),
                sectorSequence.GetDefinitions().ToImmutableArray(),
                tileSequence.GetDefinitions().ToImmutableArray());
        }

        private sealed record SectorDescription(
            Corners Floor,
            Corners Ceiling,
            int FloorLight,
            int CeilingLight);

        private sealed record TileDescription(
            Corners NorthCorners,
            Corners EastCorners,
            Corners SouthCorners,
            Corners WestCorners,
            int NorthLight,
            int EastLight,
            int SouthLight,
            int WestLight);
    }
}