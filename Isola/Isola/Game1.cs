using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;

namespace Isola
{
    public class Square 
    {
        public Square() 
        {
            _Transform = new Rectangle(0, 0, SquareSize, SquareSize);
            IsDestroyed = false;
        }

        public Square(int x, int y) 
        {
            _Transform = new Rectangle(x, y, SquareSize, SquareSize);
            IsDestroyed = false;
        }

        public Rectangle GetTransform() => _Transform;

        public static int SquareSize = 60;
        private Rectangle _Transform;
        public bool IsDestroyed;
    }

    public class Game1 : Game
    {
        private Square[,] _Field;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _WhiteTex;

        private bool hasGameBegun = false;
        private bool player1Wins;
        private bool player2Wins;
        private SpriteFont _font;
        private Rectangle _BeginTextTransform;
        public Game1()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _Field = new Square[6, 8];
            int xOffset = (Square.SquareSize + 2) * _Field.GetLength(1), yOffset = (Square.SquareSize + 2) * _Field.GetLength(0);
            
            xOffset = Window.ClientBounds.Width / 2 - xOffset / 2;
            yOffset = Window.ClientBounds.Height / 2 - yOffset / 2;

            for (int y = 0; y < _Field.GetLength(0); y++)
                for (int x = 0; x < _Field.GetLength(1); x++)
                    _Field[y, x] = new Square((x * (Square.SquareSize + 2)) + xOffset, (y * (Square.SquareSize + 2)) + yOffset);

            _WhiteTex = new Texture2D(GraphicsDevice, 1, 1);
            uint[] whiteTexturedata = new uint[] { 0xffffffff };
            _WhiteTex.SetData(whiteTexturedata);

            player1Wins = false;
            player2Wins = false;

            // TODO: Add your initialization logic 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial");

            Vector2 textSize = _font.MeasureString("Press space to continue");
            Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 50);
            _BeginTextTransform = new Rectangle(textPos.ToPoint(), textSize.ToPoint());
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                hasGameBegun = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                hasGameBegun = false;
            }

            base.Update(gameTime);
        }

        private void DrawField() 
        {
            for (int y = 0; y < _Field.GetLength(0); y++)
                for (int x = 0; x < _Field.GetLength(1); x++)
                    _spriteBatch.Draw(_WhiteTex, _Field[y, x].GetTransform(), !_Field[y, x].IsDestroyed ? Color.White : Color.Black);
        }

        private void DrawMainMenu() 
        {
            _spriteBatch.DrawString(_font, $"Press space to start playing", _BeginTextTransform.Location.ToVector2(), Color.White);
        }

        private void DrawEndScreen() 
        {
            Vector2 textSize = _font.MeasureString($"Player {(player1Wins ? 1 : 2)} has won");
            Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            _spriteBatch.DrawString(_font, $"Player {(player1Wins ? 1 : 2)} has won! Press R to restart", textPos, Color.White);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (!hasGameBegun)
            {
                DrawMainMenu();
            }
            else
            {
                DrawField();
                DrawEndScreen();
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
