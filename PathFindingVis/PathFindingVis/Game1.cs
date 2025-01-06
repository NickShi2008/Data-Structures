using GraphLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;


namespace PathFindingVis
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MouseState ms;
        private Grid<Point> grid;
        private MouseState lastMouseState = new MouseState();
        private Graph<Point> graph;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            /////awjwa0ifasdfajf
            //font = Content.Load<SpriteFont>("Ubuntu32.spritefont");
            double screenWidth = graphics.PreferredBackBufferWidth;
            double screenHeight = graphics.PreferredBackBufferHeight;

            graph = new Graph<Point>();
            grid = new Grid<Point>(20, graphics);
            ConnectGrid();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                ConnectGrid();
                graph.ASTAR(FindStart(), FindEnd(), Euclidean);
            }

            ms = Mouse.GetState();

            if(ms.LeftButton == ButtonState.Pressed)
            {
                int x = ms.X;
                int y = ms.Y;
            }


            
            if (ms.LeftButton == ButtonState.Pressed && (lastMouseState.LeftButton == ButtonState.Released || grid.EnableDrag)
                && ms.X > 0 && ms.X < graphics.PreferredBackBufferWidth && ms.Y > 0 && ms.Y < graphics.PreferredBackBufferHeight)
            {
                if (!grid.hasStored)
                {
                    grid.StoreMouseClick(ms.X, ms.Y);
                }
                else
                {
                    grid.PlaceSquare(ms.X, ms.Y);
                }
                

            }
            // TODO: Add your update logic here
            lastMouseState = ms;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            grid.DrawGrid(spriteBatch);

            
            // spriteBatch.FillRectangle(buttonRectangle, Color.MediumSlateBlue);
            

            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
            
        }

        public void ConnectGrid()
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    var vertex = new Vertex<Point>(new Point(i, j));
                    graph.AddVertex(vertex);
                }
            }

            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    BlockSquare block = new BlockSquare(0, 0);
                    if (block.GetType() == grid.Squares[i, j].GetType())
                    {
                        continue;
                    }
                    var currentVertex = graph.Search(new Point(i, j));

                    List<Point> Neighbors = new List<Point>();

                    Neighbors.Add(new Point(i - 1, j));
                    Neighbors.Add(new Point(i, j - 1));
                    Neighbors.Add(new Point(i + 1, j));
                    Neighbors.Add(new Point(i, j + 1));


                    for (int k = Neighbors.Count - 1; k >= 0; k--)
                    {
                        if (Neighbors[k].X >= grid.Squares.GetLength(0) || Neighbors[k].X < 0
                            || Neighbors[k].Y >= grid.Squares.GetLength(1) || Neighbors[k].Y < 0)
                        {
                            Neighbors.Remove(Neighbors[k]);
                        }
                    }



                    foreach (var neigh in Neighbors)
                    {

                        if (grid.Squares[neigh.X, neigh.Y].GetType != block.GetType)
                        {
                            var neighborVertex = graph.Search(neigh);
                            graph.AddEdge(currentVertex, neighborVertex, 1); // Distance between box is 1
                        }
                    }
                }
            }
        }

        void ChangeToNeigh(Edge<Point> edge)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    if (grid.Squares[i, j].Equals(edge.EndingPoint))
                    {
                        Point store = grid.Squares[i, j].location;
                        grid.Squares[i, j] = new NeighbourSquares(store.X, store.Y);
                    }
                }
            }
        }

        void ChangeToSearched(Vertex<Point> vertex)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    if (grid.Squares[i, j].Equals(vertex))
                    {
                        Point store = grid.Squares[i, j].location;
                        grid.Squares[i, j] = new SearchdSquares(store.X, store.Y);
                    }
                }
            }
        }

        Vertex<Point> FindStart()
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    StartSquare start = new StartSquare(i, j);
                    if (grid.Squares[i, j].GetType() == start.GetType())
                    {
                        foreach (Vertex<Point> vertex in graph.Vertices)
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

        Vertex<Point> FindEnd()
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    EndSquare start = new EndSquare(i, j);
                    if (grid.Squares[i, j].GetType() == start.GetType())
                    {
                        foreach (Vertex<Point> vertex in graph.Vertices)
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
