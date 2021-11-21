// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    public sealed class DoomCaveMapGenerator
    {
        private const int LogicalUnitSize = 32;
        private const int SmoothingSteps = 1;
        private const int DetailUnitSize = LogicalUnitSize >> SmoothingSteps;

        public static MapData Create(int seed, TextureQueue textureQueue)
        {
            var random = new Random(seed);

            var logicalBoard = GenerateLogicalBoard(random);
            var fineDetailBoard = logicalBoard.ScaleAndSmooth(SmoothingSteps);

            var vertexCache = new ModelSequence<LogicalPoint, Vertex>(
                p => new Vertex(p.X * DetailUnitSize, p.Y * DetailUnitSize));

            var lineCache = new ModelSequence<LineDescription, LineDef>(
                ld => new LineDef(V1: ld.V1, V2: ld.V2, SideFront: 0));

            var borderTiles = FindBorderTiles(
                fineDetailBoard.Dimensions,
                isCornerInsideMap: p => fineDetailBoard[p] == CellType.Dead);

            DrawEdges(borderTiles, vertexCache, lineCache);

            var firstOpenSpot =
                logicalBoard.GetAllPositions().First(p => logicalBoard[p] == CellType.Dead);

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
                    X: firstOpenSpot.X * LogicalUnitSize + LogicalUnitSize / 2,
                    Y: firstOpenSpot.Y * LogicalUnitSize + LogicalUnitSize / 2,
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
                .ScaleAddNoiseAndSmooth(random,noise:0.2,times:2)
                .RemoveNoise()
                .TrimToLargestDeadArea();

        private enum Side
        {
            Left,
            Top,
            Right,
            Bottom
        }

        private static IReadOnlyList<(Position Position, Corners InMapCorners)> FindBorderTiles(
            Size size,
            Func<Position, bool> isCornerInsideMap) =>
            size.GetAllPositionsExclusiveMax()
                .Select(p => ( p, InMapCorners: Corner.Create(p, isCornerInsideMap) ))
                .Where(tile => tile.InMapCorners != Corners.None && tile.InMapCorners != Corners.All)
                .ToList();

        private static void DrawEdges(
            IReadOnlyList<(Position Position, Corners InMapCorners)> borderTiles,
            ModelSequence<LogicalPoint, Vertex> vertexCache,
            ModelSequence<LineDescription, LineDef> lineCache)
        {
            foreach (var (pos, inMapCorners) in borderTiles)
            {
                void DrawLine(Position p, Side fromSide, Side toSide) =>
                    lineCache.GetIndex(
                        new LineDescription(
                            vertexCache.GetIndex(GetMiddleOfSide(p, toSide)),
                            vertexCache.GetIndex(GetMiddleOfSide(p, fromSide))));

                switch (inMapCorners)
                {
                    case Corners.BottomLeft: DrawLine(pos, Side.Left, Side.Bottom); break;
                    case Corners.BottomRight: DrawLine(pos, Side.Bottom, Side.Right); break;
                    case Corners.TopRight: DrawLine(pos, Side.Right, Side.Top); break;
                    case Corners.TopLeft: DrawLine(pos, Side.Top, Side.Left); break;

                    case Corners.ExceptBottomLeft: DrawLine(pos, Side.Bottom, Side.Left); break;
                    case Corners.ExceptBottomRight: DrawLine(pos, Side.Right, Side.Bottom); break;
                    case Corners.ExceptTopLeft: DrawLine(pos, Side.Left, Side.Top); break;
                    case Corners.ExceptTopRight: DrawLine(pos, Side.Top, Side.Right); break;

                    case Corners.Top: DrawLine(pos, Side.Right, Side.Left); break;
                    case Corners.Bottom: DrawLine(pos, Side.Left, Side.Right); break;
                    case Corners.Left: DrawLine(pos, Side.Top, Side.Bottom); break;
                    case Corners.Right: DrawLine(pos, Side.Bottom, Side.Top); break;

                    case Corners.TopLeftAndBottomRight:
                        DrawLine(pos, Side.Bottom, Side.Left);
                        DrawLine(pos, Side.Top, Side.Right);
                        break;
                    case Corners.TopRightAndBottomLeft:
                        DrawLine(pos, Side.Left, Side.Top);
                        DrawLine(pos, Side.Right, Side.Bottom);
                        break;

                    default:
                        break;
                }
            }
        }

        private static LogicalPoint GetMiddleOfSide(Position p, Side side) =>
            side switch
            {
                Side.Left => new(p.X, p.Y + 0.5),
                Side.Top => new(p.X + 0.5, p.Y),
                Side.Right => new(p.X + 1, p.Y + 0.5),
                Side.Bottom => new(p.X + 0.5, p.Y + 1),
                _ => throw new Exception("Impossible")
            };

        private sealed record LineDescription(
            int V1,
            int V2);

        private sealed record LogicalPoint(
            double X,
            double Y);

        public enum LineDirection
        {
            Left,
            UpLeft,
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
        }

        public sealed record Line(
            Position Start,
            LineDirection Direction,
            int Length
        );
    }
}