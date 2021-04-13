using System.Collections.Generic;

namespace Chess
{
    abstract class SliderPiece : Piece
    {
        public SliderPiece(bool colour) : base(colour) { }

        public List<PieceMove> GetMovesMask(Board board, Point position, Point[] dirs)
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

        public BitBoard GetSeen(Board board, Point position, Point[] dirs)
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
