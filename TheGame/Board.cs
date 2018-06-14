using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class Board
    {
        public Piece[,] Pieces = new Piece[8, 8];

        public Board()
        {
            ClearBoard();
        }

        public void ClearBoard()
        {
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    Pieces[x, y] = new Piece {
                        Texture = null,
                        PieceType = PieceTypes.Empty
                    };
                }
            }
        }

        public void Scramble()
        {
            var values = Enum.GetValues(typeof(PieceTypes));
            var rand = new Random();

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = (PieceTypes)values.GetValue(rand.Next(values.Length - 1));
                    Pieces[x, y].PieceType = piece;
                }
            }
        }

        /// <summary>
        /// This will scan the board for match-four.
        /// </summary>
        /// <returns>NormalRed, NormalBlue, or Empty (for a tie)</returns>
        public PieceTypes ScanForMatches()
        {
            for (int y = 0; y < 8; y++)
            {
                var temp = PieceTypes.Empty;
                var tempCount = 0;
                for (int x = 1; x < 8; x++)
                {
                    if (Pieces[x, y] == Pieces[x - 1, y])
                    {
                        temp = Pieces[x, y].PieceType;
                        tempCount++;
                        if (tempCount >= 3) return temp;
                    }
                    else
                    {
                        temp = PieceTypes.Empty;
                        tempCount = 0;
                    }
                }
            }

            for (int x = 0; x < 8; x++)
            {
                var temp = PieceTypes.Empty;
                var tempCount = 0;
                for (int y = 1; y < 8; y++)
                {
                    if (Pieces[x, y] == Pieces[x, y - 1])
                    {
                        temp = Pieces[x, y].PieceType;
                        tempCount++;
                        if (tempCount >= 3) return temp;
                    }
                    else
                    {
                        temp = PieceTypes.Empty;
                        tempCount = 0;
                    }
                }
            }

            // TODO: Scan Diagonal x2?


            return PieceTypes.Empty;
        }

        float speed = 500.0f;

        public bool DoGravity(float elapsed)
        {
            var result = false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 7; y >= 0; y--)
                {
                    if (Pieces[x, y].PieceType != PieceTypes.Empty)
                    {
                        Pieces[x, y].Delta = new Vector2(0, Math.Max(0, Pieces[x, y].Delta.Y - speed * elapsed));
                    }
                }
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 7; y > 0; y--)
                {
                    if (Pieces[x, y].PieceType == PieceTypes.Empty)
                    {
                        var piece = Pieces[x, y - 1];
                        if (piece.PieceType != PieceTypes.Empty && piece.Delta == Vector2.Zero)
                        {
                            Pieces[x, y] = Pieces[x, y - 1];
                            Pieces[x, y - 1] = Piece.Empty;
                            Pieces[x, y].Delta = new Vector2(0, 128);
                        }
                    }
                }
            }

            return result;
        }
    }
}
