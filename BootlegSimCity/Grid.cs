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
        public ISquare SquareClicked { get; set; }
        public bool hasStored = false;
        public Point StoredPoint { get; set; }

        private int Size;

        public bool EnableDrag = false;

        private static Dictionary<Type, Func<int, int, ISquare>> GetSquare = new Dictionary<Type, Func<int, int, ISquare>>
        {
            [typeof(EmptySquare)] = (x, y) => new EmptySquare(x,y),
            [typeof(RoadSquare)] = (x,y) => new RoadSquare(x,y),
            [typeof(HouseSquare)] = (x, y) => new HouseSquare(x, y),
        };
        //static ISquare Funcy(int x,int y)
        //{
        //    return new EmptySquare(x, y);
        //}

        public Grid(int lines, GraphicsDeviceManager graphics)
        {
            NumOfLines = lines;
            Size = graphics.PreferredBackBufferWidth / lines;
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
            foreach (var square in Squares)
            {
                square.Draw(sb, new Point(Size - 1));
            }
        }

        public void StoreClick(int x, int y)
        {
            StoredPoint = new Point(x/Size, y/Size);

            EmptySquare empty = new EmptySquare(0, 0);
            RoadSquare block = new RoadSquare(0, 0);
            EnableDrag = true;

            if (SquareClicked != null)
            {
                PlaceSquare(x, y);
                return;
            }

            SquareClicked = GetSquare[(Type)Squares[StoredPoint.X, StoredPoint.Y]].Invoke(StoredPoint.X, StoredPoint.Y);
            hasStored = true;

            if (hasStored)
            {
                EnableDrag = false;
            }

            
        }

        public void PlaceSquare(int x, int y)
        {
            Point point = new Point(x / Size, y / Size);

            Point store = Squares[point.X, point.Y].Location;
            ISquare square = Squares[point.X, point.Y];

            if (SquareClicked.Location != Squares[point.X, point.Y].Location)
            {
                Squares[StoredPoint.X, StoredPoint.Y] = square;
                Squares[StoredPoint.X, StoredPoint.Y].Location = SquareClicked.Location;
            }
            Squares[point.X, point.Y] = SquareClicked;
            Squares[point.X, point.Y].Location = store;

            hasStored = false;
        }
    }
}
