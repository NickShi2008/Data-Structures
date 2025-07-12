using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content.Tiled;
using Pathfinding;
using System;
using System.Collections.Generic;

namespace BootlegSimCity
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MouseState mouseState;
        private MouseState ms;
        private Grid<Point> grid;
        private MouseState lastMouseState = new MouseState();
        private Graph<Point> graph;
        private bool isRunning = false;
        private float timer = 0;
        private float timerDelay = 0.5f;
        private List<(ISquare, Vertex<Point>)> animateSquares;
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
            double screenWidth = graphics.PreferredBackBufferWidth;
            double screenHeight = graphics.PreferredBackBufferHeight;


            animateSquares = new List<(ISquare, Vertex<Point>)>();
            graph = new Graph<Point>();
            grid = new Grid<Point>(20, graphics);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ConnectGrid();

            (List<Vertex<Point>>, float) tracker = graph.ASTAR(FindHouse(), FindHouse(), Manhattan);

            foreach (Vertex<Point> vertex in tracker.Item1)
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


            AnimatePath(gameTime);

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
                    HouseSquare block = new HouseSquare(0, 0);
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

        Vertex<Point> FindHouse(Vertex<Point> vertex)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    HouseSquare start = new HouseSquare(i, j);
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

        Vertex<Point> ChangeToRoad(Vertex<Point> vertex)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    for (int k = 0; k < animateSquares.Count; k++)
                    {
                        if (grid.Squares[i, j].Equals(animateSquares[k].Item1)
                            && animateSquares[k].Item2.Equals(vertex))
                        {
                            Point store = grid.Squares[i, j].Location;
                            grid.Squares[i, j] = new RoadSquare(store.X, store.Y);
                        }
                    }
                }
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

        void AnimatePath(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            if (timer > timerDelay && animateSquares.Count - 1 > count)
            {
                ChangeToRoad(animateSquares[count].Item2);
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

