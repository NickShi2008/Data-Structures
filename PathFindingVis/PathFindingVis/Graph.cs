using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingVis
{
    public class Graph<T> 
    {
        private Grid<T> GridVis { get; set; }
        public List<Vertex<T>> Vertices { get; set; }
        public List<Edge<T>> Edges { get; set; }

        public int VertexCount => Vertices.Count;


        
        public Graph()
        {

            Vertices = new List<Vertex<T>>();
            
            Edges = new List<Edge<T>>();
        }

        
        public List<Vertex<T>> Dijkstra(Vertex<T> start, Vertex<T> end)
        {
            //Init
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();

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

                        
                        queuedDistances.Enqueue(edge.EndingPoint, totalDistances[edge.EndingPoint]);

                    }


                }
            }
            //traces backwards to the beginning

            return FindPath(start, end, totalDistances);
        }

        public List<Vertex<T>> ASTAR(Vertex<T> start, Vertex<T> end, Func<float> heuristic)
        {
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();

            foreach (var v in Vertices)
            {
                totalDistances.Add(v, float.PositiveInfinity);
            }

            totalDistances[start] = 0;

            queuedDistances.Enqueue(start, 0);

            while (queuedDistances.Count > 0)
            {
                Vertex<T> vertex = queuedDistances.Dequeue();

                if (visitedVertices.Contains(vertex))
                    continue;

                visitedVertices.Add(vertex);
                GridVis.ChangeToSearched(vertex);

                foreach (var neigh in vertex.Neighbors)
                {
                    GridVis.ChangeToNeigh(neigh);
                    if (totalDistances[vertex] + neigh.Distance < totalDistances[neigh.EndingPoint])
                    {
                        totalDistances[neigh.EndingPoint] = totalDistances[vertex] + neigh.Distance;

                        RearrangeQueue(queuedDistances, totalDistances[vertex] + neigh.Distance, neigh.EndingPoint);
                    }
                }

            }

            return FindPath(start, end, totalDistances);
        }

        public void RearrangeQueue(PriorityQueue<Vertex<T>, float> queue, float originalDistance, Vertex<T> comparison)
        {
            PriorityQueue<Vertex<T>, float> placeHolder = queue;
            bool hasBeenFound = false;
            bool hasBeenReplaced = false;

            while (queue.Count > 0)
            {
                queue.TryDequeue(out var currentVertex, out float currentDistance);

                if (currentVertex.Equals(comparison))
                {
                    hasBeenFound = true;
                    if (originalDistance < currentDistance)
                    {
                        placeHolder.Enqueue(comparison, originalDistance);
                        hasBeenReplaced = true;
                    }
                    else
                    {
                        placeHolder.Enqueue(currentVertex, currentDistance);
                    }
                }
                else
                {
                    placeHolder.Enqueue(currentVertex, currentDistance);
                }
            }

            if (!hasBeenFound)
            {
                placeHolder.Enqueue(comparison, originalDistance);
            }

            while (placeHolder.Count > 0)
            {
                placeHolder.TryDequeue(out var tempNode, out var tempPriority);

                queue.Enqueue(tempNode, tempPriority);
            }
        }

        public List<Vertex<T>> FindPath(Vertex<T> start, Vertex<T> end, Dictionary<Vertex<T>, float> totalDistances)
        {
            Stack<Vertex<T>> reversePath = new Stack<Vertex<T>>();
            Vertex<T> lastVertex = end;
            while (!lastVertex.Equals(start))
            {
                foreach (var vertex in totalDistances)
                {

                    for (int i = 0; i < Edges.Count; i++)
                    {
                        if (Edges[i].EndingPoint.Equals(lastVertex) && Edges[i].StartingPoint.Value.Equals(vertex.Key.Value)
                            && vertex.Value + Edges[i].Distance == totalDistances[lastVertex])
                        {
                            lastVertex = vertex.Key;
                            reversePath.Push(Edges[i].EndingPoint);
                            break;
                        }
                    }

                }
            }

            List<Vertex<T>> path = new List<Vertex<T>>();

            while (reversePath.Count > 0)
            {
                path.Add(reversePath.Pop());
            }

            return path;
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
            if (SearchVertex(a) && SearchVertex(b))
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
