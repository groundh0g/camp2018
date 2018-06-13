using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;

namespace MoreOnCode.Lib.Graphics
{
	public abstract class GameScreen
	{
		public Game Parent { get; private set; }

		public ContentManager Content { get { return Parent.Content; } }

		public GraphicsDevice GraphicsDevice { get { return Parent.GraphicsDevice; } }

		public string Name { get { return this.GetType().AssemblyQualifiedName; } }

		public Color? BackgroundColor { 
			get;
			protected set;
		}

		public bool ExitOnBack { get; set; }

		protected GameScreen (Game parent) {
			this.Parent = parent;
			this.BackgroundColor = Color.CornflowerBlue;
		}

		public abstract void Showing ();
		public abstract void Hiding ();

		public virtual void Update (GameTime gameTime) {
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (ExitOnBack && GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.Back)) { 
				Exit (); 
			}
		}

		public virtual void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			GraphicsDevice.Clear (
				BackgroundColor.HasValue ? BackgroundColor.Value : Color.CornflowerBlue
			);
		}

		public void Exit() {
			// Exit() is obsolete on iOS
			#if !__IOS__
			Parent.Exit ();
			#endif
		}
	}
}

