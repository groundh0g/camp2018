using System;

using Microsoft.Xna.Framework.Input.Touch;

namespace MoreOnCode.Xna.Framework.Input.Touch
{
	public class TouchPanelCapabilitiesEx
	{
		public bool IsConnected;
		public bool HasPressure;
		public int MaximumTouchCount;

		public TouchPanelCapabilitiesEx ()
		{
			IsConnected = true;
			MaximumTouchCount = 1;
		}

		public TouchPanelCapabilitiesEx (TouchPanelCapabilities copy)
		{
			IsConnected = copy.IsConnected;
			MaximumTouchCount = copy.MaximumTouchCount;
			HasPressure = copy.HasPressure;
		}

	}
}

