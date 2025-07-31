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

        public int NumOfLines { get; set; }
        public ISquare SquareReplacement { get; set; }
        public Point? SquareIndex { get; set; }

        private int Size;
        public bool CanDrag;
        //Factory fun
        private static Dictionary<Type, Func<int, int, ISquare>> GetSquare = new Dictionary<Type, Func<int, int, ISquare>>
        {
            [typeof(EmptySquare)] = (x, y) => new EmptySquare(x,y),
            [typeof(SeperationSquare)] = (x,y) => new SeperationSquare(x,y),
            [typeof(HouseSquare)] = (x, y) => new HouseSquare(x, y),
            [typeof(RoadSquare)] = (x, y) => new RoadSquare(x, y),
            [typeof(CarSquare)] = (x, y) => new CarSquare(x, y),
        };
        //static ISquare Funcy(int x,int y)
        //{
        //    return new EmptySquare(x, y);
        //}

        public Grid(int lines, int SCREENSIZE)
        {
            NumOfLines = lines;
            Size = SCREENSIZE / lines;
            Squares = new ISquare[lines, lines];
            InitGrid();
            CanDrag = true;
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
        }

        public void UpdatePossibleSquare(SpriteBatch sb, ISquare square, Point mouse)
        {
            int x = mouse.X; 
            int y = mouse.Y;
            Point placePoint = new Point(x / Size, y / Size);

            if (isInBounds(placePoint, square))
            {
                Squares[placePoint.X, placePoint.Y] = GetSquare[square.GetType()].Invoke(placePoint.X * Size, placePoint.Y * Size);
                if (Squares[placePoint.X, placePoint.Y] is SeperationSquare)
                {
                    RoadSquare road = new RoadSquare(0, 0);
                    PlaceSideSquares(placePoint.X, placePoint.Y, road, true);
                }
            }
        }

        public void PlaceSquare(int x, int y, ISquare square)
        {
            Point placePoint = new Point(x / Size, y / Size);

            if (isInBounds(placePoint, square))
            {
                if (placePoint.X == x && placePoint.Y == y && Squares[placePoint.X, placePoint.Y].GetType() == square.GetType())
                {
                    CanDrag = false;
                }
                else
                {
                    Squares[placePoint.X, placePoint.Y] = GetSquare[square.GetType()].Invoke(placePoint.X * Size, placePoint.Y * Size);
                    if (Squares[placePoint.X, placePoint.Y] is SeperationSquare)
                    {
                        RoadSquare road = new RoadSquare(0, 0);
                        PlaceSideSquares(placePoint.X, placePoint.Y, road, true);
                    }
                    CanDrag = true;
                }
            }

        }

        private bool isInBounds(Point placePoint, ISquare square)
        {
            bool borderCheck = placePoint.X >= 0 && placePoint.X < NumOfLines && placePoint.Y >= 0 && placePoint.Y < NumOfLines;
            if (square is SeperationSquare)
            {
                borderCheck = placePoint.X >= 1 && placePoint.X < NumOfLines - 1 && placePoint.Y >= 1 && placePoint.Y < NumOfLines - 1;
            }
            return borderCheck;
        }

        public void PlaceSideSquares(int x, int y, ISquare square, bool isSideways)
        {
             Squares[x, y - 1] = GetSquare[square.GetType()].Invoke(x * Size, (y - 1) * Size);
             Squares[x, y + 1] = GetSquare[square.GetType()].Invoke(x * Size, (y  + 1) * Size);
        }
    }
}
