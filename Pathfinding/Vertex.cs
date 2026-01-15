using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    public class Vertex<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }
        public int NeighborCount => Neighbors.Count;

        public static EqualityComparer<Vertex<T>> ValueComparer = EqualityComparer<Vertex<T>>.Create(
            (x, y) => {
                return x.Value.Equals(y.Value);
            });

        public Vertex(T value)
        {
            Value = value;
            Neighbors = new List<Edge<T>>();
        }

        public bool HasEdge(Vertex<T> a)
        {
            return Neighbors.Where(x => x.EndingPoint.Equals(a)).Count() > 0;
        }

        public Edge<T> FindFirstEdge(Vertex<T> a)
        {
            if (a.Equals(this))
            {
                return new Edge<T>(a, a, 0);
            }
            return Neighbors.Where(x => x.EndingPoint.Equals(a)).First();
        }
    }
}