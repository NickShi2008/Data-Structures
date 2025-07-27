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
        
        Dictionary<ISquare, Color> drawSquares;
        public SelectScreen(int x) 
        {
            start = new Point(x, 0);
            boxSize = 100;
            numOfSquares = 5;
        }

        public void DrawSelectScreen(SpriteBatch sb, GraphicsDeviceManager graphics)
        {
            sb.FillRectangle(new Rectangle(start.X, start.Y,
                graphics.PreferredBackBufferWidth - start.X, graphics.PreferredBackBufferHeight), Color.MistyRose);
            int middle = start.X + (graphics.PreferredBackBufferWidth - start.X) / 2 - boxSize / 2;
            for (int i = 0; i < numOfSquares; i++)
            {
                sb.DrawRectangle(new Rectangle(middle, 200 + (int)( boxSize * i *  1.5),
                    boxSize, boxSize), Color.Black);
            }
            
        }
    }
}
