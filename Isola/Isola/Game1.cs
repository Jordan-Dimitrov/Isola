using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Isola
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool _gameStatus;
        private bool begins;
        private bool Player1Wins;
        private bool Player2Wins;
        private SpriteFont _font;
        private string gameState;
        public Game1()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            begins = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                begins = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                begins = true;

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            if (begins==true)
            {
                Vector2 textSize = _font.MeasureString("Press space to continue");
                Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                _spriteBatch.DrawString(_font, $"Press space to continue", textPos, Color.White);
            }
            else
            {
                    if (Player1Wins == true)
                    {
                        Vector2 textSize = _font.MeasureString("Player 1 has won");
                        Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                        _spriteBatch.DrawString(_font, $"Player 1 has won! Press R to restart", textPos, Color.White);
                    }
                    else if (Player2Wins == true)
                    {
                        Vector2 textSize = _font.MeasureString("Player 2 has won");
                        Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                        _spriteBatch.DrawString(_font, $"Player 2 has won! Press R to restart", textPos, Color.White);
                    }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
