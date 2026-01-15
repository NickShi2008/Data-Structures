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
        public List<BoatSquare> Boats { get; set; } = new List<BoatSquare>();
        private HashSet<(int, int)> SquaresChecked { get; set; }
        public int NumOfLines { get; set; }
        public int SquaresInRow { get; set; }

        public List<Point> directions = new List<Point>
        {
            new Point(-1, 0), // left
            new Point(1, 0),  // right
            new Point(0, -1), // up
            new Point(0, 1)   // down
        };

        public List<Point> Circle = new List<Point>
        {
            new Point(-1, 0),
            new Point(1, 0),
            new Point(0, -1),
            new Point(0, 1),
            new Point(-1, -1),
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, 1),
        };

        private static Dictionary<Type, Func<int, int, ISquare>> GetSquare = new Dictionary<Type, Func<int, int, ISquare>>
        {
            [typeof(EmptySquare)] = (x, y) => new EmptySquare(x, y),
            [typeof(SeperationSquare)] = (x, y) => new SeperationSquare(x, y),
            [typeof(HouseSquare)] = (x, y) => new HouseSquare(x, y),
            [typeof(RoadSquare)] = (x, y) => new RoadSquare(x, y),
            [typeof(CarSquare)] = (x, y) => new CarSquare(x, y),
            [typeof(WaterSquare)] = (x, y) => new WaterSquare(x, y),
            [typeof(BoatSquare)] = (x, y) => new BoatSquare(x, y),
        };

        public int Size;
        public bool CanDrag;

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

            foreach (CarSquare car in Cars)
            {
                car.Draw(sb, new Point(Size - 8));
            }

            foreach (BoatSquare boat in Boats)
            {
                boat.Draw(sb, new Point(Size - 8));
            }
        }

        public void ClearGrid()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    Squares[i, j] = new EmptySquare(i * Size, j * Size);
                }
            }
            Cars.Clear();
            Boats.Clear();
        }

        RoadType FindRoadType(int x, int y, bool isWater = false)
        {
            bool left, right, up, down;

            if (isWater)
            {
                left = IsWater(x - 1, y);
                right = IsWater(x + 1, y);
                up = IsWater(x, y - 1);
                down = IsWater(x, y + 1);
            }
            else
            {
                left = IsRoad(x - 1, y);
                right = IsRoad(x + 1, y);
                up = IsRoad(x, y - 1);
                down = IsRoad(x, y + 1);
            }

            int connections = (left ? 1 : 0) + (right ? 1 : 0) + (up ? 1 : 0) + (down ? 1 : 0);

            // 4-way intersection
            if (connections == 4)
                return RoadType.CrossSection;

            // T-junctions (3 connections)
            if (connections == 3)
            {
                if (!up) return RoadType.UpJunction;
                if (!down) return RoadType.DownJunction;
                if (!left) return RoadType.LeftJunction;
                if (!right) return RoadType.RightJunction;
            }

            // Corners (2 connections at right angles)
            if (connections == 2)
            {
                if (up && right) return RoadType.BottomCornerLeft;
                if (up && left) return RoadType.BottomCornerRight;
                if (down && right) return RoadType.TopCornerLeft;
                if (down && left) return RoadType.TopCornerRight;

                // Straight roads
                if (up && down) return RoadType.Vertical;
                if (left && right) return RoadType.Horizontal;
            }

            // Single connection or dead end
            if (connections == 1)
            {
                if (up || down) return RoadType.Vertical;
                if (left || right) return RoadType.Horizontal;
            }

            // No connections - default
            return RoadType.Horizontal;
        }

        private bool IsRoad(int x, int y)
        {
            if (x < 0 || x >= SquaresInRow || y < 0 || y >= NumOfLines) return false;
            return Squares[x, y] is RoadSquare;
        }

        private bool IsWater(int x, int y)
        {
            if (x < 0 || x >= SquaresInRow || y < 0 || y >= NumOfLines) return false;
            return Squares[x, y] is WaterSquare;
        }

        public void PlaceSquare(int x, int y, ISquare square)
        {
            Point placePoint = new Point(x / Size, y / Size);

            if (!IsInBounds(placePoint, square))
                return;

            if (square is SeperationSquare)
            {
                PlaceRoad(placePoint);
            }
            else if (square is WaterSquare)
            {
                PlaceWater(placePoint);
            }
            else if (square is HouseSquare && Squares[placePoint.X, placePoint.Y] is EmptySquare)
            {
                // Only place house if next to road or water
                foreach (Point dir in directions)
                {
                    Point neighbor = placePoint + dir;
                    if (IsInBounds(neighbor, new RoadSquare(0, 0)))
                    {
                        if (Squares[neighbor.X, neighbor.Y] is RoadSquare ||
                            Squares[neighbor.X, neighbor.Y] is WaterSquare)
                        {
                            Squares[placePoint.X, placePoint.Y] = new HouseSquare(placePoint.X * Size, placePoint.Y * Size);
                            break;
                        }
                    }
                }
            }
            else if (square is CarSquare && Squares[placePoint.X, placePoint.Y] is RoadSquare)
            {
                // Only place car if next to a house
                foreach (Point dir in directions)
                {
                    Point neighbor = placePoint + dir;
                    if (IsInBounds(neighbor, new RoadSquare(0, 0)) && Squares[neighbor.X, neighbor.Y] is HouseSquare)
                    {
                        CarSquare car = new CarSquare(placePoint.X * Size, placePoint.Y * Size);
                        Cars.Add(car);
                        break;
                    }
                }
            }
            else if (square is BoatSquare && Squares[placePoint.X, placePoint.Y] is WaterSquare)
            {
                // Only place boat if next to a house
                foreach (Point dir in directions)
                {
                    Point neighbor = placePoint + dir;
                    if (IsInBounds(neighbor, new RoadSquare(0, 0)) && Squares[neighbor.X, neighbor.Y] is HouseSquare)
                    {
                        BoatSquare boat = new BoatSquare(placePoint.X * Size, placePoint.Y * Size);
                        Boats.Add(boat);
                        break;
                    }
                }
            }
            else if (square is EmptySquare)
            {
                Squares[placePoint.X, placePoint.Y] = new EmptySquare(placePoint.X * Size, placePoint.Y * Size);
            }
        }

        private void PlaceRoad(Point placePoint)
        {
            // Place separation square
            Squares[placePoint.X, placePoint.Y] = new SeperationSquare(placePoint.X * Size, placePoint.Y * Size);

            // Update all surrounding road squares
            foreach (Point dir in Circle)
            {
                Point neighbor = placePoint + dir;
                if (IsInBounds(neighbor, new RoadSquare(0, 0)))
                {
                    if (!(Squares[neighbor.X, neighbor.Y] is SeperationSquare))
                    {
                        RoadType roadType = FindRoadType(neighbor.X, neighbor.Y, false);
                        Squares[neighbor.X, neighbor.Y] = new RoadSquare(neighbor.X * Size, neighbor.Y * Size, roadType);
                    }
                }
            }
        }

        private void PlaceWater(Point placePoint)
        {
            // Place water square
            Squares[placePoint.X, placePoint.Y] = new WaterSquare(placePoint.X * Size, placePoint.Y * Size);

            // Update all surrounding water squares
            foreach (Point dir in Circle)
            {
                Point neighbor = placePoint + dir;
                if (IsInBounds(neighbor, new RoadSquare(0, 0)))
                {
                    if (Squares[neighbor.X, neighbor.Y] is WaterSquare)
                    {
                        RoadType waterType = FindRoadType(neighbor.X, neighbor.Y, true);
                        Squares[neighbor.X, neighbor.Y] = new WaterSquare(neighbor.X * Size, neighbor.Y * Size, waterType);
                    }
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

        public void UpdateNeighbors(int x, int y, Type type)
        {
            foreach (Point dir in directions)
            {
                Point neighbor = new Point(x, y) + dir;

                if (IsInBounds(neighbor, new RoadSquare(0, 0)))
                {
                    if (Squares[neighbor.X, neighbor.Y] is SeperationSquare)
                    {
                        RoadType neighborType = FindRoadType(neighbor.X, neighbor.Y, false);
                        UpdateRoads(neighbor.X, neighbor.Y, neighborType);
                    }
                }
            }
        }

        private void UpdateRoads(int x, int y, RoadType type)
        {
            foreach (Point dir in Circle)
            {
                Point neighbor = new Point(x, y) + dir;
                if (IsInBounds(neighbor, new RoadSquare(0, 0)))
                {
                    if (!(Squares[neighbor.X, neighbor.Y] is SeperationSquare))
                    {
                        RoadType roadType = FindRoadType(neighbor.X, neighbor.Y, false);
                        Squares[neighbor.X, neighbor.Y] = new RoadSquare(neighbor.X * Size, neighbor.Y * Size, roadType);
                    }
                }
            }
        }
    }
}