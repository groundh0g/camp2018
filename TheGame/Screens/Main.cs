using System;
using Microsoft.Xna.Framework;
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

		public override void Showing()
		{
			this.BackgroundColor = Color.CornflowerBlue;
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
	}
}

