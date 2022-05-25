using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;


namespace Isola
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tex;

        public Game1()
        {
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
            _tex = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            base.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void PlayerAndMovement( Square[,] field, int startingX, int startingY)
        {
           Rectangle player = new Rectangle();
           
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 2, 2);
            List<Square> avalibleSquares = new List<Square>();
            Square currentSquare = new Square((avalibleSquares.First(x => x.rcc.Intersects(player)).rcc), false);
            int currentSquarePosX = 0;
            int currentSquarePosY = 0;

            for (int i = 0; i < field.Length; i++)
            {
                int y = 0;
                if (i == field.GetLength(1))
                {
                    i = 0;
                    y++;
                }
                if (field[i, y] == currentSquare)
                {
                    currentSquarePosX = i;
                    currentSquarePosY = y;
                }
            }

            foreach (var square in field)
            {
                if (square.IsDestroyed == false && (!field[currentSquarePosX + 1, currentSquarePosY].IsDestroyed || !field[currentSquarePosX - 1, currentSquarePosY].IsDestroyed  || !field[currentSquarePosX, currentSquarePosY + 1].IsDestroyed || !field[currentSquarePosX , currentSquarePosY - 1].IsDestroyed || !field[currentSquarePosX + 1, currentSquarePosY + 1].IsDestroyed || !field[currentSquarePosX + 1, currentSquarePosY - 1].IsDestroyed || !field[currentSquarePosX - 1, currentSquarePosY + 1].IsDestroyed || !field[currentSquarePosX - 1, currentSquarePosY - 1].IsDestroyed))
                {
                    avalibleSquares.Add(square);
                }    
            }
            foreach (var CurrentSquare in avalibleSquares)
            {
                if (mouseRect.Intersects(CurrentSquare.rcc))
                {
                    player.X = CurrentSquare.rcc.X;
                    player.Y = CurrentSquare.rcc.Y;
                }
            }
        }

        public void Destroy(Square CurrentSquare)
        {
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 2, 2);

        }
    }
   public class Square
    {
        public Rectangle rcc = new Rectangle();
        public bool IsDestroyed;
        public Square(Rectangle Rcc, bool ISDestroyed) 
        {
            rcc = Rcc;
            IsDestroyed = ISDestroyed;
        }
    }

   
}
