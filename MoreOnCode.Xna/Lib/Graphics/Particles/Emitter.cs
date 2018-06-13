using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Lib;

namespace MoreOnCode.Lib.Graphics.Particles
{
	public class Emitter
	{
		// default constructor, 1000 particles
		public Emitter() : this(1000) { }

		// init emitter with max particles
		public Emitter(long maxParticles) : base()
		{ 
			this.MaxParticles = maxParticles;
			this.ParticlesPerUpdate = 1;
			this.ParticleLifetime = 10.0f;
			this.Active = true;
			this.Enabled = true;
			this.EmitterRV2 = new RangedVector2();
		}


		// helper to init particle locations within emitter bounds
		protected RangedVector2 EmitterRV2 { get; set; }

		// location and size of the emitter
		protected Rectangle m_EmitterRect;
		public Rectangle EmitterRect
		{
			get { return m_EmitterRect; }
			set
			{
				m_EmitterRect = value;
				EmitterRV2 = RangedVector2.FromRectangle(m_EmitterRect);
			}
		}

		// helper to update the location of the emitter
		public Vector2 Position
		{
			get { return new Vector2(m_EmitterRect.Left, m_EmitterRect.Top); }
			set
			{
				EmitterRect = new Rectangle(
					(int)Math.Round(value.X),
					(int)Math.Round(value.Y),
					m_EmitterRect.Width,
					m_EmitterRect.Height);
			}
		}

		public Rectangle EmitterBoundsRect { get; set; }
		public RangedVector2 RangeVelocity { get; set; }
		public RangedVector4 RangeColor { get; set; }

		public Rectangle TextureRect { get; set; }

		protected Texture2D m_Texture;
		public Texture2D Texture
		{
			get { return m_Texture; }
			set
			{
				m_Texture = value;
				TextureRect = m_Texture.Bounds;
			}
		}

		public bool Enabled { get; set; } // false? don't draw, don't update
		public bool Active { get; set; } // false? draw and update, no new
		public long ParticlesPerUpdate { get; set; } // particles per frame

		// max number of particles that this emitter can track
		protected long m_MaxParticles = 1000;
		public long MaxParticles
		{
			get { return m_MaxParticles; }
			set
			{
				m_MaxParticles = Math.Max(value, 1L);
				m_ActiveParticles.Clear();
				m_InactiveParticles.Clear();
				for (long i = 0; i < m_MaxParticles; i++)
				{
					var particle = new Particle ();
					particle.LifeTime = this.ParticleLifetime;
					particle.Scale = 1.0f;
					m_InactiveParticles.Add(particle);
				}
			}
		}

		// lifespan of particle, expressed in seconds
		public float ParticleLifetime { get; set; }

		// when to start drawing the little guys
		public float ParticleMinAgeToDraw { get; set; } 

		// keep track of active and inactive particles
		protected List<Particle> m_ActiveParticles = new List<Particle>();
		protected List<Particle> m_InactiveParticles = new List<Particle>();

		// keep track of attached modifiers
		public List<Modifier> Modifiers = new List<Modifier>();

		// manage active particles, spawn new particles if it's time to do so
		public virtual void Update(double elapsed)
		{
			// only update if enabled
			if (Enabled)
			{
				// temp variables to save typing
				bool outOfBounds;
				int parX, parY;
				for (int i = 0; i < m_ActiveParticles.Count; i++)
				{
					var particle = m_ActiveParticles[i];

					// when particle leaves emitter bounds, mark inactive
					parX = (int)Math.Round(particle.Position.X);
					parY = (int)Math.Round(particle.Position.Y);
					outOfBounds = 
						parX > EmitterBoundsRect.Right ||
						parX + TextureRect.Width < EmitterBoundsRect.Left ||
						parY > EmitterBoundsRect.Bottom ||
						parY + TextureRect.Height < EmitterBoundsRect.Top;
					if (outOfBounds) particle.IsActive = false;

					// process active particles, cleanup inactive particles
					if (particle.IsActive)
					{
						// allow active modifiers to update particle
						foreach (var modifier in Modifiers)
						{
							if (modifier.Enabled)
							{
								// tell the modifier to update this particle
								modifier.Update(particle, elapsed);
							}
						}
						// tell particle to update itself
						particle.Update(elapsed);
						m_ActiveParticles[i] = particle;
					}
					else
					{
						// move particle to inactive list for later reuse
						m_InactiveParticles.Add(particle);
						m_ActiveParticles.RemoveAt(i--);
					}
				}

				// try to generate ParticlesPerUpdate new particles
				for (long i = 0; Active && i < ParticlesPerUpdate; i++)
				{
					if (m_InactiveParticles.Count > 0)
					{
						// reset particle and add it to our pool of active particles
						var particle = m_InactiveParticles[0];
						particle.Position = EmitterRV2.RandomValue();
						particle.Velocity = RangeVelocity == null ? Vector2.Zero : RangeVelocity.RandomValue();
						particle.Color = RangeColor.RandomValue();
						particle.IsActive = true;
						particle.LifeTime = ParticleLifetime;
						particle.Scale = 1.0f;
						particle.Age = 0.0f;
						m_InactiveParticles.RemoveAt(0);
						m_ActiveParticles.Add(particle);
					}

					else
					{
						// no more particles in our inactive pool
						break;
					}
				}
			}
		}

		// render the active particles
		public virtual void Draw(SpriteBatch batch)
		{
			// only draw particles when emitter is enabled
			if (Enabled)
			{
				foreach (var particle in m_ActiveParticles)
				{
					// ask the particle to draw itself
					if (particle.Age >= ParticleMinAgeToDraw) {
						particle.Draw (batch, Texture, TextureRect);
					}
				}
			}
		}

	}
}

