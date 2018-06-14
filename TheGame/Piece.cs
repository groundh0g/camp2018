using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame
{
    public enum PieceTypes
    {
        Empty,
        NormalRed,
        NormalBlue,
        PacMan,
        ToggleColors,
        Stone,
        SwapLeft,
        SwapRight,
        SwapDown,
        Bomb,
        Kitty,
        GoTwice
    }

    public class Piece
    {
        public Texture2D Texture { get; set; }
        public PieceTypes PieceType { get; set; }
    }
}
