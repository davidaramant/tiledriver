// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public static class CaveMapGenerator
    {
        public static MapData Create(int seed, string texture0, string texture1)
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
                CreateGeometry(caveBoard.Dimensions, caveArea, alternateMaterial, texture0, texture1);

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
                Things: ImmutableArray<Thing>.Empty,
                Triggers: ImmutableArray<Trigger>.Empty);
        }

        static (ImmutableArray<MapSquare>, ImmutableArray<Sector>, ImmutableArray<Tile>) CreateGeometry(
            Size size,
            ConnectedArea cave,
            CellBoard alternateMaterial,
            string texture0,
            string texture1)
        {
            var planeMap = ImmutableArray.CreateBuilder<MapSquare>();
            var sectorSequence = new ModelSequence<SectorDescription, Sector>(description =>
                new Sector(TextureCeiling: Texture.None, TextureFloor: Texture.None));
            var tileSequence = new ModelSequence<TileDescription, Tile>(description =>
                new Tile(
                    TextureEast: Texture.None,
                    TextureNorth: Texture.None,
                    TextureWest: Texture.None,
                    TextureSouth: Texture.None));

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    planeMap.Add(new MapSquare(Tile: 0, Sector: 0, Zone: 0));
                }
            }

            return (
                planeMap.ToImmutable(),
                sectorSequence.GetDefinitions().ToImmutableArray(),
                tileSequence.GetDefinitions().ToImmutableArray());
        }

        [Flags]
        private enum Corners : byte
        {
            TopLeft = 0b0001,
            TopRight = 0b0010,
            BottomLeft = 0b0100,
            BottomRight = 0b1000
        }

        private sealed record SectorDescription(
            Corners Corners,
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