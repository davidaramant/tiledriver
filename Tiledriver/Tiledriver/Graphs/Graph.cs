using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tiledriver.Graphs
{
    public sealed class Graph<TNode,TEdge> : IEnumerable<TNode> where TEdge : Edge<TNode>
    {
        private readonly List<TNode> _nodes = new List<TNode>();
        private readonly HashSet<TEdge> _edges = new HashSet<TEdge>();

        public int NodeCount => _nodes.Count;

        public void AddNode(TNode node)
        {
            _nodes.Add(node);
        }

        public void RemoveLastNode()
        {
            var lastNode = _nodes[_nodes.Count - 1];
            Remove(lastNode);
        }

        public void Remove(TNode node)
        {
            _nodes.Remove(node);
            var edgesInvolvedWithLast = _edges.Where(edge => edge.Involves(node)).ToArray();
            foreach (var edge in edgesInvolvedWithLast)
            {
                _edges.Remove(edge);
            }
        }

        public void AddEdge(TEdge edge)
        {
            if (_edges.Contains(edge))
            {
                throw new ArgumentException("Edge already exists");
            }
            _edges.Add(edge);
        }

        public IEnumerable<TEdge> AllEdges => _edges;

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
