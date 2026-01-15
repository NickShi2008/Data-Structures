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
            Vertices = new Dictionary<T, Vertex<T>>();
            Edges = new HashSet<Edge<T>>();
        }

        public (List<Vertex<T>>, float cost) ASTAR(Vertex<T> start, Vertex<T> end,
            Func<Vertex<T>, Vertex<T>, float> heuristic)
        {
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();
            Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> map = new Dictionary<Vertex<T>, (Vertex<T>, float)>();

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

                if (vertex.Equals(end))
                    break;

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
                    if (edges.EndingPoint.HasEdge(vertex))
                    {
                        edges.EndingPoint.Neighbors.Remove(edges.EndingPoint.FindFirstEdge(vertex));
                    }
                }
                vertex.Neighbors.Clear();
                Vertices.Remove(value);
                return true;
            }
            return false;
        }

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
                return false;
            }

            if (a == null || b == null)
            {
                return false;
            }

            if (GetEdge(a, b) == null)
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
                if (a.HasEdge(b))
                    a.Neighbors.Remove(a.FindFirstEdge(b));
                if (b.HasEdge(a))
                    b.Neighbors.Remove(b.FindFirstEdge(a));
                return true;
            }
            return false;
        }

        public void RemoveVertexAndEdges(T aVal)
        {
            if (!Vertices.ContainsKey(aVal))
                return;

            Vertex<T> a = Vertices[aVal];

            // Remove all edges pointing to this vertex
            foreach (var vertex in Vertices.Values)
            {
                if (vertex.HasEdge(a))
                {
                    vertex.Neighbors.Remove(vertex.FindFirstEdge(a));
                }
            }

            // Remove all edges from this vertex and the vertex itself
            RemoveVertex(a.Value);
        }

        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a != null && b != null && Vertices.ContainsKey(a.Value) && Vertices[a.Value].HasEdge(b))
            {
                return a.FindFirstEdge(b);
            }
            return null;
        }
    }
}