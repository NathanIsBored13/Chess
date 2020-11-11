﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Knight : Piece
    {
        private static readonly Point[] checks = new Point[8]
        {
                new Point(-2, 1),
                new Point(-1, 2),
                new Point(1, 2),
                new Point(2, 1),
                new Point(2, -1),
                new Point(1, -2),
                new Point(-1, -2),
                new Point(-2, -1)
        };

        public Knight(bool colour, Point position) : base(colour, position)
        {

        }

        public override Type GetType()
        {
            return Type.Knight;
        }

        public override PieceMovesMask GetMovesMask(Board board)
        {
            List<Vector> moves = new List<Vector>();
            List<Vector> attacks = new List<Vector>();
            foreach (Point p in checks)
            {
                Point vec = new Point(p.x + GetPoition().x, p.y + GetPoition().y); 
                if (vec.x >= 0 && vec.x < 8 && vec.y >= 0 && vec.y < 8)
                {
                    if (board.GetPiece(vec.x, vec.y) is Piece piece)
                    {
                        if (piece.GetColour() != GetColour())
                        {
                            attacks.Add(new Vector(GetPoition(), vec));
                        }
                    }
                    else
                    {
                        moves.Add(new Vector(GetPoition(), vec));
                    }
                }
            }
            return new PieceMovesMask(attacks.ToArray(), moves.ToArray());
        }
    }
}