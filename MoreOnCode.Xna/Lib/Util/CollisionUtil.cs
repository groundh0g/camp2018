using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Lib.Graphics;

namespace MoreOnCode.Lib.Util
{
	public static class CollisionUtil
	{
		// overload for GetOpaqueData(Texture2D, Rectangle, byte)
		public static bool[,] GetOpaqueData(IPixelPerfectSprite sprite)
		{
			return GetOpaqueData(sprite.TextureData, sprite.TextureRect, 255);
		}

		// overload for GetOpaqueData(Texture2D, Rectangle, byte)
		public static bool[,] GetOpaqueData(
			IPixelPerfectSprite sprite, byte threshold)
		{
			return
				GetOpaqueData(sprite.TextureData, sprite.TextureRect, threshold);
		}

		// overload for GetOpaqueData(Texture2D, Rectangle, byte)
		public static bool[,] GetOpaqueData(Texture2D texture, Rectangle rect)
		{
			return GetOpaqueData(texture, rect, 255);
		}

		// extract pixel data from the texture
		public static bool[,] GetOpaqueData(
			Texture2D texture, Rectangle rect, byte threshold)
		{
			// temp variables to save some typing
			int width = rect.Width;
			int height = rect.Height;

			// an array of booleans, one for each pixel
			// true = opaque (considered), false = transparent (ignored)
			bool[,] data = new bool[width, height];

			// an array to hold the pixel data from the texture
			Color[] pixels = new Color[texture.Width * texture.Height];

			// I'd rather have used the overload for Texture2D.GetData
			// that specifies the texture rect, but that function didn't
			// work on the Xbox 360. This version pulls pixel data from 
			// the entire texture, but it should be OK since we're only 
			// using it briefly, and only on init of our game sprites.
			texture.GetData<Color>(pixels);

			// for each row of pixel data
			for (int y = 0; y < height; y++)
			{
				// for each column of pixel data
				for (int x = 0; x < width; x++)
				{
					// if the pixel's alpha exceeds our threshold,
					// note it in our boolean array
					if (pixels[
						rect.Left + x +
						(rect.Top + y) *
						texture.Width].A >= threshold)
					{
						data[x, y] = true;
					}
				}
			}

			// return our findings to the caller
			return data;
		}

		// REPLACE PLACEHOLDER CODE WITH THE FOLLOWING
		// TODO: Implement DetectCollision
		// REPLACE PLACEHOLDER CODE WITH THE FOLLOWING

		// overload for DetectCollision(Rect, Vec2, bool[], Rect, Vec2, bool[,])
		public static bool DetectCollision(
			IPixelPerfectSprite one, IPixelPerfectSprite two)
		{
			return DetectCollision(
				one.TextureRect, one.Location, one.OpaqueData,
				two.TextureRect, two.Location, two.OpaqueData);
		}

		// determine whether the bounding rectangles of the sprites overlap
		// if they do, compare pixel-by-pixel within the intersection
		public static bool DetectCollision(
			Rectangle rect1, Vector2 loc1, bool[,] data1,
			Rectangle rect2, Vector2 loc2, bool[,] data2)
		{
			return
				BoundsOverlap(rect1, loc1, rect2, loc2) &&
				PixelsTouch(rect1, loc1, data1, rect2, loc2, data2);
		}

		// see if the texture rectangles overlap, if they don't, there's
		// no need to do a pixel-by-pixel comparison
		private static bool BoundsOverlap(
			Rectangle rect1, Vector2 loc1,
			Rectangle rect2, Vector2 loc2)
		{
			// determine the top, left, bottom, right for rect1
			int top1 = (int)loc1.Y;
			int left1 = (int)loc1.X;
			int bottom1 = top1 + rect1.Height;
			int right1 = left1 + rect1.Width;

			// determine the top, left, bottom, right for rect2
			int top2 = (int)loc2.Y;
			int left2 = (int)loc2.X;
			int bottom2 = top2 + rect2.Height;
			int right2 = left2 + rect2.Width;

			return !(
				// rect1 fully to the right of rect2?
				left1 > right2 ||
				// rect1 fully to the left of rect2?
				right1 < left2 ||
				// rect1 fully below rect2?
				top1 > bottom2 ||
				// rect1 fully above rect2?
				bottom1 < top2
			);
		}

		// perform a pixel-by-pixel comparison
		private static bool PixelsTouch(
			Rectangle rect1, Vector2 loc1, bool[,] data1,
			Rectangle rect2, Vector2 loc2, bool[,] data2)
		{
			// update rects with locations of sprites
			rect1.X = (int)Math.Round(loc1.X);
			rect1.Y = (int)Math.Round(loc1.Y);
			rect2.X = (int)Math.Round(loc2.X);
			rect2.Y = (int)Math.Round(loc2.Y);

			// determine the intersection of the two rects
			Rectangle intersect = Rectangle.Empty;
			intersect.Y = Math.Max(rect1.Top, rect2.Top);
			intersect.X = Math.Max(rect1.Left, rect2.Left);
			int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
			int right = Math.Min(rect1.Right, rect2.Right);
			intersect.Height = bottom - intersect.Y;
			intersect.Width = right - intersect.X;

			// scan the intersected rectangle, pixel-by-pixel
			int x1 = intersect.X - rect1.X;
			int x2 = intersect.X - rect2.X;
			int y1 = intersect.Y - rect1.Y;
			int y2 = intersect.Y - rect2.Y;
			for (int y = 0; y < intersect.Height; y++)
			{
				for (int x = 0; x < intersect.Width; x++)
				{
					// are both pixels opaque?
					if (data1[x1 + x, y1 + y] &&
						data2[x2 + x, y2 + y])
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

