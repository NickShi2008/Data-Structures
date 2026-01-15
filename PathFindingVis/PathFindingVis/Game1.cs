using GraphLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
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
        private bool isRunning = false;
        private float timer = 0;
        private float timerDelay = 0.5f;
        private List<(ISquare, Vertex<Point>)> animateSquares;
        private List<Vertex<Point>> finalPath;
        private int count = 0;
        private bool hasReset = true;

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


            animateSquares = new List<(ISquare, Vertex<Point>)>();
            finalPath = new List<Vertex<Point>>();
            graph = new Graph<Point>();
            grid = new Grid<Point>(20, graphics);
            //ConnectGrid();
        }

        KeyboardState lastKeyState = Keyboard.GetState();
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            if ((keyboardState.IsKeyDown(Keys.Enter) && hasReset) || isRunning)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    ConnectGrid();
                    //now receiving both visited vertices and final path
                    var tracker = graph.ASTAR(FindStart(), FindEnd(), Euclidean);
                    // var tracker = graph.Dijkstra(FindStart(), FindEnd());

                    // Store the final path separately
                    finalPath = tracker.path;

                    // Add all visited vertices to animate list
                    foreach (Vertex<Point> vertex in tracker.visited)
                    {
                        for (int i = 0; i < graph.VertexCount; i++)
                        {
                            if (vertex.Equals(graph.Vertices[i]))
                            {
                                int x = i / 20;
                                int y = i % 20;

                                animateSquares.Add((grid.Squares[x, y], graph.Vertices[i]));
                            }
                        }
                    }
                }

                AnimatePath(gameTime);

            }
            else if (keyboardState.IsKeyUp(Keys.R)  && lastKeyState.IsKeyDown(Keys.R) && hasReset)
            {
                RandomSquares();
            }
            else if (keyboardState.IsKeyDown(Keys.Back))
            {
                animateSquares = new List<(ISquare, Vertex<Point>)>();
                finalPath = new List<Vertex<Point>>();
                graph = new Graph<Point>();
                grid = new Grid<Point>(20, graphics);
                hasReset = true;
                count = 0;
            }
            else if (!isRunning && hasReset)
            {
                ms = Mouse.GetState();

                if (ms.LeftButton == ButtonState.Pressed)
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
                
                lastMouseState = ms;
            }

            lastKeyState = keyboardState;

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
            if (graph.VertexCount > 0)
            {
                graph.Vertices.Clear();
            }
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

        void ChangeToNeigh(Vertex<Point> vertex)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    // Don't change walls, start, or end squares
                    BlockSquare block = new BlockSquare(0, 0);
                    StartSquare start = new StartSquare(0, 0);
                    EndSquare end = new EndSquare(0, 0);

                    if (grid.Squares[i, j].GetType() == block.GetType() ||
                        grid.Squares[i, j].GetType() == start.GetType() ||
                        grid.Squares[i, j].GetType() == end.GetType())
                    {
                        continue;
                    }

                    for (int k = 0; k < animateSquares.Count; k++)
                    {
                        if (grid.Squares[i, j].Equals(animateSquares[k].Item1)
                            && animateSquares[k].Item2.Equals(vertex))
                        {
                            Point store = grid.Squares[i, j].location;
                            grid.Squares[i, j] = new NeighbourSquares(store.X, store.Y);
                        }
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
                    // Don't change walls, start, or end squares
                    BlockSquare block = new BlockSquare(0, 0);
                    StartSquare start = new StartSquare(0, 0);
                    EndSquare end = new EndSquare(0, 0);

                    if (grid.Squares[i, j].GetType() == block.GetType() ||
                        grid.Squares[i, j].GetType() == start.GetType() ||
                        grid.Squares[i, j].GetType() == end.GetType())
                    {
                        continue;
                    }

                    for (int k = 0; k < animateSquares.Count; k++)
                    {
                        if (grid.Squares[i, j].Equals(animateSquares[k].Item1)
                            && animateSquares[k].Item2.Equals(vertex))
                        {
                            Point store = grid.Squares[i, j].location;
                            grid.Squares[i, j] = new SearchdSquares(store.X, store.Y);
                        }
                    }
                }
            }
        }

        void ChangeToPath(Vertex<Point> vertex)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    // Don't change walls, start, or end squares
                    BlockSquare block = new BlockSquare(0, 0);
                    StartSquare start = new StartSquare(0, 0);
                    EndSquare end = new EndSquare(0, 0);

                    if (grid.Squares[i, j].GetType() == block.GetType() ||
                        grid.Squares[i, j].GetType() == start.GetType() ||
                        grid.Squares[i, j].GetType() == end.GetType())
                    {
                        continue;
                    }

                    if (vertex.Value.X == i && vertex.Value.Y == j)
                    {
                        Point store = grid.Squares[i, j].location;
                        grid.Squares[i, j] = new PathSquare(store.X, store.Y);
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
            float dy = MathF.Abs(start.Value.Y - end.Value.Y);
            //distance from one square to another
            float D = 1;
            float DTwo = MathF.Sqrt(2);
            return D * MathF.Sqrt(dx * dx + dy * dy);

        }

        public void RandomSquares()
        {
            Random random = new Random();

            // Generate random walls (about 25% of the grid)
            int numWalls = (grid.Squares.GetLength(0) * grid.Squares.GetLength(1)) / 4;

            for (int i = 0; i < numWalls; i++)
            {
                int randomX = random.Next(0, grid.Squares.GetLength(0));
                int randomY = random.Next(0, grid.Squares.GetLength(1));

                // Don't place walls on start or end squares
                StartSquare start = new StartSquare(0, 0);
                EndSquare end = new EndSquare(0, 0);

                if (grid.Squares[randomX, randomY].GetType() != start.GetType() &&
                    grid.Squares[randomX, randomY].GetType() != end.GetType())
                {
                    Point location = grid.Squares[randomX, randomY].location;
                    grid.Squares[randomX, randomY] = new BlockSquare(location.X, location.Y);
                }
            }
        }


        void AnimatePath(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 30;
            if (timer > timerDelay && animateSquares.Count - 1 > count)
            {
                // Check if current vertex is in the final path
                bool isInPath = finalPath.Contains(animateSquares[count].Item2);

                if (isInPath)
                {
                    // Color as path square (final solution path)
                    ChangeToPath(animateSquares[count].Item2);
                }
                else
                {
                    // Check if any previously visited squares are neighbors of current
                    for (int i = 0; i < count; i++)
                    {
                        foreach (var check in animateSquares[i].Item2.Neighbors)
                        {
                            if (check.EndingPoint.Equals(animateSquares[count].Item2))
                            {
                                ChangeToNeigh(animateSquares[count].Item2);
                            }
                        }
                    }

                    // Color as searched square (visited but not in final path)
                    ChangeToSearched(animateSquares[count].Item2);
                }

                count++;
                timer = 0;
            }
            else if (animateSquares.Count - 1 <= count)
            {
                hasReset = false;
                isRunning = false;
            }
        }
    }
}