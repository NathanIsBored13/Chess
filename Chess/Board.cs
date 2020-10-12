﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;

namespace Chess
{
    class Board
    {
        readonly Piece[,] board = new Piece[8, 8];
        readonly List<Vector> history = new List<Vector>();

        public Board() { }

        public void SetState(Piece[,] template)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = template[x, y];
                }
            }
        }

        public Piece GetPiece(int x, int y) => board[x, y];

        public void Move(Vector vector)
        {
            history.Add(vector);
            Console.WriteLine($"[{vector.p1.x}, {vector.p1.y}] => [{vector.p2.x}, {vector.p2.y}]");
            board[vector.p2.x, vector.p2.y] = board[vector.p1.x, vector.p1.y];
            board[vector.p1.x, vector.p1.y] = null;
        }

        public List<Vector> GetHistory() => history;

        public Vector[] GetMoves(bool colour)
        {
            List<Vector> moves = new List<Vector>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] is Piece piece && piece.GetColour() == colour)
                    {
                        PieceMovesMask mask = piece.GetMovesMask(this, new Point(x, y));
                        if (mask.moves != null) moves.AddRange(mask.moves);
                        if (mask.attacks != null) moves.AddRange(mask.attacks);
                    }
                }
            }
            return moves.ToArray();
        }
    }
}
