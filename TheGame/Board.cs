using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame
{
    public class Board
    {
        public Piece[,] Pieces = new Piece[8, 8];
        public PlayerIndex Player;

        public Board()
        {
            ClearBoard();

            //BlueQueue = new List<Piece>();
            //RedQueue = new List<Piece>();
        }

        public List<Piece> RedQueue = new List<Piece>() { Piece.Empty, Piece.Empty, Piece.Empty, Piece.Empty };
        public List<Piece> BlueQueue = new List<Piece>() { Piece.Empty, Piece.Empty, Piece.Empty, Piece.Empty };

        public Random rand = new Random();
        public void FillQueue(List<Piece> queue, PieceTypes type)
        {

            if (queue[0].PieceType == PieceTypes.Empty) { queue[0] = new Piece() { PieceType = type }; }
            for(int i = 1; i < queue.Count; i++)
            {
                if(queue[i].PieceType == PieceTypes.Empty)
                {
                    var values = Enum.GetValues(typeof(PieceTypes));
                    var pieceType = PieceTypes.Empty;

                    while(pieceType == PieceTypes.Empty)
                    {
                        pieceType = (PieceTypes)values.GetValue(rand.Next(values.Length - 1));
                        if (MatchOn.Contains(pieceType)) { pieceType = PieceTypes.Empty; }
                    }

                    queue[i] = new Piece() { PieceType = pieceType };
                }
            }
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

            Player = PlayerIndex.One;

            FillQueue(BlueQueue, PieceTypes.NormalBlue);
            FillQueue(RedQueue, PieceTypes.NormalRed);
        }

        public void TogglePlayer()
        {
            Player = Player == PlayerIndex.One ? PlayerIndex.One : PlayerIndex.Two;
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
                    var normalTile = rand.Next(10);
                    Pieces[x, y].PieceType = 
                        normalTile < 3 ? PieceTypes.NormalBlue  : 
                        normalTile < 6 ? PieceTypes.NormalRed : piece;
                }
            }

            FillQueue(BlueQueue, PieceTypes.NormalBlue);
            FillQueue(RedQueue, PieceTypes.NormalRed);
        }

        private List<PieceTypes> MatchOn = new List<PieceTypes>()
        {
            PieceTypes.NormalBlue,
            PieceTypes.NormalRed,
        };

        public static List<PieceTypes> MatchOnRed = new List<PieceTypes>()
        {
            PieceTypes.NormalRed,
        };

        public static List<PieceTypes> MatchOnBlue = new List<PieceTypes>()
        {
            PieceTypes.NormalBlue,
        };

        public bool IsFull
        {
            get {
                var countEmpty = 0;
                for(int y = 0; y< 8; y++)
                {
                    for(int x = 0; x < 8; x++)
                    {
                        if(Pieces[x,y].PieceType == PieceTypes.Empty)
                        {
                            countEmpty++;
                        }
                    }
                }
                return countEmpty == 0;
            }
        }

        /// <summary>
        /// This will scan the board for match-four.
        /// </summary>
        /// <returns>NormalRed, NormalBlue, or Empty (for a tie)</returns>
        public PieceTypes ScanForMatches(List<PieceTypes> matchOn)
        {
            matchOn = matchOn != null && matchOn.Count > 0 ? matchOn : MatchOn;
            // Scan Horizontal
            for (int y = 0; y < 8; y++)
            {
                var temp = PieceTypes.Empty;
                var tempCount = 0;
                for (int x = 1; x < 8; x++)
                {
                    if (matchOn.Contains(Pieces[x, y].PieceType) && Pieces[x, y].PieceType == Pieces[x - 1, y].PieceType)
                    {
                        temp = Pieces[x, y].PieceType;
                        tempCount++;
                        if (tempCount >= 3)
                        {
                            Pieces[x - 0, y].IsChecked = true;
                            Pieces[x - 1, y].IsChecked = true;
                            Pieces[x - 2, y].IsChecked = true;
                            Pieces[x - 3, y].IsChecked = true;
                            return temp;
                        }
                    }
                    else
                    {
                        temp = PieceTypes.Empty;
                        tempCount = 0;
                    }
                }
            }

            // Scan Vertical
            for (int x = 0; x < 8; x++)
            {
                var temp = PieceTypes.Empty;
                var tempCount = 0;
                for (int y = 1; y < 8; y++)
                {
                    if (matchOn.Contains(Pieces[x, y].PieceType) && Pieces[x, y].PieceType == Pieces[x, y - 1].PieceType)
                    {
                        temp = Pieces[x, y].PieceType;
                        tempCount++;
                        if (tempCount >= 3)
                        {
                            Pieces[x, y - 0].IsChecked = true;
                            Pieces[x, y - 1].IsChecked = true;
                            Pieces[x, y - 2].IsChecked = true;
                            Pieces[x, y - 3].IsChecked = true;
                            return temp;
                        }
                    }
                    else
                    {
                        temp = PieceTypes.Empty;
                        tempCount = 0;
                    }
                }
            }

            // Scan Diagonal: //
            var endpoints = new List<Point[]>() {
                new Point[] { new Point(0,3), new Point(3,0) },
                new Point[] { new Point(0,4), new Point(4,0) },
                new Point[] { new Point(0,5), new Point(5,0) },
                new Point[] { new Point(0,6), new Point(6,0) },
                new Point[] { new Point(0,7), new Point(7,0) },
                new Point[] { new Point(1,7), new Point(7,1) },
                new Point[] { new Point(2,7), new Point(7,2) },
                new Point[] { new Point(3,7), new Point(7,3) },
                new Point[] { new Point(4,7), new Point(7,4) },
            };

            foreach(var point in endpoints)
            {
                var matched = ScanDiagonal(point[0], point[1], matchOn);
                if(matched != PieceTypes.Empty)
                {
                    return matched;
                }
            }

            // Scan Diagonal: \\
            endpoints = new List<Point[]>() {
                new Point[] { new Point(3,7), new Point(0,4) },
                new Point[] { new Point(4,7), new Point(0,3) },
                new Point[] { new Point(5,7), new Point(0,2) },
                new Point[] { new Point(6,7), new Point(0,1) },
                new Point[] { new Point(7,7), new Point(0,0) },
                new Point[] { new Point(7,6), new Point(1,0) },
                new Point[] { new Point(7,5), new Point(2,0) },
                new Point[] { new Point(7,4), new Point(3,0) },
                new Point[] { new Point(7,3), new Point(4,0) },

            };

            foreach (var point in endpoints)
            {
                var matched = ScanDiagonal(point[0], point[1], matchOn);
                if (matched != PieceTypes.Empty)
                {
                    return matched;
                }
            }

            return PieceTypes.Empty;
        }

        private PieceTypes ScanDiagonal(Point start, Point end, List<PieceTypes> matchOn)
        {
            int dx = end.X - start.X;
            int dy = end.Y - start.Y;
            dx = dx < 0 ? -1 : 1;
            dy = dy < 0 ? -1 : 1;

            Point point = start;
            point.X += dx;
            point.Y += dy;

            var temp = PieceTypes.Empty;
            var tempCount = 0;

            while (point != end)
            {
                var x = point.X;
                var y = point.Y;

                if (matchOn.Contains(Pieces[x, y].PieceType) && Pieces[x, y].PieceType == Pieces[x - dx, y - dy].PieceType)
                {
                    temp = Pieces[x, y].PieceType;
                    tempCount++;
                    if (tempCount >= 3)
                    {
                        Pieces[x - dx * 0, y - dy * 0].IsChecked = true;
                        Pieces[x - dx * 1, y - dy * 1].IsChecked = true;
                        Pieces[x - dx * 2, y - dy * 2].IsChecked = true;
                        Pieces[x - dx * 3, y - dy * 3].IsChecked = true;
                        return temp;
                    }
                }
                else
                {
                    temp = PieceTypes.Empty;
                    tempCount = 0;
                }

                point.X += dx;
                point.Y += dy;
            }

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
                        if(Pieces[x,y].Delta.Y > 0.0f) { result = true; }
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
                            result = true;
                        }
                        else if (piece.Delta.Y > 0)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}
