using System.Collections.Generic;

namespace Tiledriver.Graphs
{
    public class Edge<T>
    {
        public readonly T Node1;
        public readonly T Node2;

        public Edge(T node1, T node2)
        {
            Node1 = node1;
            Node2 = node2;
        }

        public bool Involves(T node)
        {
            return Equals(Node1, node) || Equals(Node2, node);
        }

        #region Equality stuff
        private bool Equals(Edge<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Node1, other.Node1) && EqualityComparer<T>.Default.Equals(Node2, other.Node2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Edge<T> && Equals((Edge<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Node1) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Node2);
            }
        }

        public static bool operator ==(Edge<T> left, Edge<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Edge<T> left, Edge<T> right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
