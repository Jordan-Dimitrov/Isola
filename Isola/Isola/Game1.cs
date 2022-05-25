using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;
using System.IO;
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
        private Texture2D _tex;
        public int Player1Play = 0;
        public int Player2Play = 0;
        Rectangle player;
        Rectangle player2;
        int currentSquarePosX;
        int currentSquarePosY;
        Square currentSquare;

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

        private Rectangle _BeginTextTransform;
        private ButtonState currButtonState = ButtonState.Released, prevButtonState;

        private Texture2D _whitePlayerTex, _blackPlayerTex;
        public Game1()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            int playerSize = 60;
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
            player = new Rectangle(_Field[3,0].GetTransform().X, _Field[3, 0].GetTransform().Y, playerSize, playerSize);
            player1Wins = false;
            player2Wins = false;
            currentSquarePosX = 0;
            currentSquarePosY = 0;

            // TODO: Add your initialization logic 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tex = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            base.LoadContent();
            _font = Content.Load<SpriteFont>("Arial");

            _whitePlayerTex = Content.Load<Texture2D>("circleLight");
            _blackPlayerTex = Content.Load<Texture2D>("circle");
            // TODO: use this.Content to load your game content here
            Vector2 textSize = _font.MeasureString("Singleplayer");
            Vector2 textPos = new Vector2(Window.ClientBounds.Width / 2 - 370, 100);
            _BeginTextTransform1 = new Rectangle(textPos.ToPoint(), textSize.ToPoint());

            Vector2 textSize2 = _font.MeasureString("Multiplayer");
            Vector2 textPos2 = new Vector2(Window.ClientBounds.Width / 2 - 370, Window.ClientBounds.Height / 2);
            _BeginTextTransform2 = new Rectangle(textPos2.ToPoint(), textSize2.ToPoint());
        }

        protected override void Update(GameTime gameTime)
        {
            prevButtonState = currButtonState;
            currButtonState = Mouse.GetState().LeftButton;
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
                DrawField();
                if (Player1Play % 2 == 0)
                    PlayerAndMovement(_Field, _Field[4, 0].GetTransform().X, _Field[4, 0].GetTransform().Y);
                else
                    Destroy(currentSquare, _Field);

                _spriteBatch.Draw(_whitePlayerTex, player, Color.White);
                _spriteBatch.Draw(_blackPlayerTex, player2, Color.White);
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

        private List<Square> GetValidSquares(int indX, int indY)
        {
            List<Square> validSquares = new List<Square>();

            Square currSquare = _Field[indY, indX];
            byte canMoveUpRight = 0, canMoveDownRight = 0, canMoveUpLeft = 0, canMoveDownLeft = 0;

            if (indX + 1 < _Field.GetLength(1) && !_Field[indY, indX].IsDestroyed)
            {
                validSquares.Add(_Field[indY, indX + 1]);
                canMoveUpRight++;
                canMoveDownRight++;
            }


            if (indX - 1 >= 0 && !_Field[indY, indX].IsDestroyed)
            {
                validSquares.Add(_Field[indY, indX - 1]);
                canMoveUpLeft++;
                canMoveDownLeft++;
            }

            if (indY + 1 < _Field.GetLength(0) && !_Field[indY, indX].IsDestroyed)
            {
                validSquares.Add(_Field[indY + 1, indX]);
                canMoveDownLeft++;
                canMoveDownRight++;
            }

            if (indY - 1 >= 0 && !_Field[indY, indX].IsDestroyed)
            {
                validSquares.Add(_Field[indY - 1, indX]);
                canMoveUpLeft++;
                canMoveUpRight++;
            }

            if (canMoveUpLeft == 2)
            {
                validSquares.Add(_Field[indY - 1, indX - 1]);
            }
            if (canMoveUpRight == 2)
            {
                validSquares.Add(_Field[indY - 1, indX + 1]);
            }
            if (canMoveDownLeft == 2)
            {
                validSquares.Add(_Field[indY + 1, indX - 1]);
            }
            if (canMoveDownRight == 2)
            {
                validSquares.Add(_Field[indY + 1, indX + 1]);
            }



            return validSquares;
        }

        private void PlayerAndMovement(Square[,] field, int startingX, int startingY)
        {
            Point mousePos = Mouse.GetState().Position;
            var IsPressed = currButtonState == ButtonState.Pressed && prevButtonState == ButtonState.Released;

            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 2, 2);

            currentSquare = field.Cast<Square>().ToArray().First(x => x.GetTransform().Intersects(player));

            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int i = 0; i < field.GetLength(1); i++)
                {
                    if (field[y, i].GetTransform().X == currentSquare.GetTransform().X &&
                        field[y, i].GetTransform().Y == currentSquare.GetTransform().Y)
                    {
                        currentSquarePosX = i;
                        currentSquarePosY = y;
                    }
                }
            }
            List<Square> avalibleSquares = GetValidSquares(currentSquarePosX, currentSquarePosY);

            foreach (var CurrentSquare in avalibleSquares)
            {
                if (mouseRect.Intersects(CurrentSquare.GetTransform()) && IsPressed)
                {
                    player.X = CurrentSquare.GetTransform().X;
                    player.Y = CurrentSquare.GetTransform().Y;
                    Player1Play++;
                }
            }
        }

        public void Destroy(Square CurrentSquare, Square[,] field)
        {
            Point mousePos = Mouse.GetState().Position;
            Rectangle mouseRect = new Rectangle(mousePos.X, mousePos.Y, 2, 2);
            var IsPressed = currButtonState == ButtonState.Pressed && prevButtonState == ButtonState.Released;

            foreach (var SquareToDestroy in field)
            {
                if (mouseRect.Intersects(SquareToDestroy.GetTransform()) && IsPressed &&
                    (!SquareToDestroy.GetTransform().Intersects(CurrentSquare.GetTransform())))
                {
                    SquareToDestroy.IsDestroyed = true;
                    Player1Play++;
                }
            }
        }
    }


        
}

