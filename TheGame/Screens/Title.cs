using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoreOnCode.Lib.Graphics;
using MoreOnCode.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

namespace TheGame
{
	public class Title : GameScreen
	{
		public Title(Game parent) : base(parent) { }

		public override void Hiding()
		{
		}

        public Texture2D title;
        public Vector2 location = Vector2.Zero;

		public override void Showing()
		{
			this.BackgroundColor = Color.White;

            title = Content.Load<Texture2D>("title");
            location.X = GraphicsDevice.Viewport.Width / 2 - title.Width / 2;
            //location.Y = GraphicsDevice.Viewport.Height / 2 - title.Height / 2;
            location.Y = 1280 / 2 - title.Height / 2;
        }

        GamePadState gamepad;

		public override void Update(GameTime gameTime)
		{
			gamepad = GamePadEx.GetState(PlayerIndex.One);

			if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.A))
			{
				ScreenUtil.Show(new Main(this.Parent));
			}
			else if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.Back))
			{
				ScreenUtil.Show(new Splash(this.Parent));
			}

			base.Update(gameTime);
		}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(title, location, Color.White);
        }
    }
}

