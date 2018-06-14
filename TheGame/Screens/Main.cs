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

		public override void Showing()
		{
			this.BackgroundColor = Color.CornflowerBlue;

            tileSlot = this.Content.Load<Texture2D>("slot");
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

			base.Update(gameTime);
		}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int x = 0; x < 8; x++)
            {
                var location = new Vector2(x * tileSlot.Width, 0);
                for (int y = 0; y < 8; y++)
                {
                    location.Y = y * tileSlot.Height;
                    spriteBatch.Draw(tileSlot, location, Color.White);
                }
            }
        }
    }
}

