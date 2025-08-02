using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RoadType RoadType { get; set; }

        public RoadSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Black;
            RoadType = RoadType.Horizontal; // default
        }

        public RoadSquare(int x, int y, RoadType roadType)
        {
            Location = new Point(x, y);
            Hue = Color.Black;
            RoadType = roadType;
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

        List<CarSquare> CarSquares { get; set; }
        public Color Hue { get; set; }
        public HouseSquare(int x, int y, bool isPreview = false)
        {
            Location = new Point(x, y);
            CarSquares = new List<CarSquare>();
            Hue = Color.SandyBrown;
        }

        public void AddCar(CarSquare car, List<HouseSquare> houses)
        {
            CarSquares.Add(car);
            car.AssignDestination(houses);
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Hue);
        }
    }

    public class CarSquare : ISquare
    {
        public Point Location { get; set; }

        public Point Destination { get; set; }
        public Color Hue { get; set; }

       

        public CarSquare(int x, int y)
        {
            Location = new Point(x, y);
            Hue = Color.Red;
        }

        public void AssignDestination(List<HouseSquare> houses)
        {
            Random random = new Random();
            int randIndex = random.Next(0, houses.Count);
            Destination = houses[randIndex].Location;
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
