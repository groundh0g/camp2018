using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework;

namespace MoreOnCode.Xna.Framework.Input
{
	public static class GamePadEx
	{
		public const float TRIGGER_MAX = 1.00f;
		public const float TRIGGER_MIN = 0.00f;
		public const float THUMBSTICK_MAX = 1.00f;
		public const float THUMBSTICK_MIN = -1.00f;

		static GamePadEx() {
			KeyMappings = KEY_MAPPINGS_PROFILE_1;
		}

		// set the emulated controller index
		public static PlayerIndexEx KeyboardPlayerIndexEx = PlayerIndexEx.None;
		public static PlayerIndex? KeyboardPlayerIndex { 
			get {
				PlayerIndex? playerIndex = null;
				switch (KeyboardPlayerIndexEx) {
				case PlayerIndexEx.One:
				case PlayerIndexEx.Two:
				case PlayerIndexEx.Three:
				case PlayerIndexEx.Four:
					playerIndex = (PlayerIndex)KeyboardPlayerIndexEx;
					break;
				case PlayerIndexEx.Auto:
					for (int i = 0; i < 4; i++) {
						if (!IsGamePadConnected (i)) {
							playerIndex = (PlayerIndex)i;
							break;
						}
					}
					break;
				}
				return playerIndex;
			}

			set {
				if (value.HasValue) {
					switch (value.Value) {
					case PlayerIndex.One:
					case PlayerIndex.Two:
					case PlayerIndex.Three:
					case PlayerIndex.Four:
						KeyboardPlayerIndexEx = (PlayerIndexEx)value.Value;
						break;
					}
				} else {
					KeyboardPlayerIndexEx = PlayerIndexEx.None;
				}
			}
		}

		private static bool IsGamePadConnected (int i) {
			GamePadState? state = null;
			try {
				state = GamePad.GetState ((PlayerIndex)i);
			} catch { }
			return state.HasValue && state.Value.IsConnected;
		}

		// is the specified playerIndex the one associated with the keyboard?
		private static bool IsKeyboardPlayerIndex(PlayerIndex playerIndex) 
		{
			return 
				KeyboardPlayerIndex.HasValue &&
				KeyboardPlayerIndex.Value == playerIndex;
		}

		public static Dictionary<Keys, Buttons> KeyMappings;

		public static bool SetVibration(
			PlayerIndex playerIndex, 
			float leftMotor, 
			float rightMotor)
		{
			var result = false;
			if (!IsKeyboardPlayerIndex (playerIndex)) {
				try {
					result = GamePad.SetVibration (playerIndex, leftMotor, rightMotor);
				} catch { }
			}
			return result;
		}

		public static GamePadCapabilitiesEx GetCapabilities (PlayerIndex playerIndex) {
			GamePadCapabilitiesEx? result = null;

			if (IsKeyboardPlayerIndex (playerIndex)) {
				result = new GamePadCapabilitiesEx (GamePadEx.KeyMappings);
			} else {
				try {
					result = new GamePadCapabilitiesEx(GamePad.GetCapabilities(playerIndex));
				} catch { }
			}

			return result.HasValue ? result.Value : new GamePadCapabilitiesEx ();
		}

		public static GamePadState GetState(PlayerIndex playerIndex) {
			return GamePadEx.GetState (playerIndex, GamePadDeadZone.IndependentAxes);
		}

		public static GamePadState GetState(PlayerIndex playerIndex, GamePadDeadZone deadZoneMode) {
			GamePadState? result = null;

			try {
				if(!IsKeyboardPlayerIndex(playerIndex)) {
					result = GamePad.GetState(playerIndex, deadZoneMode);
				} else {
					var keyboardState = Keyboard.GetState();
					var pressedButtons = new List<Buttons>();
					foreach(Keys key in keyboardState.GetPressedKeys()) {
						if(GamePadEx.KeyMappings.ContainsKey(key)) {
							pressedButtons.Add(GamePadEx.KeyMappings[key]);
						}
					}

					var leftThumbstick = Vector2.Zero;
					if(pressedButtons.Contains(Buttons.DPadUp)) {
						leftThumbstick.Y = THUMBSTICK_MAX;
					} else if(pressedButtons.Contains(Buttons.DPadDown)) {
						leftThumbstick.Y = THUMBSTICK_MIN;
					}
					if(pressedButtons.Contains(Buttons.DPadRight)) {
						leftThumbstick.X = THUMBSTICK_MAX;
					} else if(pressedButtons.Contains(Buttons.DPadLeft)) {
						leftThumbstick.X = THUMBSTICK_MIN;
					}

                    Buttons buttons = 0;
                    foreach(var btn in pressedButtons)
                    {
                        buttons = buttons | btn;
                    }

                    result = new GamePadState(
                        leftThumbstick,
                        Vector2.Zero, // Right Thumbstick
                        0.0f, // Left Trigger
                        0.0f, // Right Trigger
                        buttons); // pressedButtons[0]); // .ToArray());
                    // TODO: FIX ME!!!!
					}
			} catch { }

			return result.HasValue ? result.Value : GamePadState.Default;
		}

		public static readonly Dictionary<Keys, Buttons> KEY_MAPPINGS_PROFILE_1 = new Dictionary<Keys, Buttons> {
			{ Keys.W, Buttons.DPadUp },
			{ Keys.S, Buttons.DPadDown },
			{ Keys.A, Buttons.DPadLeft },
			{ Keys.D, Buttons.DPadRight },
			{ Keys.Up, Buttons.DPadUp },
			{ Keys.Down, Buttons.DPadDown },
			{ Keys.Left, Buttons.DPadLeft },
			{ Keys.Right, Buttons.DPadRight },
			{ Keys.Space, Buttons.A },
			{ Keys.RightControl, Buttons.B },
			{ Keys.Enter, Buttons.Start },
			{ Keys.Escape, Buttons.Back },
			{ Keys.Back, Buttons.Back },
		};

		#region "Events"

		public static long CurrentTicks { get; private set; }

		private static readonly Dictionary<PlayerIndex, GamePadState> CurrentState =
			new Dictionary<PlayerIndex, GamePadState> ();

		private static readonly Dictionary<PlayerIndex, GamePadState> LastState =
			new Dictionary<PlayerIndex, GamePadState> ();

		private static readonly PlayerIndex[] indexes = {
			PlayerIndex.One,
			PlayerIndex.Two,
			PlayerIndex.Three,
			PlayerIndex.Four,
		};

		public static void Update(GameTime gameTime) {
			Update(gameTime.TotalGameTime.Ticks);
		}

		private static void Update(long currentTicks) {
			if (currentTicks > CurrentTicks) {
				CurrentTicks = currentTicks;

				foreach (var playerIndex in indexes) {
					if (CurrentState.ContainsKey (playerIndex)) {
						LastState [playerIndex] = CurrentState [playerIndex];
					} else {
						LastState [playerIndex] = GamePadState.Default;
					}
					CurrentState [playerIndex] = GetState (playerIndex);
				}
			}
		}

		public static bool WasPressed(PlayerIndex playerIndex, Buttons button) {
			return 
				LastState.ContainsKey (playerIndex) &&
				LastState [playerIndex].IsButtonDown (button);
		}

		public static bool WasJustPressed(PlayerIndex playerIndex, Buttons button) {
			return 
				!WasPressed(playerIndex, button) &&
				CurrentState.ContainsKey (playerIndex) &&
				CurrentState [playerIndex].IsButtonDown (button);
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

