// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

using Graph = Dictionary<LatticePoint, HashSet<EdgeSpan>>;

public sealed class EdgeGraph
{
    private readonly List<EdgeSpan> _edges;
    private readonly Graph _pointToEdges;

    public int EdgeCount => _edges.Count;

    private EdgeGraph(
        List<EdgeSpan> edges,
        Graph pointToEdges)
    {
        _edges = edges;
        _pointToEdges = pointToEdges;
    }

    public IEnumerable<EdgeSpan> GetAllEdges() => _edges;

    public static EdgeGraph FromEdges(IEnumerable<EdgeSpan> edges)
    {
        var allEdges = edges.ToList();

        var pointToEdges = new Graph();

        foreach (var edge in allEdges)
        {
            AddToGraph(pointToEdges, edge);
        }

        return new(allEdges, pointToEdges);
    }

    public EdgeGraph Simplify()
    {
        var allSimplifiedEdges = new List<EdgeSpan>();
        var simplifiedPointToEdges = new Graph();

        var covered = new HashSet<EdgeSpan>();
        foreach (var edge in _edges)
        {
            if (covered.Contains(edge))
                continue;

            covered.Add(edge);

            (EdgeSpan End, int StepsTaken) FollowNode(EdgeSpan initial, bool goRight)
            {
                var slope = initial.Segment.Id.GetLineSlope();
                var node = initial;
                int steps = 0;

                while (true)
                {
                    if (_pointToEdges.TryGetValue(node.GetPointAtEnd(leftSide: goRight), out var connectedEdges) &&
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

            var span = new EdgeSpan(
                Start: leftNode.Start,
                End: rightNode.End,
                Segment: leftNode.Segment,
                NumSegments: 1 + leftSteps + rightSteps);

            allSimplifiedEdges.Add(span);

            AddToGraph(simplifiedPointToEdges, edge);
        }

        return new(allSimplifiedEdges, simplifiedPointToEdges);
    }

    private static void AddToGraph(Graph pointToEdges, EdgeSpan edge)
    {
        void AddPoint(LatticePoint point)
        {
            if (pointToEdges.TryGetValue(point, out var edges))
            {
                edges.Add(edge);
            }
            else
            {
                pointToEdges.Add(point, new HashSet<EdgeSpan> { edge });
            }
        }
        AddPoint(edge.Start);
        AddPoint(edge.End);
    }
}
