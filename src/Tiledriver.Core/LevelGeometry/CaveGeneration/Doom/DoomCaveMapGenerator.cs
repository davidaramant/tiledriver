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
            SideFront: sectorCache.GetIndex(ld.FrontSector!),
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

    private static IReadOnlyList<EdgeNode> GetEdges(
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
        .SelectMany(pair => pair.Sectors.GetInternalEdges().Select(edge => new EdgeNode(pair.Position, edge)))
        .ToList();

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

    private static void DrawEdges(
        IReadOnlyList<EdgeNode> edges,
        ModelSequence<LatticePoint, Vertex> vertexCache,
        ModelSequence<LineDescription, LineDef> lineCache)
    {
        var segmentGraph = new Dictionary<LatticePoint, HashSet<EdgeNode>>();

        foreach (var edgeNode in edges)
        {
            if (segmentGraph.TryGetValue(edgeNode.StartPoint, out var fromStartSegments))
            {
                fromStartSegments.Add(edgeNode);
            }
            else
            {
                segmentGraph.Add(edgeNode.StartPoint, new HashSet<EdgeNode> { edgeNode });
            }

            if (segmentGraph.TryGetValue(edgeNode.EndPoint, out var fromEndSegments))
            {
                fromEndSegments.Add(edgeNode);
            }
            else
            {
                segmentGraph.Add(edgeNode.EndPoint, new HashSet<EdgeNode> { edgeNode });
            }
        }

        // var spanGraph = new Dictionary<LatticePoint, EdgeSpan>();

        var covered = new HashSet<EdgeNode>();
        foreach (var edgeNode in edges)
        {
            if (covered.Contains(edgeNode))
                continue;

            EdgeNode FollowNode(EdgeNode start, bool goRight)
            {
                var node = start;

                while (true)
                {
                    var nextNode = node.FollowLine(goRight: goRight);
                    if (!covered.Contains(nextNode) &&
                        segmentGraph.TryGetValue(node.GetPointAtEnd(leftSide: goRight), out var connectedEdges) &&
                        connectedEdges.Count == 2 &&
                        connectedEdges.Contains(nextNode))
                    {
                        covered.Add(node);
                        node = nextNode;
                    }
                    else
                    {
                        break;
                    }
                }

                return node;
            }

            var leftNode = FollowNode(edgeNode, goRight: false);
            var rightNode = FollowNode(edgeNode, goRight: true);

            lineCache.GetIndex(new LineDescription(
                LeftVertex: vertexCache.GetIndex(leftNode.StartPoint),
                RightVertex: vertexCache.GetIndex(rightNode.EndPoint),
                FrontSector: leftNode.Segment.Front,
                BackSector: leftNode.Segment.Back));
        }








        // While there are remaining edges;
        // - Find a (perferably) single-sided edge
        //   - Go as far as possible left
        //   - Reverse, but concatenate this time
        //   - Add line to cache

        //(Position, EdgeSegment) FindStartingEdge()
        //{
        //    var edgeSequence = edges.SelectMany(positionAndEdges => positionAndEdges.Value.Values.Select(edge => (Position: positionAndEdges.Key, Edge: edge)));

        //    return edgeSequence.FirstOrDefault(pair => pair.Edge.IsSingleSided, edgeSequence.First());
        //}


        // SectorEdgeGraph actions:
        // - IsEmpty: bool
        // - FindStartingNode(): MapNode
        // - Contains(MapNode): bool
        // - Remove(MapNode)


        //while (edges.Any())
        //{
        //    var (position, edge) = FindStartingEdge();

        //    var (newPosition, newEdge) = edge.FollowLine(goRight: false, position);


        //}
    }
}
