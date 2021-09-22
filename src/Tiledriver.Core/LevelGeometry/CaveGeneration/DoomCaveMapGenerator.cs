// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.Utils.CellularAutomata;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public sealed class DoomCaveMapGenerator
    {
        private const int LogicalUnitSize = 64;

        public static MapData Create(int seed, TextureQueue textureQueue)
        {
            var random = new Random(seed);

            var logicalBoard = GenerateLogicalBoard(random);

            var vertexCache = new ModelSequence<LogicalPoint, Vertex>(
                p => new Vertex(p.X * LogicalUnitSize, p.Y * LogicalUnitSize));

            var lineCache = new ModelSequence<LineDescription, LineDef>(
                ld => new LineDef(V1: ld.V1, V2: ld.V2, SideFront: 0));

            DrawOneSidedWalls(
                logicalBoard.Dimensions,
                isPointInsideMap: p => logicalBoard[p] == CellType.Dead,
                vertexCache,
                lineCache);

            var firstSpot =
                (from y in Enumerable.Range(0, logicalBoard.Height)
                 from x in Enumerable.Range(0, logicalBoard.Width)
                 select new Position(x, y)).First(p => logicalBoard[p] == CellType.Dead);

            return new MapData(
                NameSpace: "Doom",
                LineDefs: lineCache.GetDefinitions().ToImmutableArray(),
                SideDefs: ImmutableArray.Create(new SideDef(
                    TextureTop: Texture.None,
                    TextureMiddle: new Texture("ROCK5"),
                    TextureBottom: Texture.None,
                    Sector: 0)),
                Vertices: vertexCache.GetDefinitions().ToImmutableArray(),
                Sectors: ImmutableArray.Create(new Sector(
                    TextureFloor: new Texture("RROCK16"),
                    TextureCeiling: new Texture("FLAT10"),
                    HeightFloor: 0,
                    HeightCeiling: 128,
                    LightLevel: 192)),
                Things: ImmutableArray.Create(new Thing(
                    X: firstSpot.X * LogicalUnitSize + LogicalUnitSize / 2,
                    Y: firstSpot.Y * LogicalUnitSize + LogicalUnitSize / 2,
                    Angle: 90,
                    Type: 1,
                    Skill1: true,
                    Skill2: true,
                    Skill3: true,
                    Skill4: true,
                    Skill5: true,
                    Single: true,
                    Dm: true,
                    Coop: true
                )));
        }

        private static CellBoard GenerateLogicalBoard(Random random) =>
            new CellBoard(new Size(128, 128))
                .Fill(random, probabilityAlive: 0.5)
                .MakeBorderAlive(thickness: 1)
                .GenerateStandardCave()
                .TrimToLargestDeadArea();

        private static void DrawOneSidedWalls(
            Size size,
            Func<Position, bool> isPointInsideMap,
            ModelSequence<LogicalPoint, Vertex> vertexCache,
            ModelSequence<LineDescription, LineDef> lineCache)
        {
            void Line(LogicalPoint fromPoint, LogicalPoint toPoint) =>
                lineCache.GetIndex(
                    new LineDescription(
                        vertexCache.GetIndex(toPoint),
                        vertexCache.GetIndex(fromPoint)));

            for (int y = 0; y < size.Height - 1; y++)
            {
                for (int x = 0; x < size.Width - 1; x++)
                {
                    var p = new Position(x, y);
                    var inMapCorners = Corner.Create(p, isPointInsideMap);

                    LogicalPoint center = new(x + 0.5, y + 0.5);
                    LogicalPoint middleTop = new(x + 0.5, y);
                    LogicalPoint middleRight = new(x + 1, y + 0.5);
                    LogicalPoint middleBottom = new(x + 0.5, y + 1);
                    LogicalPoint middleLeft = new(x, y + 0.5);

                    switch (inMapCorners)
                    {
                        case Corners.BottomLeft: Line(middleLeft, middleBottom); break;
                        case Corners.BottomRight: Line(middleBottom, middleRight); break;
                        case Corners.TopRight: Line(middleRight, middleTop); break;
                        case Corners.TopLeft: Line(middleTop, middleLeft); break;

                        case Corners.ExceptBottomLeft: Line(middleBottom, middleLeft); break;
                        case Corners.ExceptBottomRight: Line(middleRight, middleBottom); break;
                        case Corners.ExceptTopLeft: Line(middleLeft, middleTop); break;
                        case Corners.ExceptTopRight: Line(middleTop, middleRight); break;

                        case Corners.Top: Line(middleRight, middleLeft); break;
                        case Corners.Bottom: Line(middleLeft, middleRight); break;
                        case Corners.Left: Line(middleTop, middleBottom); break;
                        case Corners.Right: Line(middleBottom, middleTop); break;

                        case Corners.TopLeftAndBottomRight:
                            Line(middleBottom, middleLeft);
                            Line(middleTop, middleRight);
                            break;
                        case Corners.TopRightAndBottomLeft:
                            Line(middleLeft, middleTop);
                            Line(middleRight, middleBottom);
                            break;

                        case Corners.None:
                        case Corners.All:
                        default:
                            break;
                    }
                }
            }
        }

        private sealed record LineDescription(
            int V1,
            int V2);

        private sealed record LogicalPoint(
            double X,
            double Y);

    }
}