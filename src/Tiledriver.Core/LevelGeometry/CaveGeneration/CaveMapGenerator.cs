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

            var (caveArea,size) =
                ConnectedAreaAnalyzer
                    .FindForegroundAreas(caveBoard.Dimensions, p => caveBoard[p] == CellType.Dead)
                    .OrderByDescending(a => a.Area)
                    .First()
                    .TrimExcess(border:1);

            var interior = caveArea.DetermineDistanceToEdges(Neighborhood.VonNeumann);

            var alternateFloor =
                new CellBoard(new Size(size.Width + 1, size.Height + 1))
                    .Fill(random, probabilityAlive: 0.5)
                    .RunGenerations(6);
            var alternateCeiling =
                new CellBoard(new Size(size.Width + 1, size.Height + 1))
                    .Fill(random, probabilityAlive: 0.5)
                    .RunGenerations(6);

            var (planeMap, sectors, tiles) =
                CreateGeometry(size, caveArea, alternateFloor, alternateCeiling, textureQueue, texturePrefix);

            var playerPosition = caveArea.First();

            return new MapData(
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Procedural Cave",
                Width: size.Width,
                Height: size.Height,
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
            CellBoard alternateFloor,
            CellBoard alternateCeiling,
            TextureQueue textureQueue,
            string texturePrefix)
        {
            var planeMap = new Canvas(size);

            string GetTextureName(Corners corners, int light)
            {
                string name = $"t{(int)corners:D2}";
                textureQueue.Add(new CompositeTexture(name, 256, 256,
                    ImmutableArray.Create(new Patch($"{texturePrefix}{(int)corners:D2}", 0, 0)),
                    XScale: 4, YScale: 4));
                return name;

            }

            Corners GetCorners(CellBoard board, Position pos) => ToCorners(
                topLeft: board[pos] == CellType.Dead,
                topRight: board[pos.Right()] == CellType.Dead,
                bottomLeft: board[pos.Below()] == CellType.Dead,
                bottomRight: board[pos.BelowRight()] == CellType.Dead);

            Corners GetSideCorners(Position left, Position right) => ToCorners(
                topLeft: alternateCeiling[left] == CellType.Dead,
                topRight: alternateCeiling[right] == CellType.Dead,
                bottomLeft: alternateFloor[left] == CellType.Dead,
                bottomRight: alternateFloor[right] == CellType.Dead
                );

            var sectorSequence = new ModelSequence<SectorDescription, Sector>(description =>
                new Sector(
                    TextureCeiling: GetTextureName(description.Ceiling, description.CeilingLight),
                    TextureFloor: GetTextureName(description.Floor, description.FloorLight)));
            var tileSequence = new ModelSequence<TileDescription, Tile>(description =>
                new Tile(
                    TextureEast: GetTextureName(description.EastCorners, description.EastLight),
                    TextureNorth: GetTextureName(description.NorthCorners, description.NorthLight),
                    TextureWest: GetTextureName(description.WestCorners, description.WestLight),
                    TextureSouth: GetTextureName(description.SouthCorners, description.SouthLight),
                    TextureOverhead: GetTextureName(description.FloorCorners, 0)));

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    var pos = new Position(x, y);

                    int tileId = -1;
                    if (!cave.Contains(pos))
                    {
                        tileId = tileSequence.GetIndex(new TileDescription(
                            NorthCorners: GetSideCorners(left: pos.Right(), right: pos),
                            EastCorners: GetSideCorners(left: pos.BelowRight(), right: pos.Right()),
                            SouthCorners: GetSideCorners(left: pos.Below(), right: pos.BelowRight()),
                            WestCorners: GetSideCorners(left: pos, right: pos.Below()),
                            FloorCorners: GetCorners(alternateFloor, pos),
                            NorthLight: 0,
                            EastLight: 0,
                            SouthLight: 0,
                            WestLight: 0));
                    }

                    int sectorId = sectorSequence.GetIndex(new SectorDescription(
                        Floor: GetCorners(alternateFloor, pos),
                        Ceiling: GetCorners(alternateCeiling, pos),
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

        private static Corners ToCorners(bool topLeft, bool topRight, bool bottomLeft, bool bottomRight) =>
            (topLeft ? Corners.TopLeft : Corners.None) |
            (bottomLeft ? Corners.BottomLeft : Corners.None) |
            (topRight ? Corners.TopRight : Corners.None) |
            (bottomRight ? Corners.BottomRight : Corners.None);

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
            Corners FloorCorners,
            int NorthLight,
            int EastLight,
            int SouthLight,
            int WestLight);
    }
}