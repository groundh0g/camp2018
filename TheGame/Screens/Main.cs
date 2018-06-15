using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoreOnCode.Lib.Graphics;
using MoreOnCode.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

namespace TheGame
{
	public class Main : GameScreen
	{
		public Main(Game parent) : base(parent) { }

		public override void Hiding()
		{
		}

        public Texture2D tileSlot;
        public Texture2D pieceRed;
        public Texture2D pieceBlue;
        public Texture2D pieceBomb;
        public Texture2D pieceKitty;
        public Texture2D pieceStone;
        public Texture2D pieceSwapDown;
        public Texture2D pieceSwapLeft;
        public Texture2D pieceSwapRight;
        public Texture2D pieceTwice;
        public Texture2D piecePacMan;
        public Texture2D pieceToggleColors;

        public Texture2D pieceCheckMark;
        public Texture2D hand;

        public Texture2D queueSlot;
        public Texture2D queueSelect;

        public Board Board { get; set; }

        public override void Showing()
		{
			this.BackgroundColor = Color.CornflowerBlue;

            tileSlot = this.Content.Load<Texture2D>("slot");
            queueSlot = this.Content.Load<Texture2D>("queue-slot");
            queueSelect = this.Content.Load<Texture2D>("queue-select");
            pieceRed = this.Content.Load<Texture2D>("piece-red");
            pieceBlue = this.Content.Load<Texture2D>("piece-blue");
            pieceBomb = this.Content.Load<Texture2D>("piece-bomb");
            pieceKitty = this.Content.Load<Texture2D>("piece-cat");
            pieceStone = this.Content.Load<Texture2D>("piece-stone");
            pieceSwapDown = this.Content.Load<Texture2D>("piece-swap-down");
            pieceSwapLeft = this.Content.Load<Texture2D>("piece-swap-left");
            pieceSwapRight = this.Content.Load<Texture2D>("piece-swap-right");
            pieceTwice = this.Content.Load<Texture2D>("piece-twice");
            piecePacMan = this.Content.Load<Texture2D>("piece-pacman");
            pieceToggleColors = this.Content.Load<Texture2D>("piece-swap-colors");
            pieceCheckMark = this.Content.Load<Texture2D>("piece-check");
            hand = this.Content.Load<Texture2D>("hand-top");

            Origin = new Vector2(queueSlot.Width * 2.5f, queueSlot.Height);

            Board = new Board();
            Board.Scramble(); // TODO: This was a test.
    }

    GamePadState gamepad;

		public override void Update(GameTime gameTime)
		{
			gamepad = GamePadEx.GetState(PlayerIndex.One);

			if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.A))
			{
				ScreenUtil.Show(new Credits(this.Parent));
			}
			else if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.Back))
			{
				ScreenUtil.Show(new Title(this.Parent));
			}
            else
            {
                if (Board.DoGravity((float)gameTime.ElapsedGameTime.TotalSeconds))
                {
                    // DO NOTHING!
                }
                else
                {
                    var matched = Board.ScanForMatches();
                    if (matched == PieceTypes.NormalBlue)
                    {
                        // BLUE WINS!
                    }
                    else if(matched == PieceTypes.NormalRed)
                    {
                        // RED WINS!
                    }
                }
            }

			base.Update(gameTime);
		}

        public Vector2 Origin = Vector2.Zero;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int x = 0; x < 8; x++)
            {
                var location = new Vector2(x * tileSlot.Width, 0);
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.Pieces[x, y];
                    Texture2D image = null;
                    switch(piece.PieceType)
                    {
                        case PieceTypes.Bomb: image = pieceBomb; break;
                        case PieceTypes.GoTwice: image = pieceTwice; break;
                        case PieceTypes.Kitty: image = pieceKitty; break;
                        case PieceTypes.NormalBlue: image = pieceBlue; break;
                        case PieceTypes.NormalRed: image = pieceRed; break;
                        case PieceTypes.PacMan: image = piecePacMan; break;
                        case PieceTypes.Stone: image = pieceStone; break;
                        case PieceTypes.SwapDown: image = pieceSwapDown; break;
                        case PieceTypes.SwapLeft: image = pieceSwapLeft; break;
                        case PieceTypes.SwapRight: image = pieceSwapRight; break;
                        case PieceTypes.ToggleColors: image = pieceToggleColors; break;
                    }

                    location.Y = y * tileSlot.Height;
                    if(image != null)
                    {
                        spriteBatch.Draw(image, Origin + location - piece.Delta, Color.White);
                        if (piece.IsChecked)
                        {
                            spriteBatch.Draw(pieceCheckMark, Origin + location - piece.Delta, Color.White);
                        }
                    }
                }
            }

            for (int x = 0; x < 8; x++)
            {
                var location = new Vector2(x * tileSlot.Width, 0);
                for (int y = 0; y < 8; y++)
                {
                    location.Y = y * tileSlot.Height;
                    spriteBatch.Draw(tileSlot, Origin + location, Color.White);

                    for (int i = 1; i < 5; i++)
                    {
                        spriteBatch.Draw(queueSlot, Origin + new Vector2(-1.5f * queueSlot.Width, i * queueSlot.Height), Color.White);
                        spriteBatch.Draw(queueSlot, Origin + new Vector2(8.5f * queueSlot.Width, i * queueSlot.Height), Color.White);
                    }
                }
            }

        }
    }
}

