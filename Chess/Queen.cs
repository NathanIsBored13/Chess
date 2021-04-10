using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Queen : Piece
    {
        private readonly Point[] dirs = { new Point(0, 1), new Point(1, 0), new Point(-1, 0), new Point(0, -1), new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1) };
        public Queen(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Queen;
        }

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            foreach (Point dir in dirs)
            {
                bool hitPiece = false;
                Point pointer = new Point(position.x + dir.x, position.y + dir.y);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    if (board[pointer] is Piece piece)
                    {
                        hitPiece = true;
                        if (piece.GetColour() != GetColour())
                            ret.Add(new PieceMove(new Vector(position, pointer), MoveType.Capture));
                    }
                    else
                        ret.Add(new PieceMove(new Vector(position, pointer), MoveType.Move));
                    pointer.x += dir.x;
                    pointer.y += dir.y;
                }
            }
            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            foreach (Point dir in dirs)
            {
                bool hitPiece = false;
                Point pointer = new Point(position.x + dir.x, position.y + dir.y);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    seen[pointer] = true;
                    if (board[pointer.x, pointer.y] is Piece)
                        hitPiece = true;
                    pointer.x += dir.x;
                    pointer.y += dir.y;
                }
            }
            return seen;
        }
    }
}
