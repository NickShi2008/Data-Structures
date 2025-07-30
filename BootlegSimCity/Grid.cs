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
        public ISquare CurrentSquare { get; set; }
        public Point? StoredPoint { get; set; }

        private int Size;


        //Factory fun
        private static Dictionary<Type, Func<int, int, ISquare>> GetSquare = new Dictionary<Type, Func<int, int, ISquare>>
        {
            [typeof(EmptySquare)] = (x, y) => new RoadSquare(x,y),
            [typeof(RoadSquare)] = (x,y) => new EmptySquare(x,y),
            [typeof(HouseSquare)] = (x, y) => new EmptySquare(x, y),
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

        public void StoreClick(int x, int y)
        {
            Point squareClicked = new Point(x / Size, y / Size);
            if (squareClicked.X >= 0 && squareClicked.X < NumOfLines
                && squareClicked.Y >= 0 && squareClicked.Y < NumOfLines)
            {
                CurrentSquare = Squares[squareClicked.X, squareClicked.Y];
                StoredPoint = squareClicked;
            }
        }

        public void PlaceSquare(int x, int y)
        {
            if (CurrentSquare == null || StoredPoint == null)
                return;

            Point targetPoint = new Point(x / Size, y / Size);
            if (targetPoint.X >= 0 && targetPoint.X < NumOfLines && targetPoint.Y >= 0 && targetPoint.Y < NumOfLines)
            {
                ISquare temp = Squares[targetPoint.X, targetPoint.Y];
                Squares[targetPoint.X, targetPoint.Y] = CurrentSquare;
                Squares[StoredPoint.X, StoredPoint.Y] = temp;

                Squares[targetPoint.X, targetPoint.Y].Location = new Point(targetPoint.X * Size, targetPoint.Y * Size);
                Squares[StoredPoint.X, StoredPoint.Y].Location = new Point(StoredPoint.X * Size, StoredPoint.Y * Size);
            }

            CurrentSquare = null;
            StoredPoint = null;
        }
    }
}
