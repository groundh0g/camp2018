using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoreOnCode.Lib.Graphics
{
	public interface IPixelPerfectSprite
	{
		Texture2D TextureData { get; set; }
		Rectangle TextureRect { get; set; }
		Vector2 Location { get; set; }
		bool[,] OpaqueData { get; set; }
	}
}

