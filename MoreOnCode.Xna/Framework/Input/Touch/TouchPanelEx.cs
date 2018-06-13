using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MoreOnCode.Xna.Framework.Input.Touch
{
	public static class TouchPanelEx
	{
		static TouchPanelEx () { }

		private static bool IsConnected { 
			get { return TouchPanel.GetCapabilities ().IsConnected; } 
		}

		public static int DisplayWidth { 
			get { 
				return IsConnected ? 
					TouchPanel.DisplayWidth :
					GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			}
		}

		public static int DisplayHeight {
			get { 
				return IsConnected ? 
					TouchPanel.DisplayHeight :
					GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			}
		}

		public static DisplayOrientation DisplayOrientation { 
			get { 
				if (IsConnected) {
					return TouchPanel.DisplayOrientation;
				} else {
					return DisplayHeight > DisplayWidth ?
						DisplayOrientation.Portrait :
						DisplayOrientation.LandscapeLeft;
				}
			}
		}

		public static GestureType EnabledGestures {
			get {
				return IsConnected ?
					TouchPanel.EnabledGestures :
					GestureType.None;
			}
		}

		public static bool IsGestureAvailable { 
			get { 
				return IsConnected ?
					TouchPanel.IsGestureAvailable :
					false;
			}
		}

		public static GestureSample ReadGesture() {
			return IsConnected ?
				TouchPanel.ReadGesture() :
				new GestureSample();
		}

		public static IntPtr WindowHandle {
			get {
				return IsConnected ?
					TouchPanel.WindowHandle :
					IntPtr.Zero;
			}
		}

		public static TouchPanelCapabilitiesEx GetCapabilities() {
			return IsConnected ?
				new TouchPanelCapabilitiesEx (TouchPanel.GetCapabilities ()) :
				new TouchPanelCapabilitiesEx ();
		}

		public static TouchCollection GetState() {
			return IsConnected ?
				TouchPanel.GetState () :
				MakeTouchCollectionFromMouse();
		}

		private static Vector2? previousLocation = null;
		private static TouchLocationState previousTouchState = TouchLocationState.Invalid;

		private static TouchCollection MakeTouchCollectionFromMouse() {
			var mouseState = Mouse.GetState ();

			var touchState = TouchLocationState.Invalid;
			if (mouseState.LeftButton == ButtonState.Pressed) {
				switch (previousTouchState) {
				case TouchLocationState.Invalid:
				case TouchLocationState.Released:
					touchState = TouchLocationState.Pressed;
					break;
				default:
					touchState = TouchLocationState.Moved;
					break;
				}
			} else {
				switch (previousTouchState) {
				case TouchLocationState.Pressed:
				case TouchLocationState.Moved:
					touchState = TouchLocationState.Released;
					break;
				default:
					touchState = TouchLocationState.Invalid;
					break;
				}
			}

			var isValid = 
				mouseState.LeftButton == ButtonState.Pressed ||
				touchState == TouchLocationState.Released;

			var result = new List<TouchLocation>();
			if (isValid) {
				result.Add( new TouchLocation (
					0,
					touchState,
					new Vector2 (mouseState.Position.X, mouseState.Position.Y),
					previousTouchState,
					previousLocation.HasValue ? previousLocation.Value : Vector2.Zero));
			}

			previousLocation = new Vector2 (mouseState.Position.X, mouseState.Position.Y);
			previousTouchState = touchState;

			return new TouchCollection(result.ToArray());
		}

		#region "Events"

		public static long CurrentTicks { get; private set; }

		private static int CurrentCount { get; set; }
		private static int LastCount { get; set; }

		public static void Update(GameTime gameTime) {
			Update(gameTime.TotalGameTime.Ticks);
		}

		private static void Update(long currentTicks) {
			if (currentTicks > CurrentTicks) {
				CurrentTicks = currentTicks;
				LastCount = CurrentCount;
				CurrentCount = GetState ().Count;
			}
		}

		public static bool IsPressed() {
			return CurrentCount > 0;
		}

		public static bool WasPressed() {
			return LastCount > 0;
		}

		public static bool WasJustPressed() {
			return 
				!WasPressed() &&
				CurrentCount > 0;
		}

		public static bool WasJustReleased() {
			return 
				WasPressed() &&
				CurrentCount == 0;
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

