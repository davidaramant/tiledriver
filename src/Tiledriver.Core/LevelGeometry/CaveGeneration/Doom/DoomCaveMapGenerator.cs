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
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Tiledriver.Core.LevelGeometry.Extensions;
using Tiledriver.Core.Utils.CellularAutomata;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

public sealed class DoomCaveMapGenerator
{
    public const int LogicalUnitSize = 16;
    public const int TextureWidth = 128;

    public static MapData Create(int seed, TextureQueue textureQueue)
    {
        var random = new Random(seed);

        CellBoard geometryBoard = GenerateGeometryBoard(random);
        (ConnectedArea playableSpace, Size boardSize) = geometryBoard.TrimToLargestDeadConnectedArea();

        var internalDistances = playableSpace.DetermineInteriorEdgeDistance(Neighborhood.Moore);

        var sideDefCache = new ModelSequence<SideDef, SideDef>(s => s); // Seems silly, but what should be abstracted about it?
        var vertexCache = new ModelSequence<LatticePoint, Vertex>(ConvertToVertex);
        var sectorCache = new ModelSequence<SectorDescription, Sector>(ConvertToSector);
        var lineCache = new ModelSequence<LineDescription, LineDef>(ld => ConvertToLineDef(ld, sectorCache, sideDefCache));

        var edges = GetEdges(boardSize, internalDistances);

        DrawEdges(edges, vertexCache, lineCache);

        var playerLogicalSpot = FindPlayerSpot(geometryBoard);

        return new MapData(
            NameSpace: "Doom",
            LineDefs: lineCache.GetDefinitions().ToImmutableArray(),
            SideDefs: sideDefCache.GetDefinitions().ToImmutableArray(),
            Vertices: vertexCache.GetDefinitions().ToImmutableArray(),
            Sectors: sectorCache.GetDefinitions().ToImmutableArray(),
            Things: ImmutableArray.Create(
                Actor.Player1Start.MakeThing(
                    x: playerLogicalSpot.X * LogicalUnitSize + LogicalUnitSize / 2,
                    y: playerLogicalSpot.Y * LogicalUnitSize + LogicalUnitSize / 2,
                    angle: 90)));
    }

    private static Vertex ConvertToVertex(LatticePoint lp) =>
        lp.Point switch
        {
            SquarePoint.LeftMiddle => new(lp.Square.X * LogicalUnitSize, (lp.Square.Y + 0.5) * LogicalUnitSize),
            SquarePoint.BottomMiddle => new((lp.Square.X + 0.5) * LogicalUnitSize, lp.Square.Y * LogicalUnitSize),
            SquarePoint.Center => new((lp.Square.X + 0.5) * LogicalUnitSize, (lp.Square.Y + 0.5) * LogicalUnitSize),
            _ => throw new Exception("Impossible; Vertices should have been normalized to prevent this")
        };

    private static LineDef ConvertToLineDef(
        LineDescription ld,
        ModelSequence<SectorDescription, Sector> sectorCache,
        ModelSequence<SideDef, SideDef> sideDefCache)
    {
        var frontSide = sideDefCache.GetIndex(new SideDef(
            sector: sectorCache.GetIndex(ld.FrontSector),
            textureMiddle: ld.IsTwoSided ? null : new Texture("BIGDOOR2"),
            offsetX: ld.TextureXOffset));
        var backSide = ld.IsTwoSided
            ? sideDefCache.GetIndex(new SideDef(
                sector: sectorCache.GetIndex(ld.BackSector),
                textureTop: new Texture("BIGDOOR2"),
                textureBottom: new Texture("BIGDOOR2"),
                offsetX: ld.TextureXOffset))
            : -1;

        return new(
            V1: ld.RightVertex,
            V2: ld.LeftVertex,
            TwoSided: ld.IsTwoSided,
            SideFront: frontSide,
            SideBack: backSide);
    }
    private static Sector ConvertToSector(SectorDescription sd) => new(
                TextureFloor: "FLOOR6_1",
                TextureCeiling: "FLOOR6_1",
                HeightFloor: 0 - sd.HeightLevel * 4,
                HeightCeiling: 128 + sd.HeightLevel * 8,
                LightLevel: 255);

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
            .ScaleAddNoiseAndSmooth(random, noise: 0.2, times: 1)
            .RemoveNoise()
            .TrimToLargestDeadArea()
            .ScaleAndSmooth();

    private static IReadOnlyList<SectorEdge> GetEdges(
        Size size,
        IReadOnlyDictionary<Position, int> interiorDistances) =>
        size.GetAllPositionsExclusiveMax()
        .Select(p =>
        {
            var heightLookup = SquareLayerTransition.GetHeightLookup(interiorDistances, p);

            // TODO: this happens to work because of the -1 thing
            var sectorIds =
                SquareSegmentsExtensions.GetAllSegments()
                .Select(seq => new SectorDescription(HeightLevel: heightLookup(seq)))
                .ToArray();

            return (Position: p, Sectors: new SquareSegmentSectors(sectorIds));
        })
        .Where(t => !t.Sectors.IsUniform)
        .SelectMany(pair => pair.Sectors.GetInternalEdges().Select(edge => SectorEdge.FromPosition(pair.Position, edge)))
        .ToList();

    private static void DrawEdges(
        IReadOnlyList<SectorEdge> edges,
        ModelSequence<LatticePoint, Vertex> vertexCache,
        ModelSequence<LineDescription, LineDef> lineCache)
    {
        var segmentGraph = SectorEdgeGraph.FromEdges(edges);
        var edgeGraph = segmentGraph.Simplify();
        Console.Out.WriteLine($"Number of edges simplified from {segmentGraph.EdgeCount:N0} to {edgeGraph.EdgeCount:N0}");

        var remainingEdges = edgeGraph.GetAllEdges().ToHashSet();

        double Distance(LatticePoint p1, LatticePoint p2)
        {
            var v1 = ConvertToVertex(p1);
            var v2 = ConvertToVertex(p2);

            var dx = v1.X - v2.X;
            var dy = v1.Y - v2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // TODO with texture offsets:
        // - Something is wrong with the offsets. Does it have something to do with the lines being flipped from what I think a Doom level has?
        // - How do offsets work for the back of a line?

        while (remainingEdges.Any())
        {
            var startingEdge = remainingEdges.FirstOrDefault(e => e.IsSingleSided) ?? remainingEdges.First();

            var stack = new Stack<(SectorEdge Edge, double Offset)>();
            stack.Push((startingEdge, 0));

            while (stack.Any())
            {
                var (edge, offset) = stack.Pop();

                if (!remainingEdges.Contains(edge))
                    continue;

                remainingEdges.Remove(edge);

                lineCache.GetIndex(new LineDescription(
                    LeftVertex: vertexCache.GetIndex(edge.Start),
                    RightVertex: vertexCache.GetIndex(edge.End),
                    FrontSector: edge.Segment.Front,
                    BackSector: edge.Segment.Back,
                    TextureXOffset: (int)Math.Round(offset)));

                var length = Distance(edge.Start, edge.End);

                offset = (offset + length) % TextureWidth;

                var connected = edgeGraph.GetEdgesConnectedTo(edge.End);

                foreach (var connectedEdge in connected)
                {
                    stack.Push((connectedEdge, offset));
                }
            }
        }
    }
}
