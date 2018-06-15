#region File Description
//-----------------------------------------------------------------------------
// PlatformerGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using MoreOnCode.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

using MoreOnCode.Xna.Framework;

namespace TheGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
            //            graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;// 1500;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;// 1024;
            graphics.ApplyChanges();

            //var scale = (float)graphics.PreferredBackBufferHeight / 1280.0f;
            //ScreenUtil.TransformationMatrix = Matrix.CreateScale(scale);
            //var padding = ((float)graphics.PreferredBackBufferWidth - (float)graphics.PreferredBackBufferWidth * scale) / 2.0f;
            //ScreenUtil.TransformationMatrix *= Matrix.CreateTranslation(padding, 0, 0);

            var scale = (float)graphics.PreferredBackBufferHeight / 1280.0f;
            var padding = ((float)graphics.PreferredBackBufferWidth - (float)graphics.PreferredBackBufferWidth * scale) / 2.0f;
            ScreenUtil.TransformationMatrix = Matrix.CreateTranslation(padding, 0, 0) * Matrix.CreateScale(scale);

            //            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			ScreenUtil.Show(new Splash(this));
            //GamePadEx.KeyboardPlayerIndex = PlayerIndex.One;
            GamePadEx.KeyboardPlayerIndexEx = PlayerIndexEx.Auto;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			//TODO: use this.Content to load your game content here
			if (ScreenUtil.CurrentScreen != null)
			{
				ScreenUtil.CurrentScreen.Showing();
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			// TODO: Add your update logic here
			ScreenUtil.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//TODO: Add your drawing code here
			spriteBatch.Begin(transformMatrix: ScreenUtil.TransformationMatrix);
			ScreenUtil.Draw(gameTime, spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}