using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootlegSimCity
{
    public class SelectScreen
    {
        Point start;
        int boxSpacing;
        int numOfSquares;
        int boxSize;
        int selectedIndex = 1;
        
        List<ISquare> drawSquares;
        public SelectScreen(int x) 
        {
            start = new Point(x, 0);
            boxSize = 100;
            numOfSquares = 5;
            boxSpacing = 20;

            drawSquares = new List<ISquare>
            {
                new EmptySquare(0, 0),
                new RoadSquare(0, 0),
                new HouseSquare(0, 0),
                new CarSquare(0, 0),
                new EmptySquare(0, 0)
            };
        }

        public void DrawSelectScreen(SpriteBatch sb, GraphicsDeviceManager graphics)
        {
            sb.FillRectangle(new Rectangle(start.X, start.Y,
                graphics.PreferredBackBufferWidth - start.X, graphics.PreferredBackBufferHeight), Color.MistyRose);
            int middle = start.X + (graphics.PreferredBackBufferWidth - start.X) / 2 - boxSize / 2;
            for (int i = 0; i < drawSquares.Count; i++)
            {
                Point drawLocation = new Point(middle, 200 + i * (boxSize + boxSpacing));
                drawSquares[i].Location = drawLocation;
                drawSquares[i].Draw(sb, new Point(boxSize, boxSize));
                Color outlineColor = Color.Black;
                if (i== selectedIndex)
                     outlineColor = Color.Green;
                sb.DrawRectangle(new Rectangle(drawLocation, new Point(boxSize, boxSize)), outlineColor, 3f);
            }

        }

        public ISquare GetSelectedSquare()
        {
            return drawSquares[selectedIndex];
        }

        public void SetSelectedIndex(int index)
        {
            if (index >= 0 && index < drawSquares.Count)
                selectedIndex = index;
        }
    }
}
