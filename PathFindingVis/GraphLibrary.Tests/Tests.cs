using System.Collections;
using System.Drawing;
namespace GraphLibrary.Tests
{
    public class GraphGenerator : IEnumerable<object[]>
    {

        public Graph<Point> GenerateGraph()
        {
            Graph<Point> bob = new Graph<Point>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vertex<Point> currentVertex = new Vertex<Point>(new Point(i, j));
                    bob.AddVertex(currentVertex);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var currentVertex = bob.Search(new Point(i,j));
                    List<Point> Neighbors = new List<Point>();

                    Neighbors.Add(new Point(i - 1, j));
                    Neighbors.Add(new Point(i, j - 1));
                    Neighbors.Add(new Point(i + 1, j));
                    Neighbors.Add(new Point(i, j + 1));


                    for (int k = Neighbors.Count - 1; k >= 0; k--)
                    {
                        double test = Math.Sqrt(bob.VertexCount);
                        if (Neighbors[k].X >= Math.Sqrt(bob.VertexCount) || Neighbors[k].X < 0
                            || Neighbors[k].Y >= Math.Sqrt(bob.VertexCount) || Neighbors[k].Y < 0)
                        {
                            Neighbors.Remove(Neighbors[k]);
                        }
                    }

                    foreach (var neigh in Neighbors)
                    {
                        bob.AddEdge(currentVertex, bob.Search(neigh), 1); // Distance between box is 1
                    }
                }
            }

            


            return bob;
        }

        public List<object[]> arr => [
            [GenerateGraph(), new Point(0, 0), new Point(2, 2), 4]
            
        ];

        public IEnumerator<object[]> GetEnumerator() => arr.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class Tests
    {

        /*  
          [Theory]
          [InlineData(new Point(1,1), new Point(2,2), new List<Point>(new Point(1,1), new Point(2,1), new Point(2,2))
          public void Add(Point one, Point two, List<Point> path)
          {
              GraphGenerator g = new(2);
              Graph<Point> graph = new Graph<Point>();
              Assert.True(graph.ASTAR());
          }
        */

        [Theory]
        [ClassData(typeof(GraphGenerator))]
        public void Test(Graph<Point> graph, Point start, Point end, int expectedCost)
        {
            var path = graph.ASTAR(graph.Search(start), graph.Search(end), Manhattan);


            Assert.True(path.Item1.Count == expectedCost);
        }


        public float Manhattan(Vertex<Point> start, Vertex<Point> end)
        {
            float dx = MathF.Abs(start.Value.X - end.Value.X);
            float dy = MathF.Abs(start.Value.Y - end.Value.Y);
            //distance from one square to another
            float D = 1;
            return D * (dx + dy);
        }


        public float Diagonal(Vertex<Point> start, Vertex<Point> end)
        {
            float dx = MathF.Abs(start.Value.X - end.Value.X);
            float dy = MathF.Abs(start.Value.Y - end.Value.Y);
            //distance from one square to another
            float D = 1;
            float DTwo = MathF.Sqrt(2);
            return D * (dx + dy) + (DTwo - 2 * D) * MathF.Min(dx, dy);

        }

        public float Euclidean(Vertex<Point> start, Vertex<Point> end)
        {
            float dx = MathF.Abs(start.Value.X - end.Value.X);
            float dy = MathF.Abs(start.Value.X - end.Value.Y);
            //distance from one square to another
            float D = 1;
            float DTwo = MathF.Sqrt(2);
            return D * MathF.Sqrt(dx * dx + dy * dy);

        }
    }


}