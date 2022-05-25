﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;

namespace Isola
{
    class Square 
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


        private bool _gameStatus;
        private bool begins = true;
        private bool Player1Wins;
        private bool Player2Wins;
        private SpriteFont _font;
        private string gameState;
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
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            for (int y = 0; y < _Field.GetLength(0); y++)
                for (int x = 0; x < _Field.GetLength(1); x++)
                    _spriteBatch.Draw(_WhiteTex, _Field[y, x].GetTransform(), !_Field[y, x].IsDestroyed ? Color.White : Color.Black);

            if (begins==true)
            {
                _spriteBatch.DrawString(_font, $"Press space to start playing", _BeginTextTransform.Location.ToVector2(), Color.White);
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
