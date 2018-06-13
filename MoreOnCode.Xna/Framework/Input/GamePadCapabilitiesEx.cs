using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace MoreOnCode.Xna.Framework.Input
{
	public struct GamePadCapabilitiesEx
	{
		public bool IsConnected { get; set; }
		public bool HasAButton { get; set; }
		public bool HasBackButton { get; set; }
		public bool HasBButton { get; set; }
		public bool HasDPadDownButton { get; set; }
		public bool HasDPadLeftButton { get; set; }
		public bool HasDPadRightButton { get; set; }
		public bool HasDPadUpButton { get; set; }
		public bool HasLeftShoulderButton { get; set; }
		public bool HasLeftStickButton { get; set; }
		public bool HasRightShoulderButton { get; set; }
		public bool HasRightStickButton { get; set; }
		public bool HasStartButton { get; set; }
		public bool HasXButton { get; set; }
		public bool HasYButton { get; set; }
		public bool HasBigButton { get; set; }
		public bool HasLeftXThumbStick { get; set; }
		public bool HasLeftYThumbStick { get; set; }
		public bool HasRightXThumbStick { get; set; }
		public bool HasRightYThumbStick { get; set; }
		public bool HasLeftTrigger { get; set; }
		public bool HasRightTrigger { get; set; }
		public bool HasLeftVibrationMotor { get; set; }
		public bool HasRightVibrationMotor { get; set; }
		public bool HasVoiceSupport { get; set; }
		public GamePadType GamePadType { get; set; }

		public GamePadCapabilitiesEx(GamePadCapabilities copy) : this() {
			this.IsConnected = copy.IsConnected;
			this.HasAButton = copy.HasAButton;
			this.HasBackButton = copy.HasBackButton;
			this.HasBButton = copy.HasBButton;
			this.HasDPadDownButton = copy.HasDPadDownButton;
			this.HasDPadLeftButton = copy.HasDPadLeftButton;
			this.HasDPadRightButton = copy.HasDPadRightButton;
			this.HasDPadUpButton = copy.HasDPadUpButton;
			this.HasLeftShoulderButton = copy.HasLeftShoulderButton;
			this.HasLeftStickButton = copy.HasLeftStickButton;
			this.HasRightShoulderButton = copy.HasRightShoulderButton;
			this.HasRightStickButton = copy.HasRightStickButton;
			this.HasStartButton = copy.HasStartButton;
			this.HasXButton = copy.HasXButton;
			this.HasYButton = copy.HasYButton;
			this.HasBigButton = copy.HasBigButton;
			this.HasLeftXThumbStick = copy.HasLeftXThumbStick;
			this.HasLeftYThumbStick = copy.HasLeftYThumbStick;
			this.HasRightXThumbStick = copy.HasRightXThumbStick;
			this.HasRightYThumbStick = copy.HasRightYThumbStick;
			this.HasLeftTrigger = copy.HasLeftTrigger;
			this.HasRightTrigger = copy.HasRightTrigger;
			this.HasLeftVibrationMotor = copy.HasLeftVibrationMotor;
			this.HasRightVibrationMotor = copy.HasRightVibrationMotor;
			this.HasVoiceSupport = copy.HasVoiceSupport;
			this.GamePadType = copy.GamePadType;
		}

		public GamePadCapabilitiesEx(Dictionary<Keys, Buttons> keyMappings) : this() {
			this.IsConnected = true;
			this.HasAButton = keyMappings.ContainsValue(Buttons.A);
			this.HasBackButton = keyMappings.ContainsValue(Buttons.A);
			this.HasBButton = keyMappings.ContainsValue(Buttons.A);
			this.HasDPadDownButton = keyMappings.ContainsValue(Buttons.A);
			this.HasDPadLeftButton = keyMappings.ContainsValue(Buttons.A);
			this.HasDPadRightButton = keyMappings.ContainsValue(Buttons.A);
			this.HasDPadUpButton = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftShoulderButton = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftStickButton = keyMappings.ContainsValue(Buttons.A);
			this.HasRightShoulderButton = keyMappings.ContainsValue(Buttons.A);
			this.HasRightStickButton = keyMappings.ContainsValue(Buttons.A);
			this.HasStartButton = keyMappings.ContainsValue(Buttons.A);
			this.HasXButton = keyMappings.ContainsValue(Buttons.A);
			this.HasYButton = keyMappings.ContainsValue(Buttons.A);
			this.HasBigButton = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftXThumbStick = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftYThumbStick = keyMappings.ContainsValue(Buttons.A);
			this.HasRightXThumbStick = keyMappings.ContainsValue(Buttons.A);
			this.HasRightYThumbStick = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftTrigger = keyMappings.ContainsValue(Buttons.A);
			this.HasRightTrigger = keyMappings.ContainsValue(Buttons.A);
			this.HasLeftVibrationMotor = false;
			this.HasRightVibrationMotor = false;
			this.HasVoiceSupport = false;
			this.GamePadType = GamePadType.GamePad;
		}

	}
}

