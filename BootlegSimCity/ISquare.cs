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

    public class WaterSquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }
        public RoadType roadType { get; set; }

        public WaterSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.SkyBlue;
            roadType = RoadType.Horizontal;
        }

        public WaterSquare(int x, int y, RoadType roadType)
        {
            Location = new Point(x, y);
            Hue = Color.SkyBlue;
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
        public Color Hue { get; set; }
        private int Center = 4;
        public Point LastDirection { get; set; } = new Point(0, 0);

        public PriorityQueue<Vertex<Point>, int> Path { get; set; } = new PriorityQueue<Vertex<Point>, int>();

        public CarSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Red;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            // Calculate offset based on direction to simulate right-side driving
            Point offset = new Point(Center, Center);

            // If moving horizontally, offset vertically (stay on right side)
            if (LastDirection.X != 0)
            {
                // Moving right: offset down (right side in top-down view)
                // Moving left: offset up (right side in top-down view)
                offset.Y = LastDirection.X > 0 ? Center + 3 : Center - 3;
            }
            // If moving vertically, offset horizontally
            else if (LastDirection.Y != 0)
            {
                // Moving down: offset right
                // Moving up: offset left
                offset.X = LastDirection.Y > 0 ? Center + 3 : Center - 3;
            }

            sb.FillRectangle(new Rectangle(Location + offset, size), Hue);
        }
    }

    public class BoatSquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }
        private int Center = 4;

        public PriorityQueue<Vertex<Point>, int> Path { get; set; } = new PriorityQueue<Vertex<Point>, int>();

        public BoatSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.DarkSlateGray;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location + new Point(Center, Center), size), Hue);
        }
    }

    public class RedSquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }

        public PriorityQueue<Vertex<Point>, int> Path { get; set; } = new PriorityQueue<Vertex<Point>, int>();

        public RedSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Red;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }

    public class GraySquare : ISquare
    {
        public Point Location { get; set; }
        public Color Hue { get; set; }

        public PriorityQueue<Vertex<Point>, int> Path { get; set; } = new PriorityQueue<Vertex<Point>, int>();

        public GraySquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.DarkSlateGray;
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }
}