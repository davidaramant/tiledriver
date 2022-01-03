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
    public const int LogicalUnitSize = 16;

    public static MapData Create(int seed, TextureQueue textureQueue)
    {
        var random = new Random(seed);

        CellBoard geometryBoard = GenerateGeometryBoard(random);
        (ConnectedArea playableSpace, Size boardSize) = geometryBoard.TrimToLargestDeadConnectedArea();

        var internalDistances = playableSpace.DetermineInteriorEdgeDistance(Neighborhood.Moore);

        var vertexCache = new ModelSequence<LatticePoint, Vertex>(ConvertToVertex);
        var sectorCache = new ModelSequence<SectorDescription, Sector>(ConvertToSector);
        var lineCache = new ModelSequence<LineDescription, LineDef>(ld => ConvertToLineDef(ld, sectorCache));
        // TODO: Need sideDefCache too!!!

        var edges = GetEdges(boardSize, internalDistances);

        DrawEdges(edges, vertexCache, lineCache);

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
            SquarePoint.TopMiddle => new((lp.Square.X + 0.5) * LogicalUnitSize, lp.Square.Y * LogicalUnitSize),
            SquarePoint.Center => new((lp.Square.X + 0.5) * LogicalUnitSize, (lp.Square.Y + 0.5) * LogicalUnitSize),
            _ => throw new Exception("Impossible; Vertices should have been normalized to prevent this")
        };

    private static LineDef ConvertToLineDef(LineDescription ld, ModelSequence<SectorDescription, Sector> sectorCache) =>
        new(
            V1: ld.RightVertex,
            V2: ld.LeftVertex,
            TwoSided: !ld.BackSector.IsOutsideLevel,
            SideFront: sectorCache.GetIndex(ld.FrontSector!), // TODO: WRONG! this is a side def index!!!!
            SideBack: ld.BackSector.IsOutsideLevel ? -1 : sectorCache.GetIndex(ld.BackSector));
    private static Sector ConvertToSector(SectorDescription sd) => new(
                TextureFloor: new Texture(sd.HeightLevel % 2 == 0 ? "RROCK16" : "FLAT10"),
                TextureCeiling: new Texture(sd.HeightLevel % 2 == 1 ? "RROCK16" : "FLAT10"),
                HeightFloor: 0,
                HeightCeiling: 128,
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
            .ScaleAddNoiseAndSmooth(random, noise: 0.2, times: 2)
            .RemoveNoise()
            .TrimToLargestDeadArea()
            .ScaleAndSmooth();

    private static IReadOnlyList<EdgeSpan> GetEdges(
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
        .SelectMany(pair => pair.Sectors.GetInternalEdges().Select(edge => EdgeSpan.FromPosition(pair.Position, edge)))
        .ToList();

    private static void DrawEdges(
        IReadOnlyList<EdgeSpan> edges,
        ModelSequence<LatticePoint, Vertex> vertexCache,
        ModelSequence<LineDescription, LineDef> lineCache)
    {
        var segmentGraph = EdgeGraph.FromEdges(edges);
        var spanGraph = segmentGraph.Simplify();

        foreach (var edgeSpan in spanGraph.GetAllEdges())
        {
            lineCache.GetIndex(new LineDescription(
                LeftVertex: vertexCache.GetIndex(edgeSpan.Start),
                RightVertex: vertexCache.GetIndex(edgeSpan.End),
                FrontSector: edgeSpan.Segment.Front,
                BackSector: edgeSpan.Segment.Back));
        }
    }
}
