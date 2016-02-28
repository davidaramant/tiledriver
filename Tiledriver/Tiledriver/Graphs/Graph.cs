using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tiledriver.Graphs
{
    public sealed class Graph<TNode,TCost> : IEnumerable<TNode>
    {
        private readonly List<TNode> _nodes = new List<TNode>();
        private readonly Dictionary<GraphEdge<TNode>, TCost> _weightedEdges = new Dictionary<GraphEdge<TNode>, TCost>();

        public int NodeCount => _nodes.Count;

        public void AddNode(TNode node)
        {
            _nodes.Add(node);
        }

        public void RemoveLastNode()
        {
            var lastNode = _nodes[_nodes.Count - 1];
            _nodes.Remove(lastNode);
            var edgesInvolvedWithLast = _weightedEdges.Keys.Where(edge => edge.Involves(lastNode)).ToArray();
            foreach (var edge in edgesInvolvedWithLast)
            {
                _weightedEdges.Remove(edge);
            }
        }

        public void AddWeightedEdge(TNode node1, TNode node2, TCost cost)
        {
            var edge = new GraphEdge<TNode>(node1, node2);
            AddWeightedEdge(edge,cost);
        }

        public void AddWeightedEdge(GraphEdge<TNode> edge, TCost cost)
        {
            if (_weightedEdges.ContainsKey(edge))
            {
                throw new ArgumentException("Edge already exists");
            }
            _weightedEdges.Add(edge, cost);
        }


        public IEnumerator<TNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
