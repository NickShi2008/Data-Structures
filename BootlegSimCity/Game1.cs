using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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
        private int count = 0;
        private bool hasReset = true;
        const double SCREENSIZE = 800;
        private SelectScreen selectScreen;
        SpriteFont spriteFont;
        private bool hasGridChanged = false;
        private Point? lastPlacedCell = null;
        private int SquareDistance;
        private Dictionary<Point, ISquare> houseSquares = new Dictionary<Point, ISquare>();
        private List<Point> points = new List<Point>();
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
            SquareDistance = 1;
            graph = new Graph<Point>();
            grid = new Grid<Point>(40, (int) (SCREENSIZE), (int) (SCREENSIZE));
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

            for (int car = 0; car < grid.Cars.Count; car++)
            {
                if (grid.Cars[car].Path.Count <= 0)
                {
                   
                    Vertex<Point> houseTwo = FindHouse();
                    foreach (Point point in grid.directions)
                    {
                        Point newPoint = point + houseTwo.Value;
                        if (grid.IsInBounds(newPoint, new RoadSquare(0, 0)) && grid.Squares[newPoint.X, newPoint.Y] is RoadSquare houseSquare)
                        {
                            houseTwo = graph.Vertices[new Point(newPoint.X, newPoint.Y)];
                            break;
                        }
                    }
                    (List<Vertex<Point>>, float) tracker;
                    if (houseTwo != null)
                    {
                        tracker = graph.ASTAR(graph.Vertices[grid.Cars[car].Location/new Point(grid.Size, grid.Size)]
                            , houseTwo, Manhattan);
                        for (int i = 0; i < tracker.Item1.Count; i++)
                        {
                            grid.Cars[car].Path.Enqueue(tracker.Item1[i], i);
                        }
                    }
                }
            }



            AnimateCars(gameTime);

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

            if(!selectScreen.hasErased)
            {
                grid.ClearGrid();
                graph = new Graph<Point>();
                houseSquares.Clear();
                selectScreen.hasErased = true;
                hasGridChanged = true;

            }


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
            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    Point point = new Point(i, j);

                    if(graph.Vertices.ContainsKey(point) && (grid.Squares[i,j] is EmptySquare || grid.Squares[i,j] is SeperationSquare))
                    {
                        if (grid.Squares[point.X, point.Y] is not RoadSquare)
                        {
                            graph.RemoveVertexAndEdges(point);
                        }
                        else if (grid.Squares[point.X, point.Y] is not HouseSquare)
                        {
                            graph.RemoveVertexAndEdges(point);
                            houseSquares.Remove(point);
                        }
                        continue; 
                    }
                    
                    if (grid.Squares[i, j] is RoadSquare)
                    {
                        graph.AddVertex(point);
                        grid.UpdateNeighbors(i, j, grid.Squares[i,j].GetType());
                    }
                    else if (grid.Squares[i, j] is HouseSquare && !houseSquares.ContainsKey(new Point(i,j)))
                    {
                        graph.AddVertex(point);
                        houseSquares.Add(point, (HouseSquare)grid.Squares[i, j]);
                        points.Add(point);
                        foreach (Point dir in grid.directions)
                        {
                            Point newPoint = point + dir;
                            if (grid.IsInBounds(newPoint, new RoadSquare(0,0)) && grid.Squares[newPoint.X, newPoint.Y] is RoadSquare roadSquare)
                            {
                                graph.AddEdge(point, newPoint, SquareDistance);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < grid.Squares.GetLength(0); i++)
            {
                for (int j = 0; j < grid.Squares.GetLength(1); j++)
                {
                    if (!(grid.Squares[i, j] is RoadSquare))
                    {
                        continue;
                    }
                    Vertex<Point> currentVertex = graph.Vertices[new Point(i, j)];

                    List<Point> Neighbors = new List<Point>();

                    Neighbors.Add(new Point(i - 1, j));
                    Neighbors.Add(new Point(i, j - 1));
                    Neighbors.Add(new Point(i + 1, j));
                    Neighbors.Add(new Point(i, j + 1));


                    for (int k = Neighbors.Count - 1; k >= 0; k--)
                    {
                        if (Neighbors[k].X >= grid.SquaresInRow || Neighbors[k].X <= 0
                            || Neighbors[k].Y >= grid.NumOfLines || Neighbors[k].Y <= 0)
                        {
                            Neighbors.Remove(Neighbors[k]);
                        }
                    }

                    foreach (var neigh in Neighbors)
                    {
                        if (grid.Squares[neigh.X, neigh.Y] is RoadSquare rSquare)
                        {
                            Vertex<Point> neighborVertex = graph.Vertices[neigh];
                            ConnectRoads(rSquare);
                        }
                    }
                }
            }
        }
           

        Vertex<Point> FindHouse()
        {
            if(houseSquares.Count == 0)
            {
                return null;
            }
            Random rand = new Random();
            int houseNum = rand.Next(0, houseSquares.Count);
            return graph.Vertices[houseSquares[points[houseNum]].Location/new Point(grid.Size, grid.Size)];
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
        void AnimateCars(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * 5;
            if (timer > timerDelay)
            {
                foreach(CarSquare car in grid.Cars)
                {
                    if (car.Path.Count > 0)
                        car.Location = car.Path.Dequeue().Value * new Point(grid.Size, grid.Size);

                }
                timer = 0;
            }
        }

        //based on type of roadsquare deteect where seperation square is (know you know orientation)
        //if roadsquare is on "Right" side connect a->b other wise b->a
        //ex: horizontal, detect seperation is on left side then right side road, vice versa
        void ConnectRoads(RoadSquare square)
        {
            int x = square.Location.X / grid.Size;
            int y = square.Location.Y / grid.Size; 
            Vertex<Point> a = new Vertex<Point>(new Point(x,y));
            Vertex<Point> b = null;
            Vertex<Point> c = null;
            switch (square.roadType)
            {
                case RoadType.Vertical:
                    
                    if (grid.Squares[x - 1, y] is SeperationSquare)
                    {
                        b = new Vertex<Point>(new Point(x, y - 1));
                    }
                    else if(grid.Squares[x + 1, y] is SeperationSquare)
                    {
                        b = new Vertex<Point>(new Point(x, y + 1));
                    }
                    else
                    {
                        b = null;
                        throw new NullReferenceException();
                    }

                    if(grid.IsInBounds(new Point(x - 1, y), new RoadSquare(0, 0)) && grid.IsInBounds(new Point(x + 1, y), new RoadSquare(0, 0)) 
                        && grid.Squares[x - 1, y] is SeperationSquare && grid.Squares[x + 1, y] is SeperationSquare )
                    {
                        c = new Vertex<Point>(new Point(x, y + 1));
                        b = new Vertex<Point>(new Point(x, y - 1));
                        graph.AddEdge(a.Value, c.Value, SquareDistance);
                    }

                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.Horizontal:
                    if (grid.Squares[x, y - 1] is SeperationSquare)
                    {
                        b = new Vertex<Point>(new Point(x + 1, y));
                    }
                    else if (grid.Squares[x,y + 1] is SeperationSquare)
                    {
                        b = new Vertex<Point>(new Point(x - 1, y));
                    }
                    else
                    {
                        b = null;
                        throw new NullReferenceException();
                    }

                    if (grid.IsInBounds(new Point(x - 1, y), new RoadSquare(0, 0)) && grid.IsInBounds(new Point(x + 1, y), new RoadSquare(0, 0)) && 
                        grid.Squares[x, y - 1] is SeperationSquare && grid.Squares[x, y + 1] is SeperationSquare)
                    {
                        c = new Vertex<Point>(new Point(x - 1, y));
                        b = new Vertex<Point>(new Point(x + 1, y));
                        graph.AddEdge(a.Value, c.Value, SquareDistance);
                    }

                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.TopCornerLeft:
                    b = new Vertex<Point>(new Point(x, y + 1));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.TopCornerRight:
                    b = new Vertex<Point>(new Point(x - 1, y));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.BottomCornerLeft:
                    b = new Vertex<Point>(new Point(x + 1, y));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.BottomCornerRight:
                    b = new Vertex<Point>(new Point(x, y - 1));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    break;
                case RoadType.UpJunction:
                    c = new Vertex<Point>(new Point(x + 1, y));
                    b = new Vertex<Point>(new Point(x, y + 1));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    break;
                case RoadType.DownJunction:
                    c = new Vertex<Point>(new Point(x - 1, y));
                    b = new Vertex<Point>(new Point(x, y - 1));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    break;
                case RoadType.RightJunction:
                    
                   /* if (grid.Squares[x + 1, y] is SeperationSquare)
                    {
                        c = new Vertex<Point>(new Point(x, y - 1));
                    }
                    else
                    {*/
                        c = new Vertex<Point>(new Point(x, y - 1));
                        b = new Vertex<Point>(new Point(x - 1, y));
                        graph.AddEdge(a.Value, b.Value, SquareDistance);
                   // }
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    break;
                case RoadType.LeftJunction:
                   
                    if (grid.Squares[x-1,y] is SeperationSquare)
                    {
                        c = new Vertex<Point>(new Point(x, y - 1));
                        b = new Vertex<Point>(new Point(x + 1, y));
                        graph.AddEdge(a.Value, b.Value, SquareDistance);
                        
                    }
                    else
                    {
                        c = new Vertex<Point>(new Point(x, y + 1));
                    }
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    break;
                case RoadType.CrossSection:
                    b = new Vertex<Point>(new Point(x - 1, y));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    c = new Vertex<Point>(new Point(x + 1, y));
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    b = new Vertex<Point>(new Point(x, y - 1));
                    graph.AddEdge(a.Value, b.Value, SquareDistance);
                    c = new Vertex<Point>(new Point(x, y + 1));
                    graph.AddEdge(a.Value, c.Value, SquareDistance);
                    break;
            }
            foreach (Point dir in grid.directions)
            {
                Point newPoint = a.Value + dir;
                if (grid.IsInBounds(newPoint, new RoadSquare(0, 0)) && grid.Squares[newPoint.X, newPoint.Y] is HouseSquare houseSquare)
                {
                    graph.AddEdge(a.Value, newPoint, SquareDistance);
                }
            }
        }
    }
}

