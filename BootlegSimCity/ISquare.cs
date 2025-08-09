using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;

namespace BootlegSimCity
{
    public interface ISquare
    {
        Point Location { get; set; }
        Color Hue { get; set; }
        void Draw(SpriteBatch sb, Point size);
    }

    public class EmptySquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }

        public EmptySquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.AntiqueWhite;
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }

    public class RoadSquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }
        public RoadType roadType { get; set; }

        public RoadSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Black;
            roadType = RoadType.Horizontal;
        }

        public RoadSquare(int x, int y, RoadType roadType)
        {
            Location = new Point(x, y);
            Hue = Color.Black;
            this.roadType = roadType;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);

        }
    }

    public class SeperationSquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }

        public SeperationSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Yellow;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }

    public class HouseSquare : ISquare
    {
        public Point Location { get; set; }

        public Color Hue { get; set; }
        public HouseSquare(int x, int y, bool isPreview = false)
        {
            Location = new Point(x, y);
            Hue = Color.SandyBrown;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }

    public class CarSquare : ISquare
    {
        public Point Location { get; set; }

        public Point NextSquare { get; set; }
        public Color Hue { get; set; }

        private int Center = 4;

        public PriorityQueue<Vertex<Point>, int> Path { get; set; } = new PriorityQueue<Vertex<Point>, int>();

        public CarSquare(int x, int y)
        {
            Location = new Point(x + Center, y + Center);
            Hue = Color.Red;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }

        public void DrawTransparent(SpriteBatch sb, Point size)
        {
            Color adjustedColor = new Color(Hue.R, Hue.G, Hue.B, Hue.A - 100);
            sb.FillRectangle(new Rectangle(Location, size), adjustedColor);
        }
    }
}
