// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.Extensions.Collections;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.GameInfo.Doom;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;
using static Tiledriver.Core.Utils.MathUtil;

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

        var vertexCache = new ModelSequence<LogicalPoint, Vertex>(ConvertToVertex);
        var sectorCache = new ModelSequence<SectorDescription, Sector>(ConvertToSector);
        var lineCache = new ModelSequence<LineDescription, LineDef>(ld => ConvertToLineDef(ld, sectorCache));

        //var borderTiles = FindBorderTiles(
        //    geometryBoard.Dimensions,
        //    isCornerInsideMap: p => geometryBoard[p] == CellType.Dead);

        var edges = GetEdges(boardSize, internalDistances);

        //DrawEdges2(borderTiles2, vertexCache, lineCache, sectorCache);

        //DrawEdges(borderTiles, vertexCache, lineCache);

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

    private static Vertex ConvertToVertex(LogicalPoint p) => new(p.X * LogicalUnitSize, p.Y * LogicalUnitSize);
    private static LineDef ConvertToLineDef(LineDescription ld, ModelSequence<SectorDescription, Sector> sectorCache) =>
        new(
            V1: ld.LeftVertex,
            V2: ld.RightVertex,
            TwoSided: !ld.BackSector.IsOutside,
            SideFront: sectorCache.GetIndex(ld.FrontSector!),
            SideBack: ld.BackSector.IsOutside ? -1 : sectorCache.GetIndex(ld.BackSector));
    private static Sector ConvertToSector(SectorDescription sd) => new(
                TextureFloor: new Texture("RROCK16"),
                TextureCeiling: new Texture("FLAT10"),
                HeightFloor: 0,
                HeightCeiling: 128,
                LightLevel: 16 * sd.HeightLevel);

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

    private static IReadOnlyDictionary<Position, IReadOnlyList<InternalEdge>> GetEdges(
        Size size,
        IReadOnlyDictionary<Position, int> interiorDistances) =>
        size.GetAllPositionsExclusiveMax()
        .Select(p =>
        {
            var heightLookup = GetHeightLookup(interiorDistances, p);

            var sectorIds =
                SquareSegmentsExtensions.GetAllSegments()
                .Select(seq => new SectorDescription(HeightLevel: heightLookup(seq)))
                .ToArray();

            return (Position: p, Sectors: new SquareSegmentSectors(sectorIds));
        })
        .Where(t => !t.Sectors.IsUniform)
        .ToDictionary(pair => pair.Position, pair => (IReadOnlyList<InternalEdge>)pair.Sectors.GetEdges().ToList());

    private static Func<SquareSegment, int> GetHeightLookup(
        IReadOnlyDictionary<Position, int> interiorDistances,
        Position position)
    {
        var upperLeft = interiorDistances.TryGet(position) ?? -1;
        var upperRight = interiorDistances.TryGet(position.Right()) ?? -1;
        var lowerLeft = interiorDistances.TryGet(position.Below()) ?? -1;
        var lowerRight = interiorDistances.TryGet(position.BelowRight()) ?? -1;

        // Because this is operating on internal distances from walls, there are only 1 or 2 different values for the
        // corners.
        var minHeight = Min(upperLeft, upperRight, lowerLeft, lowerRight);
        var maxHeight = Max(upperLeft, upperRight, lowerLeft, lowerRight);

        var segments = Corner.Create(
            topLeft: upperLeft == maxHeight,
            topRight: upperRight == maxHeight,
            bottomLeft: lowerLeft == maxHeight,
            bottomRight: lowerRight == maxHeight).ToSquareSegments();

        return seg => segments.HasFlag(seg.ToSquareSegments()) ? maxHeight : minHeight;
    }

    sealed class SquareSegmentSectors
    {
        private readonly SectorDescription[] _sectors;

        public SquareSegmentSectors(IEnumerable<SectorDescription> sectors) => _sectors = sectors.ToArray();

        public SectorDescription this[SquareSegment id] => _sectors[(int)id];
        public bool IsUniform => _sectors.Skip(1).All(s => s == _sectors[0]);

        public IEnumerable<InternalEdge> GetEdges() =>
            new InternalEdge[]
            {
                new InternalEdge.DiagTopLeft(
                    UpperLeftOuter:this[SquareSegment.UpperLeftOuter],
                    UpperLeftInner:this[SquareSegment.UpperLeftInner]),
                new InternalEdge.DiagTopRight(
                    UpperRightOuter:this[SquareSegment.UpperRightOuter],
                    UpperRightInner:this[SquareSegment.UpperRightInner]),
                new InternalEdge.DiagBottomRight(
                    LowerRightOuter:this[SquareSegment.LowerRightOuter],
                    LowerRightInner:this[SquareSegment.LowerRightInner]),
                new InternalEdge.DiagBottomLeft(
                    LowerLeftOuter:this[SquareSegment.LowerLeftOuter],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
                new InternalEdge.HorizontalLeft(
                    UpperLeftInner:this[SquareSegment.UpperLeftInner],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
                new InternalEdge.HorizontalRight(
                    UpperRightInner:this[SquareSegment.UpperRightInner],
                    LowerRightInner:this[SquareSegment.LowerRightInner]),
                new InternalEdge.VerticalTop(
                    UpperLeftInner:this[SquareSegment.UpperLeftInner],
                    UpperRightInner:this[SquareSegment.UpperRightInner]),
                new InternalEdge.VerticalBottom(
                    LowerRightInner:this[SquareSegment.LowerRightInner],
                    LowerLeftInner:this[SquareSegment.LowerLeftInner]),
            }.Where(ie => ie.IsValid);
    }

    abstract record InternalEdge(bool IsValid)
    {
        public sealed record DiagTopLeft(
            SectorDescription UpperLeftOuter,
            SectorDescription UpperLeftInner)
            : InternalEdge(UpperLeftOuter != UpperLeftInner);
        public sealed record DiagTopRight(
            SectorDescription UpperRightOuter,
            SectorDescription UpperRightInner)
            : InternalEdge(UpperRightOuter != UpperRightInner);
        public sealed record DiagBottomRight(
            SectorDescription LowerRightOuter,
            SectorDescription LowerRightInner)
            : InternalEdge(LowerRightOuter != LowerRightInner);
        public sealed record DiagBottomLeft(
            SectorDescription LowerLeftOuter,
            SectorDescription LowerLeftInner)
            : InternalEdge(LowerLeftOuter != LowerLeftInner);
        public sealed record HorizontalLeft(
            SectorDescription UpperLeftInner,
            SectorDescription LowerLeftInner)
            : InternalEdge(UpperLeftInner != LowerLeftInner);
        public sealed record HorizontalRight(
            SectorDescription UpperRightInner,
            SectorDescription LowerRightInner)
            : InternalEdge(UpperRightInner != LowerRightInner);
        public sealed record VerticalTop(
            SectorDescription UpperLeftInner,
            SectorDescription UpperRightInner)
            : InternalEdge(UpperLeftInner != UpperRightInner);
        public sealed record VerticalBottom(
            SectorDescription LowerRightInner,
            SectorDescription LowerLeftInner)
            : InternalEdge(LowerRightInner != LowerLeftInner);
    }

    //private static void DrawEdges(
    //    IReadOnlyList<(Position Position, Corners InMapCorners)> borderTiles,
    //    ModelSequence<LogicalPoint, Vertex> vertexCache,
    //    ModelSequence<LineDescription, LineDef> lineCache)
    //{
    //    foreach (var (pos, inMapCorners) in borderTiles)
    //    {
    //        void DrawLine(Position p, Side fromSide, Side toSide) =>
    //            lineCache.GetIndex(
    //                new LineDescription(
    //                    vertexCache.GetIndex(GetMiddleOfSide(p, toSide)),
    //                    vertexCache.GetIndex(GetMiddleOfSide(p, fromSide))));

    //        switch (inMapCorners)
    //        {
    //            case Corners.LowerLeft: DrawLine(pos, Side.Left, Side.Bottom); break;
    //            case Corners.LowerRight: DrawLine(pos, Side.Bottom, Side.Right); break;
    //            case Corners.UpperRight: DrawLine(pos, Side.Right, Side.Top); break;
    //            case Corners.UpperLeft: DrawLine(pos, Side.Top, Side.Left); break;

    //            case Corners.AllButLowerLeft: DrawLine(pos, Side.Bottom, Side.Left); break;
    //            case Corners.AllButLowerRight: DrawLine(pos, Side.Right, Side.Bottom); break;
    //            case Corners.AllButUpperLeft: DrawLine(pos, Side.Left, Side.Top); break;
    //            case Corners.AllButUpperRight: DrawLine(pos, Side.Top, Side.Right); break;

    //            case Corners.Upper: DrawLine(pos, Side.Right, Side.Left); break;
    //            case Corners.Lower: DrawLine(pos, Side.Left, Side.Right); break;
    //            case Corners.Left: DrawLine(pos, Side.Top, Side.Bottom); break;
    //            case Corners.Right: DrawLine(pos, Side.Bottom, Side.Top); break;

    //            case Corners.UpperLeftAndLowerRight:
    //                DrawLine(pos, Side.Bottom, Side.Left);
    //                DrawLine(pos, Side.Top, Side.Right);
    //                break;
    //            case Corners.UpperRightAndLowerLeft:
    //                DrawLine(pos, Side.Left, Side.Top);
    //                DrawLine(pos, Side.Right, Side.Bottom);
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //}

    private static void DrawEdges2(
        IEnumerable<(Position Position, SquareSegmentSectors Sectors)> borderTiles,
        ModelSequence<LogicalPoint, Vertex> vertexCache,
        ModelSequence<LineDescription, LineDef> lineCache,
        ModelSequence<SectorDescription, Sector> sectorCache)
    {
        // Loop over tiles
        //   Get all line segments from sector segments (new method in SquareSegmentSectors?)
        //     This will loop over all the segment ids
        //     If [this]!=[next], there should be a line segment between them
        //     Create line description based on that

        throw new NotImplementedException();
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

    // TODO: LeftVertex & RightVertex should not be indices because of simplification
    private sealed record LineDescription(
        int LeftVertex,
        int RightVertex,
        SectorDescription FrontSector,
        SectorDescription BackSector);

    private sealed record SectorDescription(
        int HeightLevel)
    {
        public static readonly SectorDescription Outside = new(-1);
        public bool IsOutside => this == Outside;
    }

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

    // TODO: Not used yet - will be useful when simplifying geometry
    public sealed record Line(
        Position Start,
        LineDirection Direction,
        int Length
    );
}
