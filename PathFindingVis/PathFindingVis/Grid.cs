using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFindingVis
{
    interface ISquare
    {
        void Draw()
        { }
    }

    public class EmptySquare : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.AntiqueWhite);
        }
    }

    public class BlockSquare : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.LightSlateGray);
        }
    }
    public class StartSquare : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.MediumSpringGreen);
        }
    }

    public class SearchdSquares : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.LightGoldenrodYellow);
        }
    }

    public class NeighbourSquares : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.MonoGameOrange);
        }
    }

    public class EndSquare : ISquare
    {
        void Draw(SpriteBatch sb, Point location, Point size)
        {
            sb.DrawRectangle(new Rectangle(location, size), Color.MediumVioletRed);
        }
    }

    internal class Grid 
    {
        public int NumOfLines { get; set; }
        public Graph<Point> graphPoint { get; set; }
       
        public Grid(int lines) 
        {
            NumOfLines = lines;
        }

        void DrawGrid(GraphicsDeviceManager graphics)
        {
         //   graphics.PreferredBackBufferWidth/NumOfLines    
        }

    }
}
