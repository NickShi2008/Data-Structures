using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary
{
    public class Graph<T>
    {
        //private Grid<T> GridVis { get; set; }
        public List<Vertex<T>> Vertices { get; set; }
        public List<Edge<T>> Edges { get; set; }

        public int VertexCount => Vertices.Count;



        public Graph()
        {

            Vertices = new List<Vertex<T>>();

            Edges = new List<Edge<T>>();
        }


        public (List<Vertex<T>> path, List<Vertex<T>> visited, float cost) Dijkstra(Vertex<T> start, Vertex<T> end)
        {
            //Init
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();
            Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> map = [];
            //set each vertex as Unknown
            foreach (var v in Vertices)
            {
                totalDistances.Add(v, float.PositiveInfinity);
            }

            //prepare start vertex
            totalDistances[start] = 0;

            queuedDistances.Enqueue(start, 0);


            //looks till visits all vertex
            while (queuedDistances.Count > 0)
            {
                Vertex<T> currentVertex = queuedDistances.Dequeue();

                if (visitedVertices.Contains(currentVertex))
                    continue;
                visitedVertices.Add(currentVertex);

                foreach (var edge in currentVertex.Neighbors)
                {
                    if (totalDistances[currentVertex] + edge.Distance
                        < totalDistances[edge.EndingPoint])
                    {
                        totalDistances[edge.EndingPoint] = totalDistances[currentVertex] + edge.Distance;
                        map[edge.EndingPoint] = (edge.StartingPoint, edge.Distance);

                        queuedDistances.Enqueue(edge.EndingPoint, totalDistances[edge.EndingPoint]);

                    }


                }
            }
            //traces backwards to the beginning

            var pathResult = FindPath(start, end, map);
            return (pathResult.path, visitedVertices, pathResult.cost);
        }

        public (List<Vertex<T>> path, List<Vertex<T>> visited, float cost) ASTAR(Vertex<T> start, Vertex<T> end,
            Func<Vertex<T>, Vertex<T>, float> heuristic)
        {
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();
            Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> map = [];

            foreach (var v in Vertices)
            {
                totalDistances.Add(v, float.PositiveInfinity);
            }

            totalDistances[start] = 0;

            //queuedDistances.Enqueue(start, 0);
            queuedDistances.Enqueue(start, heuristic(start, end));

            while (queuedDistances.Count > 0)
            {
                Vertex<T> vertex = queuedDistances.Dequeue();

                if (visitedVertices.Contains(vertex))
                    continue;
                visitedVertices.Add(vertex);

                foreach (var neigh in vertex.Neighbors)
                {
                    /*float finalDistance = totalDistances[vertex] + neigh.Distance + heuristic(neigh.EndingPoint, end);

                    if (finalDistance < totalDistances[neigh.EndingPoint])
                    {
                        totalDistances[neigh.EndingPoint] = finalDistance;
                        map[neigh.EndingPoint] = (neigh.StartingPoint, neigh.Distance);
                        queuedDistances.Enqueue(neigh.EndingPoint, finalDistance);
                    }*/
                    float finalDistance = totalDistances[vertex] + neigh.Distance;

                    if (finalDistance < totalDistances[neigh.EndingPoint])
                    {
                        totalDistances[neigh.EndingPoint] = finalDistance;
                        map[neigh.EndingPoint] = (neigh.StartingPoint, neigh.Distance);
                        float fScore = finalDistance + heuristic(neigh.EndingPoint, end);
                        queuedDistances.Enqueue(neigh.EndingPoint, fScore);
                    }
                        /*float tentativeGScore = gScore[vertex] + neigh.Distance;

                        if (tentativeGScore < gScore[neigh.EndingPoint])
                        {
                            gScore[neigh.EndingPoint] = tentativeGScore;
                            map[neigh.EndingPoint] = (neigh.StartingPoint, neigh.Distance);
                            float fScore = tentativeGScore + heuristic(neigh.EndingPoint, end);
                            queuedDistances.Enqueue(neigh.EndingPoint, fScore);
                        }*/
                }

            }

            var pathResult = FindPath(start, end, map);
            return (pathResult.path, visitedVertices, pathResult.cost);
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


        public void AddVertex(Vertex<T> vertex)
        {
            if (!SearchVertex(vertex) && vertex.NeighborCount == 0)
            {
                Vertices.Add(vertex);
            }
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (Vertices.Contains(vertex))
            {
                foreach (Edge<T> edges in vertex.Neighbors)
                {
                    edges.EndingPoint.Neighbors.Remove(edges.EndingPoint.FindFirstEdge(vertex));
                    vertex.Neighbors.Remove(vertex.FindFirstEdge(edges.EndingPoint));
                }
                Vertices.Remove(vertex);
                return true;
            }
            return false;
        }

        private bool SearchVertex(Vertex<T> vertex)
        {
            bool check = Vertices.Contains(vertex);
            return vertex != null && Vertices.Contains(vertex);
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if (SearchVertex(a))
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
            if (SearchVertex(a) && SearchVertex(b) && a.HasEdge(b) && b.HasEdge(a))
            {
                a.Neighbors.Remove(a.FindFirstEdge(b));
                b.Neighbors.Remove(b.FindFirstEdge(a));
                return true;
            }
            return false;
        }

        public Vertex<T> Search(T vertex)
        {
            int count = -1;
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Value.Equals(vertex))
                {
                    count = i;
                    break;
                }
            }

            if (count == -1)
            {
                return null;
            }
            return Vertices[count];
        }

        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a != null && b != null && a.HasEdge(b) && b.HasEdge(a))
            {
                return a.FindFirstEdge(b);
            }
            return null;
        }


    }
}