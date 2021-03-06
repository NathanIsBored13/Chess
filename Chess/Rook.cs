﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Rook : Piece
    {
        public Rook(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Rook;
        }

        public override PieceMovesMask GetMovesMask(Board board, Point position)
        {
            BitBoard moves = new BitBoard();
            BitBoard attacks = new BitBoard();
            int index = 0;
            int[,] dirs = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };
            do
            {
                bool hitPiece = false;
                Point pointer = new Point(position.x + dirs[index, 0], position.y + dirs[index, 1]);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    if (board[pointer] is Piece piece)
                    {
                        hitPiece = true;
                        if (piece.GetColour() != GetColour())
                            attacks.Set(pointer);
                    }
                    else
                        moves.Set(pointer);
                    pointer.x += dirs[index, 0];
                    pointer.y += dirs[index, 1];
                }
                index++;
            } while (index < 4);
            return new PieceMovesMask(attacks, moves);
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            int index = 0;
            int[,] dirs = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };
            do
            {
                bool hitPiece = false;
                Point pointer = new Point(position.x + dirs[index, 0], position.y + dirs[index, 1]);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    seen.Set(pointer);
                    if (board[pointer] is Piece)
                        hitPiece = true;
                    pointer.x += dirs[index, 0];
                    pointer.y += dirs[index, 1];
                }
                index++;
            } while (index < 4);
            return seen;
        }
    }
}
