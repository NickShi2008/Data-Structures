using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace PathFindingVis
{
    public interface ISquare
    {
        public Point location { get; set; }
        abstract void Draw(SpriteBatch sb, Point size);

    }

    public class EmptySquare : ISquare
    {
        public Point location { get; set; }

        public EmptySquare(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.AntiqueWhite);
        }
    }

    public class PathSquare : ISquare
    {
        public Point location { get; set; }

        public PathSquare(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MediumSlateBlue);
        }
    }

    public class BlockSquare : ISquare
    {
        public Point location { get; set; }

        public BlockSquare(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.LightSlateGray);
        }
    }
    public class StartSquare : ISquare
    {
        public Point location { get; set; }

        public StartSquare(int x, int y)
        {
            location = new Point(x, y);
        }

        public StartSquare(StartSquare start)
        {
            location = start.location;
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MediumSpringGreen);
        }
    }

    public class SearchdSquares : ISquare
    {
        public Point location { get; set; }

        public SearchdSquares(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.LightYellow);
        }
    }

    public class NeighbourSquares : ISquare
    {
        public Point location { get; set; }

        public NeighbourSquares(int x, int y)
        {
            location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb,  Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MonoGameOrange);
        }
    }

    public class EndSquare : ISquare
    {
        public Point location { get; set; }

        public EndSquare(int x, int y)
        {
            location = new Point(x, y);
        }

        public EndSquare(EndSquare end)
        {
            location = end.location;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(location, size), Color.MediumVioletRed);
        }
    }

    public class Grid <T>
    {
        public int NumOfLines { get; set; }

        public ISquare[,] Squares { get; set; }

        public ISquare squareClicked;
        public Point storedIndex;
        public bool hasStored = false;

        private int size;

        public bool EnableDrag = false;

        public Grid(int lines, GraphicsDeviceManager graphics)
        {
            NumOfLines = lines;
            size = graphics.PreferredBackBufferWidth / lines;
            Squares = new ISquare[lines, lines];
            InitGrid();
        }

        private void InitGrid()
        {
            for (int i = 0; i < Squares.GetLength(0); i++)
            {
                for (int j = 0; j < Squares.GetLength(1); j++)
                {
                    Squares[i, j] = new EmptySquare(i * size, j * size);
                }
            }

            Squares[0, Squares.GetLength(1) - 1] = new StartSquare(0, (Squares.GetLength(1) - 1) * size);
            Squares[Squares.GetLength(0) - 1, 0] = new EndSquare((Squares.GetLength(0) - 1) * size ,0);
        }


        public void DrawGrid(SpriteBatch spriteBatch)
        {
            foreach (var square in Squares)
            {
                square.Draw(spriteBatch, new Point(size - 1)); 
            }
        }

        

        public void StoreMouseClick(int x, int y)
        {
            //fix enable drag so it doesn't repeat on one thing
            storedIndex = new Point(x/size, y/size);

            
            

            EmptySquare empty = new EmptySquare(0,0);
            BlockSquare block = new BlockSquare(0, 0);
            EnableDrag = true;

            if (squareClicked != null && (squareClicked.GetType() == empty.GetType() || squareClicked.GetType() == block.GetType()) && squareClicked == Squares[storedIndex.X, storedIndex.Y])
            {
                return;
            }
            squareClicked = Squares[storedIndex.X, storedIndex.Y];

            if (squareClicked.GetType() == empty.GetType())
            {
             
                squareClicked = new BlockSquare(Squares[storedIndex.X, storedIndex.Y].location.X, Squares[storedIndex.X, storedIndex.Y].location.Y);
                PlaceSquare(x, y);
                
            }
            else if(squareClicked.GetType() == block.GetType())
            {
         
                squareClicked = new EmptySquare(Squares[storedIndex.X, storedIndex.Y].location.X, Squares[storedIndex.X, storedIndex.Y].location.Y);
                PlaceSquare(x, y);
            }
            else
            {
                EnableDrag = false;
                hasStored = true;
            }
        }

        

        public void PlaceSquare(int x, int y)
        {
            Point point = new Point(x / size, y / size);
            //new clicked location
            Point store = Squares[point.X, point.Y].location;
            ISquare square = Squares[point.X, point.Y];
            
            //testing to see if I can get rid of black square
            
            

            if (squareClicked.location != Squares[point.X, point.Y].location)
            {
                Squares[storedIndex.X, storedIndex.Y] = square;
                Squares[storedIndex.X, storedIndex.Y].location = squareClicked.location;
            }
            Squares[point.X, point.Y] = squareClicked;
            Squares[point.X, point.Y].location = store;

            hasStored = false;
        }

    }
}
