using System;
using System.Collections.Generic;
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

        public List<Texture2D> explosionEffect = new List<Texture2D>();

        public Texture2D queueSlot;
        public Texture2D queueSelect;

        public Board Board { get; set; }

        public Vector2 QueueRedLocation = Vector2.Zero;
        public Vector2 QueueBlueLocation = Vector2.Zero;

        public Piece StagedPiece { get; set; }
        public int StagedPieceColumn { get; set; }

        public enum GameState {
            SelectingFromQueue,
            SelectingColumn,
            Unknown
        };
        public GameState State { get; set; }

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

            explosionEffect.Add(Content.Load<Texture2D>("explode-1"));
            explosionEffect.Add(Content.Load<Texture2D>("explode-2"));
            explosionEffect.Add(Content.Load<Texture2D>("explode-3"));
            explosionEffect.Add(Content.Load<Texture2D>("explode-4"));
            explosionEffect.Add(Content.Load<Texture2D>("explode-5"));
            explosionEffect.Add(Content.Load<Texture2D>("explode-6"));

            Origin = new Vector2(queueSlot.Width * 2.5f, queueSlot.Height);

            QueueBlueLocation = new Vector2(-1.5f * queueSlot.Width, queueSlot.Height);
            QueueRedLocation = new Vector2(8.5f * queueSlot.Width, queueSlot.Height);

            Board = new Board();
            //Board.Scramble(); // TODO: This was a test.

            Board.ExplosionImages = explosionEffect;

            PieceImages = new Dictionary<PieceTypes, Texture2D>()
            {
                { PieceTypes.Bomb, pieceBomb },
                //{ PieceTypes.GoTwice, pieceTwice },
                { PieceTypes.Kitty, pieceKitty },
                { PieceTypes.NormalBlue, pieceBlue },
                { PieceTypes.NormalRed, pieceRed },
                //{ PieceTypes.PacMan, piecePacMan },
                { PieceTypes.Stone, pieceStone },
                //{ PieceTypes.SwapDown, pieceSwapDown },
                //{ PieceTypes.SwapLeft, pieceSwapLeft },
                //{ PieceTypes.SwapRight, pieceSwapRight },
                { PieceTypes.ToggleColors, pieceToggleColors },
            };

            StagedPiece = Piece.Empty;
            State = GameState.SelectingFromQueue;
        }

        public Dictionary<PieceTypes, Texture2D> PieceImages;

        GamePadState gamepad;

        public int SelectedQueueIndex;

		public override void Update(GameTime gameTime)
		{
            // TODO: Fix, the add 2nd controller
			gamepad = GamePadEx.GetState(Board.Player);

            if(State == GameState.SelectingFromQueue)
            {
                if (GamePadEx.WasJustPressed(Board.Player, Buttons.DPadUp))
                {
                    SelectedQueueIndex = Math.Max(0, SelectedQueueIndex - 1);
                }
                else if (GamePadEx.WasJustPressed(Board.Player, Buttons.DPadDown))
                {
                    SelectedQueueIndex = Math.Min(3, SelectedQueueIndex + 1);
                }

                if (GamePadEx.WasJustPressed(Board.Player, Buttons.A))
                {
                    //ScreenUtil.Show(new Credits(this.Parent));
                    // TODO: move selected piece to top for drop
                    StagedPiece = Board.Player == PlayerIndex.One ?
                        Board.BlueQueue[SelectedQueueIndex] :
                        Board.RedQueue[SelectedQueueIndex];
                    (Board.Player == PlayerIndex.One ? Board.BlueQueue : Board.RedQueue)[SelectedQueueIndex]
                        = Piece.Empty;
                    StagedPieceColumn = 3;
                    State = GameState.SelectingColumn;
                }

            }
            else if(State == GameState.SelectingColumn)
            {
                if (GamePadEx.WasJustPressed(Board.Player, Buttons.DPadLeft))
                {
                    StagedPieceColumn = Math.Max(0, StagedPieceColumn - 1);
                }
                else if (GamePadEx.WasJustPressed(Board.Player, Buttons.DPadRight))
                {
                    StagedPieceColumn = Math.Min(7, StagedPieceColumn + 1);
                }

                if(GamePadEx.WasJustPressed(Board.Player, Buttons.A))
                {
                    if(Board.Pieces[StagedPieceColumn,0].PieceType == PieceTypes.Empty)
                    {
                        Board.Pieces[StagedPieceColumn, 0] = StagedPiece;

                        switch (Board.Player)
                        {
                            case PlayerIndex.One:
                                Board.FillQueue(Board.BlueQueue, PieceTypes.NormalBlue);
                                break;
                            case PlayerIndex.Two:
                                Board.FillQueue(Board.RedQueue, PieceTypes.NormalRed);
                                break;
                        }

                        State = GameState.SelectingFromQueue;
                        StagedPiece = Piece.Empty;
                        Board.TogglePlayer();
                    }
                }
            }









            if (GamePadEx.WasJustPressed(Board.Player, Buttons.Back))
			{
				ScreenUtil.Show(new Title(this.Parent));
			}
            else
            {
                var isAnimating = false;
                if(Board.Animations != null && Board.Animations.Count > 0)
                {
                    foreach(var animation in Board.Animations)
                    {
                        if(animation.IsDone == false)
                        {
                            isAnimating = true;
                            break;
                        }
                    }
                }

                if(isAnimating)
                {
                    // DO NOTHING!
                }
                else if (Board.DoGravity((float)gameTime.ElapsedGameTime.TotalSeconds))
                {
                    // DO NOTHING!
                }
                else
                {
                    // TODO: scan for powerups
                    if (Board.ScanForPowerUps())
                    {

                    }
                    else
                    {
                        Board.RemoveExploded();
                    }

                    var matchRed = Board.ScanForMatches(Board.MatchOnRed);
                    var matchBlue = Board.ScanForMatches(Board.MatchOnBlue);

                    if (matchRed == PieceTypes.NormalRed && matchBlue == PieceTypes.NormalBlue)
                    {
                        // TIE GAME :/
                    }
                    else if (matchBlue == PieceTypes.NormalBlue)
                    {
                        // BLUE WINS!
                    }
                    else if(matchRed == PieceTypes.NormalRed)
                    {
                        // RED WINS!
                    }
                    else if(Board.IsFull)
                    {
                        // TIE GAME :/
                    }
                }
            }

            if (Board.Animations != null && Board.Animations.Count > 0)
            {
                foreach (var animation in Board.Animations)
                {
                    animation.Update(gameTime);
                }
            }
            base.Update(gameTime);
		}

        public Vector2 Origin = Vector2.Zero;

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (StagedPiece != null && StagedPiece.PieceType != PieceTypes.Empty)
            {
                var stagedLocation = new Vector2(StagedPieceColumn * tileSlot.Width, -tileSlot.Height);
                spriteBatch.Draw(PieceImages[StagedPiece.PieceType], Origin + stagedLocation, Color.White);
            }

            for (int x = 0; x < 8; x++)
            {
                var location = new Vector2(x * tileSlot.Width, 0);
                for (int y = 0; y < 8; y++)
                {
                    var piece = Board.Pieces[x, y];
                    Texture2D image = piece.PieceType == PieceTypes.Empty ? null : PieceImages[piece.PieceType];
                    switch(piece.PieceType)
                    {
                        case PieceTypes.Bomb: image = pieceBomb; break;
                        //case PieceTypes.GoTwice: image = pieceTwice; break;
                        case PieceTypes.Kitty: image = pieceKitty; break;
                        case PieceTypes.NormalBlue: image = pieceBlue; break;
                        case PieceTypes.NormalRed: image = pieceRed; break;
                        //case PieceTypes.PacMan: image = piecePacMan; break;
                        case PieceTypes.Stone: image = pieceStone; break;
                        //case PieceTypes.SwapDown: image = pieceSwapDown; break;
                        //case PieceTypes.SwapLeft: image = pieceSwapLeft; break;
                        //case PieceTypes.SwapRight: image = pieceSwapRight; break;
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
                }
            }

            if (Board.Animations != null && Board.Animations.Count > 0)
            {
                foreach (var animation in Board.Animations)
                {
                    var location = Vector2.Zero;
                    location.X = animation.Location.X * tileSlot.Width;
                    location.Y = animation.Location.Y * tileSlot.Height;
                    if (animation.CurrentFrame != null)
                        spriteBatch.Draw(animation.CurrentFrame, Origin + location, Color.White);
                }
            }


            var queueSlotHeight = new Vector2(0, queueSlot.Height);
            for (int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(queueSlot, Origin + QueueBlueLocation + i * queueSlotHeight, Color.White);
                spriteBatch.Draw(queueSlot, Origin + QueueRedLocation + i * queueSlotHeight, Color.White);

                if (Board.Player == PlayerIndex.One && i == SelectedQueueIndex)
                {
                    spriteBatch.Draw(queueSelect, Origin + QueueBlueLocation + i * queueSlotHeight, Color.White);
                }
                else if (Board.Player == PlayerIndex.Two && i == SelectedQueueIndex)
                {
                    spriteBatch.Draw(queueSelect, Origin + QueueRedLocation + i * queueSlotHeight, Color.White);
                }

                var piece = Board.BlueQueue[i];
                Texture2D image = piece.PieceType == PieceTypes.Empty ? null : PieceImages[piece.PieceType];
                if (image != null) { spriteBatch.Draw(image, Origin + QueueBlueLocation + i * queueSlotHeight, Color.White); }

                piece = Board.RedQueue[i];
                image = piece.PieceType == PieceTypes.Empty ? null : PieceImages[piece.PieceType];
                if (image != null) { spriteBatch.Draw(image, Origin + QueueRedLocation + i * queueSlotHeight, Color.White); }
            }
        }

    }
}

