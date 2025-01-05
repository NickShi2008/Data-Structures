using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PathFindingVis
{
    public interface ISquare
    {

        public Point location { get; set; }
        abstract void Draw(SpriteBatch sb, Point size);

    }

    public class EmptySquare : ISquare
    {
        public Point location { get; set; }

        public EmptySquare(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.AntiqueWhite);
        }
    }

    public class BlockSquare : ISquare
    {
        public Point location { get; set; }

        public BlockSquare(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.LightSlateGray);
        }
    }
    public class StartSquare : ISquare
    {
        public Point location { get; set; }

        public StartSquare(int x, int y)
        {
            location = new Point(x, y);
        }

        public StartSquare(StartSquare start)
        {
            location = start.location;
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MediumSpringGreen);
        }
    }

    public class SearchdSquares : ISquare
    {
        public Point location { get; set; }

        public SearchdSquares(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.LightGoldenrodYellow);
        }
    }

    public class NeighbourSquares : ISquare
    {
        public Point location { get; set; }

        public NeighbourSquares(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb,  Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MonoGameOrange);
        }
    }

    public class EndSquare : ISquare
    {
        public Point location { get; set; }

        public EndSquare(int x, int y)
        {
            location = new Point(x, y);
        }

        public EndSquare(EndSquare end)
        {
            location = end.location;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MediumVioletRed);
        }
    }

    public class Grid <T>
    {
        public int NumOfLines { get; set; }
        public Graph<Point> GraphPoint { get; set; }    

        public ISquare[,] Squares { get; set; }

        public ISquare squareClicked;
        public Point storedIndex;
        public bool hasStored = false;

        private int size;

        public bool EnableDrag = false;

        public Grid(int lines, GraphicsDeviceManager graphics)
        {
            GraphPoint = new Graph<Point>(lines, graphics);
            NumOfLines = lines;
            size = graphics.PreferredBackBufferWidth / lines;
            Squares = new ISquare[lines, lines];
            InitGrid();
        }

        private void InitGrid()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    Squares[i, j] = new EmptySquare(i * size, j * size);
                }
            }

            Squares[0, Squares.GetLength(1) - 1] = new StartSquare(0, (Squares.GetLength(1) - 1) * size);
            Squares[Squares.GetLength(0) - 1, 0] = new EndSquare((Squares.GetLength(0) - 1) * size ,0);
        }

        public void ConnectGrid()
        {
            for(int i = 0; i < Squares.GetLength(0); i++)
            {
                for(int j = 0; j < Squares.GetLength(1); j++)
                {
                    var vertex = new Vertex<Point>(new Point(i, j));
                    GraphPoint.AddVertex(vertex);
                }
            }

            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    BlockSquare block = new BlockSquare(0, 0);
                    if(block.GetType() == Squares[i,j].GetType())
                    {
                        continue;
                    }
                    var currentVertex = GraphPoint.Search(new Point(i, j));

                    List<Point> Neighbors = new List<Point>();
                    
                    Neighbors.Add(new Point(i - 1, j));
                    Neighbors.Add(new Point(i, j - 1));
                    Neighbors.Add(new Point(i + 1, j));
                    Neighbors.Add(new Point(i, j + 1));
                    
                    
                    for(int k = Neighbors.Count - 1; k >= 0; k--)
                    { 
                        if (Neighbors[k].X >= Squares.GetLength(0) || Neighbors[k].X < 0 
                            || Neighbors[k].Y >= Squares.GetLength(1) || Neighbors[k].Y < 0)
                        {
                            Neighbors.Remove(Neighbors[k]);
                        }
                    }

                    

                    foreach (var neigh in Neighbors)
                    {
                        
                        if (Squares[neigh.X, neigh.Y].GetType != block.GetType)
                        {
                            var neighborVertex = GraphPoint.Search(neigh);
                            GraphPoint.AddEdge(currentVertex, neighborVertex, 1); // Distance between box is 1
                        }
                    }
                }
            }


        }

        public void ChangeToNeigh(Edge<T> edge)
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    if (Squares[i,j].Equals(edge.EndingPoint))
                    {
                        Point store = Squares[i, j].location;
                        Squares[i, j] = new NeighbourSquares(store.X, store.Y);
                    }
                }
            }
        }

        public void ChangeToSearched(Vertex<T> vertex)
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    if (Squares[i, j].Equals(vertex))
                    {
                        Point store = Squares[i, j].location;
                        Squares[i, j] = new SearchdSquares(store.X, store.Y);
                    }
                }
            }
        }

        public Vertex<Point> FindStart()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    StartSquare start = new StartSquare(i,j);
                    if (Squares[i, j].GetType() == start.GetType())
                    {
                        foreach(Vertex<Point> vertex in GraphPoint.Vertices)
                        {
                            if (vertex.Value.X == i && vertex.Value.Y == j)
                            {
                                return vertex;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public Vertex<Point> FindEnd()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    EndSquare start = new EndSquare(i, j);
                    if (Squares[i, j].GetType() == start.GetType())
                    {
                        foreach (Vertex<Point> vertex in GraphPoint.Vertices)
                        {
                            if (vertex.Value.X == i && vertex.Value.Y == j)
                            {
                                return vertex;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            foreach (var square in Squares)
            {
                square.Draw(spriteBatch, new Point(size - 1)); 
            }
        }

        public void StoreMouseClick(int x, int y)
        {
            //fix enable drag so it doesn't repeat on one thing
            storedIndex = new Point(x/size, y/size);

            
            

            EmptySquare empty = new EmptySquare(0,0);
            BlockSquare block = new BlockSquare(0, 0);
            EnableDrag = true;

            if (squareClicked != null && (squareClicked.GetType() == empty.GetType() || squareClicked.GetType() == block.GetType()) && squareClicked == Squares[storedIndex.X, storedIndex.Y])
            {
                return;
            }
            squareClicked = Squares[storedIndex.X, storedIndex.Y];

            if (squareClicked.GetType() == empty.GetType())
            {
             
                squareClicked = new BlockSquare(Squares[storedIndex.X, storedIndex.Y].location.X, Squares[storedIndex.X, storedIndex.Y].location.Y);
                PlaceSquare(x, y);
                
            }
            else if(squareClicked.GetType() == block.GetType())
            {
         
                squareClicked = new EmptySquare(Squares[storedIndex.X, storedIndex.Y].location.X, Squares[storedIndex.X, storedIndex.Y].location.Y);
                PlaceSquare(x, y);
            }
            else
            {
                EnableDrag = false;
                hasStored = true;
            }
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

        public void PlaceSquare(int x, int y)
        {
            Point point = new Point(x / size, y / size);
            //new clicked location
            Point store = Squares[point.X, point.Y].location;
            ISquare square = Squares[point.X, point.Y];
            
            //testing to see if I can get rid of black square
            
            

            if (squareClicked.location != Squares[point.X, point.Y].location)
            {
                Squares[storedIndex.X, storedIndex.Y] = square;
                Squares[storedIndex.X, storedIndex.Y].location = squareClicked.location;
            }
            Squares[point.X, point.Y] = squareClicked;
            Squares[point.X, point.Y].location = store;

            hasStored = false;
        }

    }
}
