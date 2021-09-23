// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
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
        private const int SmoothingSteps = 2;
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

            var edges = FindEdges(
                fineDetailBoard.Dimensions,
                isCornerInsideMap: p => fineDetailBoard[p] == CellType.Dead);

            DrawEdges(edges, vertexCache, lineCache);

            var firstOpenSpot =
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
                .TrimToLargestDeadArea();

        private enum Side
        {
            Left,
            Top,
            Right,
            Bottom
        }

        private static IReadOnlyDictionary<Position, Corners> FindEdges(
            Size size,
            Func<Position, bool> isCornerInsideMap)
        {
            var edges = new Dictionary<Position, Corners>();

            for (int y = 0; y < size.Height - 1; y++)
            {
                for (int x = 0; x < size.Width - 1; x++)
                {
                    var p = new Position(x, y);
                    var inMapCorners = Corner.Create(p, isCornerInsideMap);
                    if (inMapCorners != Corners.None && inMapCorners != Corners.All)
                        edges[p] = inMapCorners;
                }
            }

            return edges;
        }

        private static void DrawEdges(IReadOnlyDictionary<Position, Corners> edges,
            ModelSequence<LogicalPoint, Vertex> vertexCache,
            ModelSequence<LineDescription, LineDef> lineCache)
        {
            foreach (var pair in edges)
            {
                LogicalPoint GetMiddleOfSide(Position p, Side side) => side switch
                {
                    Side.Left => new(p.X, p.Y + 0.5),
                    Side.Top => new(p.X + 0.5, p.Y),
                    Side.Right => new(p.X + 1, p.Y + 0.5),
                    Side.Bottom => new(p.X + 0.5, p.Y + 1),
                    _ => throw new Exception("Impossible")
                };

                void DrawLine(Position pos, Side fromSide, Side toSide) =>
                    lineCache.GetIndex(
                        new LineDescription(
                            vertexCache.GetIndex(GetMiddleOfSide(pos, toSide)),
                            vertexCache.GetIndex(GetMiddleOfSide(pos, fromSide))));

                var p = pair.Key;
                var inMapCorners = pair.Value;

                switch (inMapCorners)
                {
                    case Corners.BottomLeft: DrawLine(p, Side.Left, Side.Bottom); break;
                    case Corners.BottomRight: DrawLine(p, Side.Bottom, Side.Right); break;
                    case Corners.TopRight: DrawLine(p, Side.Right, Side.Top); break;
                    case Corners.TopLeft: DrawLine(p, Side.Top, Side.Left); break;

                    case Corners.ExceptBottomLeft: DrawLine(p, Side.Bottom, Side.Left); break;
                    case Corners.ExceptBottomRight: DrawLine(p, Side.Right, Side.Bottom); break;
                    case Corners.ExceptTopLeft: DrawLine(p, Side.Left, Side.Top); break;
                    case Corners.ExceptTopRight: DrawLine(p, Side.Top, Side.Right); break;

                    case Corners.Top: DrawLine(p, Side.Right, Side.Left); break;
                    case Corners.Bottom: DrawLine(p, Side.Left, Side.Right); break;
                    case Corners.Left: DrawLine(p, Side.Top, Side.Bottom); break;
                    case Corners.Right: DrawLine(p, Side.Bottom, Side.Top); break;

                    case Corners.TopLeftAndBottomRight:
                        DrawLine(p, Side.Bottom, Side.Left);
                        DrawLine(p, Side.Top, Side.Right);
                        break;
                    case Corners.TopRightAndBottomLeft:
                        DrawLine(p, Side.Left, Side.Top);
                        DrawLine(p, Side.Right, Side.Bottom);
                        break;

                    default:
                        break;
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