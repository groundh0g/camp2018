using System;

using Microsoft.Xna.Framework;

namespace MoreOnCode.Lib.Graphics.Particles
{
	// update a particle based on custom code, used by emitter
	public class Modifier
	{
		// only update active modifiers
		public bool Enabled;
		public Vector2 PositionDelta;
		public Vector2 VelocityDelta;
		public float RotationDelta;
		public float ScaleDelta;
		public float DepthDelta;

		// called for each particle, every frame, by emitter, if enabled
		public void Update(Particle particle, double elapsed)
		{
			float elapsedFloat = (float)elapsed;
			if (PositionDelta != Vector2.Zero) 
			{ particle.Position += PositionDelta * elapsedFloat; }
			if (VelocityDelta != Vector2.Zero) 
			{ particle.Velocity += VelocityDelta * elapsedFloat; }
			if (RotationDelta != 0.0f) 
			{ particle.Rotation += RotationDelta * elapsedFloat; }
			if (ScaleDelta != 0.0f) 
			{ particle.Scale += ScaleDelta * elapsedFloat; }
			if (DepthDelta != 0.0f) 
			{ particle.Depth += DepthDelta * elapsedFloat; }

			// allow for custom modifier logic
			if (OnUpdate != null) {
				OnUpdate (particle, elapsed);
			}
		}

		public delegate void OnUpdateDelegate(
			Particle particle, 
			double elapsed);
		public OnUpdateDelegate OnUpdate;

	}
}

