﻿using System;
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
        //PacMan,
        ToggleColors,
        Stone,
        SwapLeft,
        SwapRight,
        SwapDown,
        Bomb,
        Kitty,
        //GoTwice,
    }

    public class Piece
    {
        public Texture2D Texture { get; set; }
        public PieceTypes PieceType { get; set; }
        public Vector2 Delta { get; set; }

        public static Piece Empty { get { return new Piece() { PieceType = PieceTypes.Empty }; } }
        public bool IsChecked { get; set; }
        public bool IsExploded { get; set; }

        public Piece() { }
        public Piece(Piece src)
        {
            this.Texture = src.Texture;
            this.PieceType = src.PieceType;
            this.Delta = src.Delta;
            this.IsChecked = false;
            this.IsExploded = false;
        }
    }
}
