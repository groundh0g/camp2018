using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Lib.Graphics;

namespace MoreOnCode.Lib.Util
{
	public static class SpriteUtil
	{
		public enum VerticalAlign 
		{
			Top,
			Middle,
			Bottom,
		}

		public enum HorizontalAlign 
		{
			Left,
			Center,
			Right,
		}

		public static void DrawInRect(
			SpriteBatch spriteBatch, 
			GameSprite sprite, 
			Rectangle bounds, 
			HorizontalAlign alignHorizontal = HorizontalAlign.Center,
			VerticalAlign alignVertical = VerticalAlign.Middle
		) 
		{
			Vector2 location = Vector2.Zero;
			Vector2 origin = Vector2.Zero;

			switch (alignHorizontal) {
			case HorizontalAlign.Left:
				location.X = bounds.X;
				origin.X = 0;
				break;
			case HorizontalAlign.Center:
				location.X = bounds.X + bounds.Width / 2;
				origin.X = sprite.TextureRect.Width / 2;
				break;
			case HorizontalAlign.Right:
				location.X = bounds.X + bounds.Width;
				origin.X = sprite.TextureRect.Width;
				break;
			}

			switch (alignVertical) {
			case VerticalAlign.Top:
				location.Y = bounds.Y;
				origin.Y = 0;
				break;
			case VerticalAlign.Middle:
				location.Y = bounds.Y + bounds.Height / 2;
				origin.Y = sprite.TextureRect.Height / 2;
				break;
			case VerticalAlign.Bottom:
				location.Y = bounds.Y + bounds.Height;
				origin.Y = sprite.TextureRect.Height;
				break;
			}

			sprite.Draw (spriteBatch, location, sprite.Tint, sprite.Rotation, origin, sprite.Scale, sprite.SpriteEffects, sprite.Depth);
		}

		public enum FillMode
		{
			Fill,
			Scale,
			Tile,
			TileRandomFlipHorizontal,
			TileRandomFlipVertical,
			TileRandomFlipBoth,
		}

		public static void FillRect(
			SpriteBatch spriteBatch,
			GameSprite sprite,
			Rectangle bounds,
			FillMode fillMode = FillMode.Scale,
			Vector2? scroll = null
		) 
		{
			var location = new Vector2 (bounds.X, bounds.Y);
			var origin = Vector2.Zero;
			var rotation = 0.0f;
			var scale = Vector2.One;
			var depth = 0.0f;

			switch (fillMode) {
			case FillMode.Fill:
				scale.X = (float)bounds.Width / (float)sprite.TextureRect.Width;
				scale.Y = (float)bounds.Height / (float)sprite.TextureRect.Height;
				sprite.Draw (spriteBatch, location, sprite.Tint, rotation, origin, scale, SpriteEffects.None, depth);
				break;
			case FillMode.TileRandomFlipHorizontal:
			case FillMode.TileRandomFlipVertical:
			case FillMode.TileRandomFlipBoth:
			case FillMode.Tile:
				var rand = new Random (42);
				var doRand = scroll != Vector2.Zero;

				var startY = bounds.Y - (int)Math.Round (scroll.HasValue ? scroll.Value.Y : 0);
				var startX = bounds.X - (int)Math.Round (scroll.HasValue ? scroll.Value.X : 0);
				var rectHeight = sprite.TextureRect.Height;
				var rectWidth = sprite.TextureRect.Width;

				startY = startY % rectHeight;
				startX = startX % rectWidth;
				if (startY > 0) { startY -= rectHeight; }
				if (startX > 0) { startX -= rectWidth; }

				for (int y = startY; y < bounds.Y + bounds.Height; y += rectHeight ) {
					location.Y = y;
					for (int x = startX; x < bounds.X + bounds.Width; x += rectWidth) {
						location.X = x;
						var spriteEffects = SpriteEffects.None;
						switch (fillMode) {
						case FillMode.TileRandomFlipHorizontal:
							if (doRand && rand.Next () % 2 == 1) {
								spriteEffects = SpriteEffects.FlipHorizontally;
							}
							break;
						case FillMode.TileRandomFlipVertical:
							if (doRand && rand.Next () % 2 == 1) {
								spriteEffects = SpriteEffects.FlipVertically;
							}
							break;
						case FillMode.TileRandomFlipBoth:
							if (doRand) {
								switch (rand.Next () % 4) {
								case 1:
									spriteEffects = SpriteEffects.FlipHorizontally;
									break;
								case 2:
									spriteEffects = SpriteEffects.FlipVertically;
									break;
								case 3:
									spriteEffects = 
										SpriteEffects.FlipHorizontally | 
										SpriteEffects.FlipVertically;
									break;
								}
							}
							break;
						}
						sprite.Draw (spriteBatch, location, sprite.Tint, rotation, origin, scale, spriteEffects, depth);
					}
				}
				break;
			case FillMode.Scale:
			default:
				var theScale = (float)bounds.Width / (float)sprite.TextureRect.Width;
				if (sprite.TextureRect.Height * theScale > bounds.Height) {
					theScale = (float)bounds.Height / (float)sprite.TextureRect.Height;
					location.Y = 
						bounds.Height / 2 -
						(sprite.TextureRect.Height * theScale) / 2;
				} else {
					location.X = 
						bounds.Width / 2 -
						(sprite.TextureRect.Width * theScale) / 2;
				}
				scale.X = scale.Y = theScale;
				sprite.Draw (spriteBatch, location, sprite.Tint, rotation, origin, scale, SpriteEffects.None, depth);
				break;
			}
		}
	}
}

