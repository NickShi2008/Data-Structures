using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BootlegSimCity
{
    public class SelectScreen
    {
        Point start;
        int boxSpacing;
        int boxSize;
        int selectedIndex = 1;
        int screenHeight;
        
        List<ISquare> drawSquares;
        HashSet<Rectangle> squareBoxes;
        ISquare seperationSquare;
        public SelectScreen(int x) 
        {
            start = new Point(x, 0);
            boxSize = 80;
            boxSpacing = 60;
    
            drawSquares = new List<ISquare>
            {
                new EmptySquare(0, 0),
                new RoadSquare(0, 0),
                new HouseSquare(0, 0),
                new CarSquare(0, 0),
            };
            seperationSquare = new SeperationSquare(0, 0);
            squareBoxes = new HashSet<Rectangle>();
        }

        public void DrawSelectScreen(SpriteBatch sb, GraphicsDeviceManager graphics, SpriteFont sf)
        {
            screenHeight = graphics.PreferredBackBufferHeight;
            sb.FillRectangle(new Rectangle(start.X, start.Y,
                graphics.PreferredBackBufferWidth - start.X, screenHeight), Color.MistyRose);
            int middle = start.X + (graphics.PreferredBackBufferWidth - start.X) / 2 - boxSize / 2;
            for (int i = 0; i < drawSquares.Count; i++)
            {
                Point drawLocation = new Point(middle, 200 + i * (boxSize + boxSpacing));
                drawSquares[i].Location = drawLocation;
                drawSquares[i].Draw(sb, new Point(boxSize, boxSize));
                Color outlineColor = Color.Black;

                if (i== selectedIndex)
                     outlineColor = Color.Green;
                Rectangle squareBox = new Rectangle(drawLocation, new Point(boxSize, boxSize));
                sb.DrawRectangle(squareBox, outlineColor, 3f);
                squareBoxes.Add(squareBox);


                string squareLabel = $"{drawSquares[i].GetType()}";
                squareLabel = squareLabel[15..^6];
                
                Vector2 textSize = sf.MeasureString(squareLabel);
                Vector2 textPosition = new Vector2(
                    (drawLocation.X + (boxSize - textSize.X)/2),
                    (drawLocation.Y + boxSize + 20 - textSize.Y)
                );
                sb.DrawString(sf, squareLabel, textPosition, Color.Black);
            }
        }

        public ISquare GetCurrentSquare()
        {
            if (selectedIndex == 1) return seperationSquare;
            return drawSquares[selectedIndex];
        }

        public void SetSelectedIndex(int index)
        {
            if (index >= 0 && index < drawSquares.Count)
                selectedIndex = index;
        }

        public void FindSelectedSquare(Point point)
        {
            Rectangle cursorBox = new Rectangle(point, new Point(1, 1));
            
            int count = 0;
            foreach(Rectangle squareBox in squareBoxes)
            {
                if (cursorBox.Intersects(squareBox))
                {
                    SetSelectedIndex(count);
                    break;
                }
                count++;

            }
            
        }
    }
}
