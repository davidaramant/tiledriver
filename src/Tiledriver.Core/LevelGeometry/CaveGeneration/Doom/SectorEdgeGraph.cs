// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

using Graph = Dictionary<LatticePoint, HashSet<SectorEdge>>;

public sealed class SectorEdgeGraph
{
    private readonly List<SectorEdge> _edges;
    private readonly Graph _pointToEdges;

    public int EdgeCount => _edges.Count;

    private SectorEdgeGraph(
        List<SectorEdge> edges,
        Graph pointToEdges)
    {
        _edges = edges;
        _pointToEdges = pointToEdges;
    }

    public static SectorEdgeGraph FromEdges(IEnumerable<SectorEdge> edges)
    {
        var allEdges = edges.ToList();

        var pointToEdges = new Graph();

        foreach (var edge in allEdges)
        {
            AddToGraph(pointToEdges, edge);
        }

        return new(allEdges, pointToEdges);
    }

    public IEnumerable<SectorEdge> GetAllEdges() => _edges;

    public IEnumerable<SectorEdge> GetEdgesConnectedTo(LatticePoint point) => 
        _pointToEdges.TryGetValue(point, out var edges) 
        ? edges 
        : Enumerable.Empty<SectorEdge>();

    public SectorEdgeGraph Simplify()
    {
        var allSimplifiedEdges = new List<SectorEdge>();
        var simplifiedPointToEdges = new Graph();

        var covered = new HashSet<SectorEdge>();
        foreach (var edge in _edges)
        {
            if (covered.Contains(edge))
                continue;

            covered.Add(edge);

            (SectorEdge End, int StepsTaken) FollowNode(SectorEdge initial, bool goRight)
            {
                var node = initial;
                int steps = 0;

                while (true)
                {
                    if (_pointToEdges.TryGetValue(node.GetPointAtEnd(leftSide: !goRight), out var connectedEdges) &&
                        connectedEdges.Count == 2)
                    {
                        var nextNode = connectedEdges.Single(n => n != node);

                        if (!covered.Contains(nextNode) &&
                            nextNode.Segment.Front == node.Segment.Front &&
                            nextNode.Segment.Back == node.Segment.Back &&
                            nextNode.Segment.Id.GetLineSlope() == node.Segment.Id.GetLineSlope())
                        {
                            node = nextNode;
                            covered.Add(node);
                            steps++;
                            continue;
                        }
                    }
                    break;
                }
                return (node, steps);
            }

            var (leftNode, leftSteps) = FollowNode(edge, goRight: false);
            var (rightNode, rightSteps) = FollowNode(edge, goRight: true);

            var span = new SectorEdge(
                Start: leftNode.Start,
                End: rightNode.End,
                Segment: leftNode.Segment,
                NumSquares: 1 + leftSteps + rightSteps);

            allSimplifiedEdges.Add(span);

            AddToGraph(simplifiedPointToEdges, span);
        }

        return new(allSimplifiedEdges, simplifiedPointToEdges);
    }

    private static void AddToGraph(Graph pointToEdges, SectorEdge edge)
    {
        void AddPoint(LatticePoint point)
        {
            if (pointToEdges.TryGetValue(point, out var edges))
            {
                edges.Add(edge);
            }
            else
            {
                pointToEdges.Add(point, new HashSet<SectorEdge> { edge });
            }
        }
        AddPoint(edge.Start);
        AddPoint(edge.End);
    }
}
