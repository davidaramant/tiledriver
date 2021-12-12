// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.GameInfo.Doom;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed class DoomCaveMapGenerator
{
    private const int LogicalUnitSize = 16;

    public static MapData Create(int seed, TextureQueue textureQueue)
    {
        var random = new Random(seed);

        CellBoard geometryBoard = GenerateGeometryBoard(random);
        (ConnectedArea playableSpace, Size boardSize) = geometryBoard.TrimToLargestDeadConnectedArea();

        var internalDistances = playableSpace.DetermineInteriorEdgeDistance(Neighborhood.Moore);
        // TODO: Feed the above into FindBorderTiles

        var vertexCache = new ModelSequence<LogicalPoint, Vertex>(
            p => new Vertex(p.X * LogicalUnitSize, p.Y * LogicalUnitSize));

        var lineCache = new ModelSequence<LineDescription, LineDef>(
            ld => new LineDef(V1: ld.V1, V2: ld.V2, SideFront: 0));

        var borderTiles = FindBorderTiles(
            geometryBoard.Dimensions,
            isCornerInsideMap: p => geometryBoard[p] == CellType.Dead);

        DrawEdges(borderTiles, vertexCache, lineCache);

        var playerLogicalSpot = FindPlayerSpot(geometryBoard);

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
            Things: ImmutableArray.Create(
                Actor.Player1Start.MakeThing(
                    x: playerLogicalSpot.X * LogicalUnitSize + LogicalUnitSize / 2,
                    y: playerLogicalSpot.Y * LogicalUnitSize + LogicalUnitSize / 2,
                    angle: 90)));
    }

    private static Position FindPlayerSpot(CellBoard board)
    {
        // Add a buffer so the player doesn't start out stuck in the walls
        var squaresToCheck = (int)Math.Ceiling((double)Actor.Player1Start.Width / LogicalUnitSize) + 2;

        return
            board.Dimensions
            .GetAllPositions()
            .First(p =>
                (from yd in Enumerable.Range(0, squaresToCheck)
                 from xd in Enumerable.Range(0, squaresToCheck)
                 select p + new PositionDelta(xd, yd))
                 .All(p2 => board[p2] == CellType.Dead))
            + new PositionDelta(1, 1);
    }

    private static CellBoard GenerateGeometryBoard(Random random) =>
        new CellBoard(new Size(128, 128))
            .Fill(random, probabilityAlive: 0.5)
            .MakeBorderAlive(thickness: 1)
            .GenerateStandardCave()
            .ScaleAddNoiseAndSmooth(random, noise: 0.2, times: 2)
            .RemoveNoise()
            .TrimToLargestDeadArea()
            .ScaleAndSmooth();

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
            .Select(p => (p, InMapCorners: Corner.Create(p, isCornerInsideMap)))
            .Where(tile => tile.InMapCorners != Corners.None && tile.InMapCorners != Corners.All)
            .ToList();

    // Check all layers for corners
    // - Turn corners into SquareSegments
    // Turn layers of SquareSegments into sectorId[SquareSegments] (-1 is outside I guess)
    // Filter out ones where all sectorId values are the same
    // LATER iterate over list to find line segments
    //
    // What does this return? (Position, sectorId[SquareSegments]) ?
    private static IReadOnlyList<(Position Position, Corners InMapCorners)> FindBorderTiles2(
        Size size,
        Func<Position, bool> isCornerInsideMap) =>
        throw new NotImplementedException();

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
                case Corners.LowerLeft: DrawLine(pos, Side.Left, Side.Bottom); break;
                case Corners.LowerRight: DrawLine(pos, Side.Bottom, Side.Right); break;
                case Corners.UpperRight: DrawLine(pos, Side.Right, Side.Top); break;
                case Corners.UpperLeft: DrawLine(pos, Side.Top, Side.Left); break;

                case Corners.AllButLowerLeft: DrawLine(pos, Side.Bottom, Side.Left); break;
                case Corners.AllButLowerRight: DrawLine(pos, Side.Right, Side.Bottom); break;
                case Corners.AllButUpperLeft: DrawLine(pos, Side.Left, Side.Top); break;
                case Corners.AllButUpperRight: DrawLine(pos, Side.Top, Side.Right); break;

                case Corners.Upper: DrawLine(pos, Side.Right, Side.Left); break;
                case Corners.Lower: DrawLine(pos, Side.Left, Side.Right); break;
                case Corners.Left: DrawLine(pos, Side.Top, Side.Bottom); break;
                case Corners.Right: DrawLine(pos, Side.Bottom, Side.Top); break;

                case Corners.UpperLeftAndLowerRight:
                    DrawLine(pos, Side.Bottom, Side.Left);
                    DrawLine(pos, Side.Top, Side.Right);
                    break;
                case Corners.UpperRightAndLowerLeft:
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
