using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Pathfinding
{
    public class Graph<T>
    {
        public Dictionary<T, Vertex<T>> Vertices { get; set; }
        public HashSet<Edge<T>> Edges { get; set; }

        public int VertexCount => Vertices.Count;
        public Graph()
        {
            Vertices = new Dictionary<T,Vertex<T>>();

            Edges = new HashSet<Edge<T>>();
        }
        
        
        public (List<Vertex<T>>, float cost) ASTAR(Vertex<T> start, Vertex<T> end,
            Func<Vertex<T>, Vertex<T>, float> heuristic)
        {
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();
            Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> map = [];

            foreach (var v in Vertices)
            {
                totalDistances.Add(v.Value, float.PositiveInfinity);
            }

            totalDistances[start] = 0;

            queuedDistances.Enqueue(start, 0);

            while (queuedDistances.Count > 0)
            {
                Vertex<T> vertex = queuedDistances.Dequeue();

                if (visitedVertices.Contains(vertex))
                    continue;
                visitedVertices.Add(vertex);

                foreach (var neigh in vertex.Neighbors)
                {
                    float finalDistance = totalDistances[vertex] + neigh.Distance + heuristic(neigh.EndingPoint, end);

                    if (finalDistance < totalDistances[neigh.EndingPoint])
                    {
                        totalDistances[neigh.EndingPoint] = finalDistance;
                        map[neigh.EndingPoint] = (neigh.StartingPoint, neigh.Distance);
                        queuedDistances.Enqueue(neigh.EndingPoint, finalDistance);
                    }
                }

            }

            return FindPath(start, end, map);
        }


        public (List<Vertex<T>> path, float cost) FindPath(Vertex<T> start, Vertex<T> end, Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> founderMap)
        {
            Stack<Vertex<T>> reversePath = new Stack<Vertex<T>>();
            var curr = end;
            float cost = 0;
            while (founderMap.ContainsKey(curr))
            {
                reversePath.Push(curr);
                cost += founderMap[curr].cost;
                curr = founderMap[curr].founder;
            }

            return (reversePath.ToList(), cost);
        }

        public void AddVertex(T value)
        {
            if (!Vertices.ContainsKey(value))
            {
                Vertices.Add(value, new Vertex<T>(value));
            }
        }

        public bool RemoveVertex(T value)
        {
            
            if (Vertices.ContainsKey(value))
            {
                Vertex<T> vertex = Vertices[value];
                foreach (Edge<T> edges in vertex.Neighbors)
                {
                    edges.EndingPoint.Neighbors.Remove(edges.EndingPoint.FindFirstEdge(vertex));
                    vertex.Neighbors.Remove(vertex.FindFirstEdge(edges.EndingPoint));
                }
                Vertices.Remove(value);
                return true;
            }
            return false;
        }

       /*private bool SearchVertex(Vertex<T> vertex)
        {
            return vertex != null && Vertices.Contains(vertex, Vertex<T>.ValueComparer);
        }*/

        public bool AddEdge(T aValue, T bValue, float distance)
        {
            Vertex<T> a = null;
            Vertex<T> b = null;
            if (Vertices.ContainsKey(aValue) && Vertices.ContainsKey(bValue))
            {
                a = Vertices[aValue];
                b = Vertices[bValue];
            }
            else
            {
                throw new ArgumentException("One or both vertices do not exist in the graph.");
            }


            if (a == null || b == null)
            {
                throw new ArgumentNullException("Vertices cannot be null");
            }

            

            if (GetEdge(a,b) == null && GetEdge(b,a) == null)
            {
                Edge<T> AConnector = new Edge<T>(a, b, distance);
                Edges.Add(AConnector);
                if (!a.Neighbors.Contains(AConnector))
                   a.Neighbors.Add(AConnector);

                return true;
            }
            return false;
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a != null && b != null)
            {
                if(a.HasEdge(b))
                    a.Neighbors.Remove(a.FindFirstEdge(b));
                if(b.HasEdge(a))
                    b.Neighbors.Remove(b.FindFirstEdge(a));
                return true;
            }
            return false;
        }

        public void RemoveVertexAndEdges(T aVal)
        {
            Vertex<T> a = Vertices[aVal];
            foreach (var edge in Edges)
            {
                if(edge.StartingPoint.Equals(a))
                    RemoveEdge(a, edge.EndingPoint);
                else if(edge.EndingPoint.Equals(a))
                    RemoveEdge(edge.StartingPoint, a);
            }
            RemoveVertex(a.Value);
        }

      /*  public Vertex<T>? Search(T value)
        {
            if(Vertices.Count == 0 || value == null)
            {
                return null;
            }
      
            return Vertices.ContainsKey(value) ? Vertices[value] : null;
        }*/

        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            
            if (a != null && b != null 
                && Vertices[a.Value].HasEdge(b))
            {
                return a.FindFirstEdge(b);
            }
            return null;
        }
    }
}
