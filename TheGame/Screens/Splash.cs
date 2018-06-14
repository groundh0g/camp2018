using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoreOnCode.Lib.Graphics;
using MoreOnCode.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

namespace TheGame
{
	public class Splash : GameScreen
	{
		public Splash(Game parent) : base(parent) { }

		public override void Hiding()
		{
		}

        public Texture2D splash;
        public Vector2 location = Vector2.Zero;

		public override void Showing()
		{
			this.BackgroundColor = Color.White;

            splash = Content.Load<Texture2D>("splash");
            location.X = GraphicsDevice.Viewport.Width / 2 - splash.Width / 2;
            location.Y = GraphicsDevice.Viewport.Height / 2 - splash.Height / 2;
        }

        GamePadState gamepad;

		public override void Update(GameTime gameTime)
		{
			gamepad = GamePadEx.GetState(PlayerIndex.One);

			if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.A))
			{
				ScreenUtil.Show(new Title(this.Parent));
			}

			base.Update(gameTime);
		}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(splash, location, Color.White);
        }
    }
}

