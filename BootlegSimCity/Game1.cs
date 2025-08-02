using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace BootlegSimCity
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
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
        private bool hasGridChanged = false;
        private Point? lastPlacedCell = null;
        private int SquareDistance;
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
            selectScreen = new SelectScreen((int) (SCREENSIZE));

            animateSquares = new List<(ISquare, Vertex<Point>)>();
            graph = new Graph<Point>();
            grid = new Grid<Point>(20, (int) (SCREENSIZE), (int) (SCREENSIZE));
            spriteFont = Content.Load<SpriteFont>("Ubuntu32");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if(hasGridChanged)
            {
                ConnectGrid();
            }
                
            Vertex<Point> houseOne = FindHouse();
            Vertex<Point> houseTwo = FindHouse(houseOne);
            (List<Vertex<Point>>, float) tracker;
            if (houseOne != null && houseTwo != null)
            {
                tracker = graph.ASTAR(houseOne, houseTwo, Manhattan);
                animateSquares.Clear();
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
            if (ms.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                selectScreen.FindSelectedSquare(ms.Position);

           
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Point currentCell = new Point(ms.X / grid.Size, ms.Y / grid.Size);

                if (lastPlacedCell == null || (lastPlacedCell != currentCell))
                {
                    grid.PlaceSquare(ms.X, ms.Y, selectScreen.GetCurrentSquare());
                    lastPlacedCell = currentCell;
                    hasGridChanged = true;
                }
            }
            else
            {
               // lastPlacedCell = null;
                hasGridChanged = false;
            }

            lastMouseState = ms;



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            grid.DrawGrid(spriteBatch);

            selectScreen.DrawSelectScreen(spriteBatch, graphics, spriteFont);


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

                    if (grid.Squares[i, j] is RoadSquare)
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
                        if (grid.Squares[neigh.X, neigh.Y] is RoadSquare rSquare)
                        {
                            var neighborVertex = graph.Search(neigh);
                            ///ConnectRoads(rSquare);
                            graph.AddEdge(neighborVertex, currentVertex, 1);
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

        //cars drive on right side of the road
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

        //based on type of roadsquare deteect where seperation square is (know you know orientation)
        //if roadsquare is on "Right" side connect a->b other wise b->a
        //ex: horizontal, detect seperation is on left side then right side road, vice versa
        void ConnectRoads(RoadSquare square)
        {
            int x = square.Location.X / grid.Size;
            int y = square.Location.Y / grid.Size; 
            Vertex<Point> a = new Vertex<Point>(square.Location);
            Vertex<Point> b;
            switch (square.roadType)
            {
                case RoadType.Vertical:
                    
                    if (grid.Squares[x - 1, y] is SeperationSquare)
                    {
                        b = new Vertex<Point> (grid.Squares[x, y - 1].Location);
                    }
                    else if(grid.Squares[x + 1, y] is SeperationSquare)
                    {
                        b = new Vertex<Point>(grid.Squares[x, y + 1].Location);
                    }
                    else
                    {
                        b = null;
                        throw new NullReferenceException();
                    }
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.Horizontal:
                    if (grid.Squares[x, y - 1] is SeperationSquare)
                    {
                        b = new Vertex<Point>(grid.Squares[x + 1, y].Location);
                    }
                    else if (grid.Squares[x,y + 1] is SeperationSquare)
                    {
                        b = new Vertex<Point>(grid.Squares[x - 1, y + 1].Location);
                    }
                    else
                    {
                        b = null;
                        throw new NullReferenceException();
                    }
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.TopCornerLeft:
                    b = new Vertex<Point>(grid.Squares[x, y + 1].Location);
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.TopCornerRight:
                    b = new Vertex<Point>(grid.Squares[x - 1, y].Location);
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.BottomCornerLeft:
                    b = new Vertex<Point>(grid.Squares[x + 1, y].Location);
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.BottomCornerRight:
                    b = new Vertex<Point>(grid.Squares[x, y - 1].Location);
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.UpJunction:
                    b = new Vertex<Point>(grid.Squares[x, y - 1].Location);
                    graph.AddEdge(a, b, SquareDistance);
                    break;
                case RoadType.DownJunction:
                    break;
                case RoadType.RightJunction:
                    break;
                case RoadType.LeftJunction:
                    break;
                case RoadType.CrossSection:
                    break; ;

            }

        }
    }
}

