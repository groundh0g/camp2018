using System;
using System.Collections.Generic;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MoreOnCode.Lib.Graphics
{
	public class BitmapFont
	{
		public Texture2D Texture { get; protected set; }
		protected Dictionary<char, Rectangle> Glyphs { get; set; }
		public float Ascent { get; protected set; }
		public float Height { get; protected set; }

		// You can use a sprite packer to combine font sheets
		public int TextureOffsetX { get; set; }
		public int TextureOffsetY { get; set; }

		private BitmapFont (Texture2D texture, Dictionary<char, Rectangle> glyphs, float ascent, float height)
		{
			Texture = texture;
			Glyphs = glyphs;
			Ascent = ascent;
			Height = height;
			TextureOffsetX = 0;
			TextureOffsetY = 0;
		}

		// =============================
		// Load font
		// =============================

		public static Dictionary<string, BitmapFont> LoadedFonts =
			new Dictionary<string, BitmapFont> ();

		public static BitmapFont Load(ContentManager content, string fontName) {
			Texture2D texture = null;
			if(content != null) {
				texture = content.Load<Texture2D> (fontName);
			}
			var xml = new XmlDocument ();
			xml.Load(TitleContainer.OpenStream (fontName + ".xml"));
			var root = xml.SelectSingleNode ("/glyphs");
			var ascent = 0.0f;
			float.TryParse (root.Attributes ["ascent"].Value, out ascent);
			int height = (int)Math.Round (float.Parse (root.Attributes ["height"].Value));

			var glyphs = new Dictionary<char, Rectangle> ();
			foreach (XmlNode node in root.SelectNodes("/glyphs/glyph")) {
				var rect = Rectangle.Empty;
				rect.X = int.Parse (node.Attributes ["x"].Value);
				rect.Y = int.Parse (node.Attributes ["y"].Value);
				rect.Width = int.Parse (node.Attributes ["w"].Value);
				rect.Height = height;
				glyphs.Add(node.Attributes ["c"].Value[0], rect);
			}

			return new BitmapFont (texture, glyphs, ascent, height);
		}

		public static BitmapFont Load(ContentManager content, string fontName, float fontSize) {
			return Load (content, string.Format ("{0}_{1}", fontName, fontSize));
		}

		public static BitmapFont Load(Texture2D texture, string fontName, int offsetX, int offsetY) {
			var font = Load ((ContentManager)null, fontName);
			font.Texture = texture;
			font.TextureOffsetX = offsetX;
			font.TextureOffsetY = offsetY;
			return font;
		}

		public static BitmapFont Load(Texture2D texture, string fontName) {
			return Load(texture, fontName, 0, 0);
		}

		public static BitmapFont Load(Texture2D texture, string fontName, float fontSize) {
			return Load (texture, fontName, fontSize, 0, 0);
		}

		public static BitmapFont Load(Texture2D texture, string fontName, float fontSize, int offsetX, int offsetY) {
			return Load (texture, string.Format ("{0}_{1}", fontName, fontSize), offsetX, offsetY);
		}

		// -----------------------------

		public static Dictionary<string, BitmapFont> LoadFromTextureAtlas(ContentManager content, string assetName) {
			Texture2D texture = null;
			if(content != null) {
				texture = content.Load<Texture2D> (assetName);
			}
			return LoadFromTextureAtlas (texture, assetName);
		}

		public static Dictionary<string, BitmapFont> LoadFromTextureAtlas(Texture2D texture, string assetName) {
			var result = new Dictionary<string, BitmapFont> ();

			var fontRects = TextureAtlas.Load (assetName);
			foreach (var key in fontRects.Keys) {
				var font = BitmapFont.Load (texture, key);
				font.TextureOffsetX = fontRects [key].X;
				font.TextureOffsetY = fontRects [key].Y;
				result.Add (key, font);
			}

			return result;
		}

		// =============================
		// Draw and Measure String
		// =============================

		public Vector4 DrawString(SpriteBatch batch, Vector2 location, string text) {
			return DrawString (batch, location, text, Color.White, 1.0f, 0.0f);
		}

		public Vector4 DrawString(SpriteBatch batch, Vector2 location, string text, Color color) {
			return DrawString (batch, location, text, color, 1.0f, 0.0f);
		}

		public Vector4 DrawString(SpriteBatch batch, Vector2 location, string text, Color color, float scale) {
			return DrawString (batch, location, text, color, scale, 0.0f);
		}

		public Vector4 DrawString(SpriteBatch batch, Vector2 location, string text, Color color, float scale, float depth) {
			var loc = location;
			var maxLocX = loc.X;
			for (int i = 0; i < text.Length; i++) {
				char c = text [i];
				if (c == '\n') {
					loc.X = location.X;
					loc.Y += scale * Glyphs [' '].Height;
				} else {
					if (!Glyphs.ContainsKey (c)) {
						c = '?';
					}
					var rect = Glyphs [c];
					rect.X += TextureOffsetX;
					rect.Y += TextureOffsetY;
					if (batch != null) {
						batch.Draw (Texture, loc, rect, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, depth);
					}
					loc.X += scale * rect.Width;
					maxLocX = Math.Max (loc.X, maxLocX);
				}
			}
			return new Vector4 (location, maxLocX, loc.Y + scale * Glyphs [' '].Height);
		}

		public Vector4 MeasureString(string text) {
			return DrawString (null, Vector2.Zero, text, Color.White, 1.0f, 0.0f);
		}

		public Vector4 MeasureString(string text, float scale) {
			return DrawString (null, Vector2.Zero, text, Color.White, scale, 0.0f);
		}

		// =============================
	}
}

