using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoreOnCode.Lib.Graphics.Particles
{
	public class Particle
	{
		public bool IsActive;
		public double Age;
		public double LifeTime;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector4 Color;
		public float Rotation;
		public float Opacity;
		public float Scale;
		public float Depth;

		// update position and age
		public void Update(double elapsed)
		{
			float elapsedFloat = (float)elapsed;

			// only update active particles
			if (IsActive)
			{
				// move the particle
				Position += Velocity * elapsedFloat;

				// check for expired particles
				if (LifeTime > 0.0f) // 0.0f == never dies
				{
					Age += elapsed;
					if (Age > LifeTime)
					{
						IsActive = false;
					}
				}
			}
		}

		// render the particle
		public void Draw(
			SpriteBatch batch,
			Texture2D texture,
			Rectangle clipRect)
		{
			// only draw active particles
			if (IsActive)
			{
				batch.Draw(
					texture,
					Position,
					clipRect,
					new Color(Color),
					Rotation,
					Vector2.Zero,
					Scale,
					SpriteEffects.None,
					Depth);
			}
		}
	}
}

