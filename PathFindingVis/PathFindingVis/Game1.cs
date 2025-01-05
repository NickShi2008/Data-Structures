using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Security.Cryptography.X509Certificates;


namespace PathFindingVis
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MouseState ms;
        private Grid<Point> grid;
        private MouseState lastMouseState = new MouseState();
        private Rectangle buttonRectangle;
        SpriteFont font;
        private Graph<Point> graph;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            /////awjwa0ifasdfajf
            //font = Content.Load<SpriteFont>("Ubuntu32.spritefont");
            double screenWidth = graphics.PreferredBackBufferWidth;
            double screenHeight = graphics.PreferredBackBufferHeight;
            buttonRectangle = new Rectangle((int) (screenWidth * 0.33) ,(int) (screenHeight * 0.45), (int) (
                screenWidth * 0.35), (int) (screenHeight * 0.1));

            graph = new Graph<Point>();
            grid = new Grid<Point>(20, graphics);
            grid.ConnectGrid();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                grid.ConnectGrid();
                graph.ASTAR(grid.FindStart(), grid.FindEnd());
            }

            ms = Mouse.GetState();

            if(ms.LeftButton == ButtonState.Pressed)
            {
                int x = ms.X;
                int y = ms.Y;
            }


            
            if (ms.LeftButton == ButtonState.Pressed && (lastMouseState.LeftButton == ButtonState.Released || grid.EnableDrag)
                && ms.X > 0 && ms.X < graphics.PreferredBackBufferWidth && ms.Y > 0 && ms.Y < graphics.PreferredBackBufferHeight)
            {
                if (!grid.hasStored)
                {
                    grid.StoreMouseClick(ms.X, ms.Y);
                }
                else
                {
                    grid.PlaceSquare(ms.X, ms.Y);
                }
                

            }
            // TODO: Add your update logic here
            lastMouseState = ms;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            grid.DrawGrid(spriteBatch);

            
            // spriteBatch.FillRectangle(buttonRectangle, Color.MediumSlateBlue);
            

            spriteBatch.End();
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
            
        }
    }
}
