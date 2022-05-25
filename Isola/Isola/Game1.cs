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

        private bool hasGameBegun = true;
        private bool player1Wins;
        private bool player2Wins;
        private SpriteFont _font;
        private Rectangle _BeginTextTransform1, _BeginTextTransform2;
        private enum PlayMode 
        {
            None = 0,
            SinglePlayer,
            MultiPlayer
        }

        PlayMode _playMode;

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

            Vector2 textSize = _font.MeasureString("Singleplayer");
            Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2 - 370, 100);
            _BeginTextTransform1 = new Rectangle(textPos.ToPoint(), textSize.ToPoint());

            Vector2 textSize2 = _font.MeasureString("Multiplayer");
            Vector2 textPos2 = new Vector2(Window.ClientBounds.Width / 2 - 370, Window.ClientBounds.Height / 2);
            _BeginTextTransform2 = new Rectangle(textPos2.ToPoint(), textSize2.ToPoint());
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                hasGameBegun = true;

            }
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseRect = new Rectangle(mousePos, new Point(2, 2));
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (mouseRect.Intersects(_BeginTextTransform1))
                {
                    hasGameBegun = false;
                    _playMode = PlayMode.SinglePlayer;

                }
                else if (mouseRect.Intersects(_BeginTextTransform2))
                {
                    hasGameBegun = false;
                    _playMode = PlayMode.MultiPlayer;
                }
            }

            base.Update(gameTime);
        }

        private void DrawField()
        {
            for (int y = 0; y < _Field.GetLength(0); y++)
                for (int x = 0; x < _Field.GetLength(1); x++)
                    _spriteBatch.Draw(_WhiteTex, _Field[y, x].GetTransform(), !_Field[y, x].IsDestroyed ? Color.White : Color.Black);
        }
        protected override void Draw(GameTime gameTime)
        {
            player2Wins = true;
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            if (hasGameBegun == true)
            {
                StartScreen();
            }
            else
            {
                //DrawField();
                EndScreen();
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void StartScreen()
        {

            _spriteBatch.DrawString(_font, $"Singleplayer", _BeginTextTransform1.Location.ToVector2(), Color.White);


            _spriteBatch.DrawString(_font, $"Multiplayer", _BeginTextTransform2.Location.ToVector2(), Color.White);
        }
        public void EndScreen()
        {
            if (player1Wins || player2Wins) 
            {
                string player = player1Wins ? "Player 1" : "Player 2";
                if (player2Wins && _playMode == PlayMode.SinglePlayer)
                    player = "Computer";

                Vector2 textSize = _font.MeasureString($"{player} has won");
                Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2 - textSize.X / 2, Window.ClientBounds.Height / 2 - textSize.Y / 2);
                _spriteBatch.DrawString(_font, $"{player} has won! Press R to restart", textPos, Color.White);
            }
        }
    }

}
