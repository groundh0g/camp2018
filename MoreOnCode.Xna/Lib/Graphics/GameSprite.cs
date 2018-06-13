using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Lib.Util;

namespace MoreOnCode.Lib.Graphics
{
	public class GameSprite : IPixelPerfectSprite
	{
		public Texture2D TextureData { get; set; }
		public Rectangle TextureRect { get; set; }
		public Rectangle OffsetRect { get; set; }
		public Vector2 Location { get; set; }
		public Vector2 Origin { get; set; }
		public Vector2 Scale { get; set; }
		public SpriteEffects SpriteEffects { get; set; }
		public float Depth { get; set; }
		public float Rotation { get; set; }
		public bool[,] OpaqueData { get; set; }
		public Color Tint = Color.White;

		public Rectangle DestRect {
			get {
				return new Rectangle (
					(int)Math.Round(Location.X - Origin.X),
					(int)Math.Round(Location.Y - Origin.Y),
					(int)Math.Round(TextureRect.Width * Scale.X),
					(int)Math.Round(TextureRect.Height * Scale.Y)
				);
			}

			set {
				Origin = Vector2.Zero;
				Rotation = 0.0f;
				Location = new Vector2 (value.X, value.Y);
				Scale = new Vector2 (
					(float)value.Width / (float)TextureRect.Width,
					(float)value.Height / (float)TextureRect.Height
				);
			}
		}

		public GameSprite (
			Texture2D texture, 
			Rectangle rectangle, 
			Rectangle rectangleOffset = default(Rectangle),
			Vector2 location = default(Vector2), 
			Vector2 origin = default(Vector2),
			Vector2? scale = null,
			SpriteEffects spriteEffects = SpriteEffects.None,
			float rotation = 0.0f,
			float depth = 0.0f,
			bool initCollisionData = true
		) : base() {
			this.TextureData = texture;
			this.TextureRect = rectangle;
			this.OffsetRect = rectangleOffset;
			this.Location = location;
			this.Origin = origin;
			this.Scale = scale.HasValue ? scale.Value : Vector2.One;
			this.SpriteEffects = spriteEffects;
			this.Rotation = rotation;
			this.Depth = depth;
			if (initCollisionData) {
				this.OpaqueData = CollisionUtil.GetOpaqueData (this);
			} else {
				this.OpaqueData = null;
			}
		}

		public GameSprite (GameSprite other) : base() {
			this.TextureData = other.TextureData;
			this.TextureRect = other.TextureRect;
			this.OffsetRect = other.OffsetRect;
			this.Location = other.Location;
			this.Origin = other.Origin;
			this.Scale = other.Scale;
			this.SpriteEffects = other.SpriteEffects;
			this.Rotation = other.Rotation;
			this.Depth = other.Depth;
			this.OpaqueData = other.OpaqueData;
		}

		// draw this sprite, using current settings, with specified overrides
		public void Draw(
			SpriteBatch batch, 
			Vector2? location = null, 
			Color? color = null,
			float? rotation = null,
			Vector2? origin = null,
			Vector2? scale = null,
			SpriteEffects? spriteEffects = null,
			float? depth = null
		)
		{
			batch.Draw (
				TextureData, 
				location.HasValue ? location.Value : Location, 
				TextureRect, 
				color.HasValue ? color.Value : Tint, 
				rotation.HasValue ? rotation.Value : Rotation, 
				origin.HasValue ? origin.Value : Origin, 
				scale.HasValue ? scale.Value : Scale, 
				spriteEffects.HasValue ? spriteEffects.Value : SpriteEffects, 
				depth.HasValue ? depth.Value : Depth);
		}

		// draw this sprite, using current settings, with specified overrides
		public void Draw(
			SpriteBatch batch, 
			Rectangle destRectangle, 
			Color? color = null,
			SpriteEffects? spriteEffects = null,
			float? depth = null
		)
		{
			batch.Draw (
				TextureData, 
				null, // location
				destRectangle, 
				TextureRect, 
				null, // origin 
				0.0f, // rotation, 
				null, // scale 
				color.HasValue ? color.Value : Tint, 
				spriteEffects.HasValue ? spriteEffects.Value : SpriteEffects, 
				depth.HasValue ? depth.Value : Depth);
		}

		// detect collision
		public bool Touches(GameSprite other) {
			return
				this.OpaqueData != null && 
				other.OpaqueData != null &&
				CollisionUtil.DetectCollision (this, other);
		}
	}
}