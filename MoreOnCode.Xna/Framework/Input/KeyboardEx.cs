using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoreOnCode.Xna.Framework.Input
{
	public static class KeyboardEx
	{
		public static KeyboardState GetState() {
			return Keyboard.GetState ();
		}

		public static KeyboardState GetState(PlayerIndex playerIndex) {
			return Keyboard.GetState (playerIndex);
		}

		#region "Events"

		public static long CurrentTicks { get; private set; }

		private static readonly List<Keys> CurrentState = new List<Keys>();
		private static readonly List<Keys> LastState = new List<Keys>();

		public static void Update(GameTime gameTime) {
			Update(gameTime.TotalGameTime.Ticks);
		}

		private static void Update(long currentTicks) {
			if (currentTicks > CurrentTicks) {
				CurrentTicks = currentTicks;
				LastState.Clear ();
				LastState.AddRange (CurrentState);
				CurrentState.Clear ();
				CurrentState.AddRange (Keyboard.GetState ().GetPressedKeys ());
			}
		}

		public static bool WasPressed(Keys key) {
			return LastState.Contains (key);
		}

		public static bool WasJustPressed(Keys key) {
			return 
				!WasPressed (key) &&
				CurrentState.Contains (key);
		}

		public static void ResetWasPressed() {
			if (CurrentTicks > 0) {
				CurrentTicks--;
				Update (CurrentTicks + 1);
			}
		}

		#endregion
	}
}

