using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Graphs
{
    public sealed  class WeightedEdge<TNode,TCost> : Edge<TNode>
    {
        public TCost Cost { get; }

        public WeightedEdge(TNode node1, TNode node2, TCost cost) : base(node1, node2)
        {
            Cost = cost;
        }
    }
}
