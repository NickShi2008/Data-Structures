using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootlegSimCity
{
    public class Grid<T>
    {
        public ISquare[,] Squares { get; set; }
        public List<CarSquare> Cars { get; set; } = new List<CarSquare>();
        private HashSet<(int, int)> SquaresChecked { get; set; }
        public int NumOfLines { get; set; }
        public int SquaresInRow { get; set; }

        public List<Point> directions = new List<Point>
            {
                left,
                right,
                up,
                down
            };

        public List<Point> Circle = new List<Point>
            {
                left,
                right,
                up,
                down,
                new Point(-1, -1),
                new Point(1, 1),
                new Point(1, -1),
                new Point(-1, 1),
            };


        static Point left = new Point(-1, 0);
        static Point right = new Point(1, 0);
        static Point up = new Point(0, -1);
        static Point down = new Point(0, 1);
        private Dictionary<RoadType, List<Point>> RoadCreator = new()
        {
            
            [RoadType.Vertical] = new() { left, right },
            [RoadType.Horizontal] = new() { up, down },
            [RoadType.TopCornerLeft] = new() { up, left, new (-1, -1), new(1, 1) },
            [RoadType.TopCornerRight] = new() { right, up, new (1,-1), new (-1, 1) },
            [RoadType.BottomCornerLeft] = new() { down, left, new(-1, -1) },
            [RoadType.BottomCornerRight] = new() { down, right, new(1, -1) },
            [RoadType.UpJunction] = new() { down, new(-1, -1), new(-1, 1), new(1, -1), new(1, 1) },
            [RoadType.DownJunction] = new() {up, new(-1, 1), new(-1, -1), new(1, 1), new(1, -1) },
            [RoadType.RightJunction] = new() { left, new(1, -1), new(-1, -1), new(1, 1), new (-1, 1) },
            [RoadType.LeftJunction] = new() { right, new(1, -1), new(-1, -1), new(1, 1), new (-1, 1) },
            [RoadType.CrossSection] = new() { new(-1, -1), new(1, 1), new(1, -1), new(-1, 1) },
            //[RoadType.Circle] = new() { new(-1, -1), new (-1,0), new(1, 0), new(1, 1), new(1, -1), new(0, -1), new(-1, 1), new(0, 1) },
        };

       
     
        public int Size;
        public bool CanDrag;
        //Factory fun
        private static Dictionary<Type, Func<int, int, ISquare>> GetSquare = new Dictionary<Type, Func<int, int, ISquare>>
        {
            [typeof(EmptySquare)] = (x, y) => new EmptySquare(x, y),
            [typeof(SeperationSquare)] = (x, y) => new SeperationSquare(x, y),
            [typeof(HouseSquare)] = (x, y) => new HouseSquare(x, y),
            [typeof(RoadSquare)] = (x, y) => new RoadSquare(x, y),
            [typeof(CarSquare)] = (x, y) => new CarSquare(x, y),
        };
        //static ISquare Funcy(int x,int y)
        //{
        //    return new EmptySquare(x, y);
        //}

        public Grid(int lines, int SCREENHeight, int SCREENWIDTH)
        {
            NumOfLines = lines;
            
            Size = SCREENHeight / lines;
            SquaresInRow = SCREENWIDTH / Size;
            Squares = new ISquare[SquaresInRow, lines];
            InitGrid();
            CanDrag = true;
            SquaresChecked = new HashSet<(int, int)>();
        }

        private void InitGrid()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    Squares[i, j] = new EmptySquare(i * Size, j * Size);
                }
            }
        }

        public void DrawGrid(SpriteBatch sb)
        {
            foreach (ISquare square in Squares)
            {
                square.Draw(sb, new Point(Size - 1));
            }

            foreach(CarSquare car in Cars)
            {
                car.Draw(sb, new Point(Size - 8));
            }
        }

        /*  public void UpdatePossibleSquare(SpriteBatch sb, ISquare square, Point mouse)
          {
              int x = mouse.X; 
              int y = mouse.Y;
              Point placePoint = new Point(x / Size, y / Size);

              if (isInBounds(placePoint, square))
              {
                  Squares[placePoint.X, placePoint.Y].PreviewSquare = GetSquare[square.GetType()].Invoke(placePoint.X * Size, placePoint.Y * Size);
                  if (Squares[placePoint.X, placePoint.Y] is SeperationSquare)
                  {
                      Squares[placePoint.X, placePoint.Y]
                  }
              }
          }*/

        RoadType FindRoadType(int x, int y)
        {
            bool left = IsRoad(x - 1, y);
            bool right = IsRoad(x + 1, y);
            bool up = IsRoad(x, y - 1);
            bool down = IsRoad(x, y + 1);

            int connections = (left ? 1 : 0) + (right ? 1 : 0) + (up ? 1 : 0) + (down ? 1 : 0);
            if (up && right && !down && !left) return RoadType.BottomCornerLeft;
            if (up && left && !down && !right) return RoadType.BottomCornerRight;
            if (down && left && !up && !right) return RoadType.TopCornerRight;
            if (down && right && !up && !left) return RoadType.TopCornerLeft;
            if (left && right && down && !up) return RoadType.UpJunction;
            if (left && right && up && !down) return RoadType.DownJunction;
            if (up && down && right && !left) return RoadType.LeftJunction;
            if (up && down && left && !right) return RoadType.RightJunction;
            if (connections >= 3) return RoadType.CrossSection;
            if (up && down && !left && !right) return RoadType.Vertical;
            if (left && right && !up && !down) return RoadType.Horizontal;
           
           // if (connections == 0) return RoadType.Circle;

            if ((up && !down && !left && !right) || (!up && down && !left && !right)) return RoadType.Vertical;
            if ((left && !right && !up && !down) || (!left && right && !up && !down)) return RoadType.Horizontal;


            return RoadType.Horizontal;
        }
        private bool IsRoad(int x, int y)
        {
            if (x < 0 || x >= SquaresInRow || y < 0 || y >= NumOfLines) return false;
            return Squares[x, y] is RoadSquare; //|| Squares[x, y] is SeperationSquare;
        }

        public void PlaceSquare(int x, int y, ISquare square)
        {
            Point placePoint = new Point(x / Size, y / Size);

            if (IsInBounds(placePoint, square))
            {
                if (placePoint.X == x && placePoint.Y == y && Squares[placePoint.X, placePoint.Y].GetType() == square.GetType())
                {
                    CanDrag = false;
                }
                else
                {

                    if (square is SeperationSquare)
                    {
                        RoadType roadType = FindRoadType(placePoint.X, placePoint.Y);
                        UpdateRoads(placePoint.X, placePoint.Y, roadType);

                        if (!SquaresChecked.Contains((placePoint.X,placePoint.Y)))
                            UpdateNeighbors(placePoint.X, placePoint.Y);
                    }
                    else if (square is HouseSquare && Squares[placePoint.X,placePoint.Y] is EmptySquare)
                    {
                        foreach (Point neigh in directions)
                        {
                            int newX = placePoint.X + neigh.X;
                            int newY = placePoint.Y + neigh.Y;

                            if (IsInBounds(new Point(newX, newY), new RoadSquare(newX, newY)) && Squares[newX,newY] is RoadSquare)
                            {
                                Squares[placePoint.X, placePoint.Y] = GetSquare[square.GetType()].Invoke(placePoint.X * Size, placePoint.Y * Size);
                            }
                                
                        }
                    }
                    else if (square is CarSquare && Squares[placePoint.X, placePoint.Y] is RoadSquare)
                    {
                        foreach (Point neigh in directions)
                        {
                            int newX = placePoint.X + neigh.X;
                            int newY = placePoint.Y + neigh.Y;

                            if (Squares[newX, newY] is HouseSquare)
                            {
                                CarSquare car = new CarSquare(placePoint.X * Size, placePoint.Y * Size);
                                Cars.Add(car);
                            }

                        }
                    }
                    else if(square is EmptySquare)
                    {
                        Squares[placePoint.X, placePoint.Y] = GetSquare[square.GetType()].Invoke(placePoint.X * Size, placePoint.Y * Size);
                    }

                     CanDrag = true;
                }
            }

        }

        public bool IsInBounds(Point point, ISquare square)
        {

            bool borderCheck = point.X >= 0 && point.X < SquaresInRow && point.Y >= 0 && point.Y < NumOfLines;
            if (square is SeperationSquare)
            {
                borderCheck = point.X >= 1 && point.X < SquaresInRow - 1 && point.Y >= 1 && point.Y < NumOfLines - 1;
            }
            return borderCheck;
        }

        private void UpdateRoads(int x, int y, RoadType type)
        {
            Squares[x, y] = GetSquare[typeof(SeperationSquare)].Invoke(x * Size, y * Size);
            SquaresChecked.Remove((x, y));
            RoadSquare road = new RoadSquare(0, 0);
           // List<Point> creator = RoadCreator[type];
            foreach (Point point in Circle)
            {
                int newX = x + point.X;
                int newY = y + point.Y;
                if (newX >= 0 && newX < SquaresInRow && newY >= 0 && newY < NumOfLines)
                {
                    /*if (newX == origin.X && newY == origin.Y && Squares[newX, newY] is SeperationSquare)
                      {
                          continue;
                      }*/
                    if (!(Squares[newX, newY] is SeperationSquare) && !SquaresChecked.Contains((newX, newY)))
                    {
                        RoadType neighborType = FindRoadType(newX, newY);
                        Squares[newX, newY] = new RoadSquare(newX * Size, newY * Size, neighborType);
                        SquaresChecked.Remove((newX, newY));
                        //UpdateNeighbors(newX, newY);
                    }
                }
            }

        }

        public void UpdateNeighbors(int x, int y)
        {

            foreach (Point neigh in directions)
            {
                int newX = x + neigh.X;
                int newY = y + neigh.Y;

                if (newX >= 0 && newX < SquaresInRow && newY >= 0 && newY < NumOfLines)
                {
                    if (Squares[newX, newY] is SeperationSquare && !SquaresChecked.Contains((newX,newY)))
                    {
                        RoadType neighborType = FindRoadType(newX, newY);
                        SquaresChecked.Add((newX, newY));
                        UpdateRoads(newX, newY, neighborType);
                        
                    }
                }
            }
        }

        

    }
}
