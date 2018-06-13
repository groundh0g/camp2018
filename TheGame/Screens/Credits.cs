using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MoreOnCode.Lib.Graphics;
using MoreOnCode.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

namespace TheGame
{
	public class Credits : GameScreen
	{
		public Credits(Game parent) : base(parent) { }

		public override void Hiding()
		{
		}

		public override void Showing()
		{
			this.BackgroundColor = Color.Red;
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
				ScreenUtil.Show(new Main(this.Parent));
			}

			base.Update(gameTime);
		}
	}
}

