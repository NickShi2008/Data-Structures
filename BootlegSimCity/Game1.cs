using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathfinding;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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
        const double SCREENSIZE = 1000;
        private SelectScreen selectScreen;
        SpriteFont spriteFont;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = (int)SCREENSIZE;
            graphics.PreferredBackBufferWidth = (int)(SCREENSIZE*1.2);
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            double screenWidth = graphics.PreferredBackBufferWidth;
            double screenHeight = graphics.PreferredBackBufferHeight;
            selectScreen = new SelectScreen((int) SCREENSIZE);

            animateSquares = new List<(ISquare, Vertex<Point>)>();
            graph = new Graph<Point>();
            grid = new Grid<Point>(20, (int) SCREENSIZE);
            spriteFont = Content.Load<SpriteFont>("Ubuntu32");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ConnectGrid();
         
            Vertex<Point> houseOne = FindHouse();
            Vertex<Point> houseTwo = FindHouse(houseOne);
            (List<Vertex<Point>>, float) tracker;
            if (houseOne != null && houseTwo != null)
            {
                tracker = graph.ASTAR(houseOne, houseTwo, Manhattan);

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
            }
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                grid.StoreClick(ms.X, ms.Y);
            }
            else if (ms.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                grid.PlaceSquare(ms.X, ms.Y);
            }

            lastMouseState = ms;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            grid.DrawGrid(spriteBatch);

            selectScreen.DrawSelectScreen(spriteBatch, graphics);

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
                    if (grid.Squares[i, j] is EmptySquare || grid.Squares[i,j] is HouseSquare)
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
                        if (grid.Squares[neigh.X, neigh.Y] is RoadSquare || grid.Squares[neigh.X, neigh.Y] is RoadSquare)
                        {
                            var neighborVertex = graph.Search(neigh);
                            graph.AddEdge(currentVertex, neighborVertex, 1); // Distance between box is 1
                        }
                    }
                }
            }
        }

        Vertex<Point> FindHouse(Vertex<Point> stored = null)
        {
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    if (grid.Squares[i, j] is HouseSquare && grid.Squares[i,j] != stored)
                    {
                        foreach (Vertex<Point> ver in graph.Vertices)
                        {
                            if (ver.Value.X == i && ver.Value.Y == j)
                            {
                                return ver;
                            }
                        }
                    }
                }
            }
            return null;
        }

        void ChangeToCar(Vertex<Point> vertex)
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
                            grid.Squares[i, j] = new CarSquare(store.X, store.Y);
                        }
                    }
                }
            }
        }

        void ChangeToRoad(Vertex<Point> vertex)
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

        void AnimateSquare(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            if (timer > timerDelay && animateSquares.Count - 1 > count)
            {
                if (count != 0) ChangeToRoad(animateSquares[count - 1].Item2);
                ChangeToCar(animateSquares[count].Item2);
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

