using System;
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

        public Knight(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Knight;
        }

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            foreach (Point p in checks)
            {
                Point vec = new Point(p.x + position.x, p.y + position.y); 
                if (vec.x >= 0 && vec.x < 8 && vec.y >= 0 && vec.y < 8)
                {
                    if (board[vec] is Piece piece)
                    {
                        if (piece.GetColour() != GetColour())
                            ret.Add(new PieceMove(new Vector(position, vec), MoveType.Capture));
                    }
                    else
                        ret.Add(new PieceMove(new Vector(position, vec), MoveType.Move));
                }
            }
            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            foreach (Point p in checks)
            {
                Point vec = new Point(p.x + position.x, p.y + position.y);
                if (vec.x >= 0 && vec.x < 8 && vec.y >= 0 && vec.y < 8)
                    seen[vec] = true;
            }
            return seen;
        }
    }
}