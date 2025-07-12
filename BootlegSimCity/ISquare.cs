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
        void Draw(SpriteBatch sb, Point size);
    }

    public class EmptySquare : ISquare
    {
        public Point Location { get; set; }

        public EmptySquare(int x, int y)
        {
            Location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Color.AntiqueWhite);
        }
    }

    public class RoadSquare : ISquare
    {
        public Point Location { get; set; }

        public RoadSquare(int x, int y)
        {
            Location = new Point(x, y);
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Color.Black);
        }
    }
    public class HouseSquare : ISquare
    {
        public Point Location { get; set; }

        List<CarSquare> CarSquares { get; set; }
        public HouseSquare(int x, int y)
        {
            Location = new Point(x, y);
            CarSquares = new List<CarSquare>();
        }

        public void AddCar(CarSquare car, List<HouseSquare> houses)
        {
            CarSquares.Add(car);
            car.AssignDestination(houses);
        }

        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Color.SlateGray);
        }
    }

    public class CarSquare : ISquare
    {
        public Point Location { get; set; }

        public Point Destination { get; set; }
        public CarSquare(int x, int y)
        {
            Location = new Point(x, y);
        }

        public void AssignDestination(List<HouseSquare> houses)
        {
            Random random = new Random();
            int randIndex = random.Next(0, houses.Count);
            Destination = houses[randIndex].Location;
        }
        public void Draw(SpriteBatch sb, Point size)
        {
            sb.FillRectangle(new Rectangle(Location, size), Color.Black);
        }
    }
}
