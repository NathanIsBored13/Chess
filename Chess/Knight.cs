using System.Collections.Generic;

namespace Chess
{
    class Knight : Piece
    {
        private static readonly Point[] mask = new Point[]
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

        public override Type GetType() => Type.Knight;

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            foreach (Point offset in mask)
            {
                Point absolutePosition = new Point(offset.x + position.x, offset.y + position.y); 
                if (absolutePosition.x >= 0 && absolutePosition.x < 8 && absolutePosition.y >= 0 && absolutePosition.y < 8)
                {
                    if (board[absolutePosition] is Piece piece)
                    {
                        if (piece.GetColour() != GetColour())
                            ret.Add(new PieceMove(new Vector(position, absolutePosition), MoveType.Capture));
                    }
                    else
                        ret.Add(new PieceMove(new Vector(position, absolutePosition), MoveType.Move));
                }
            }
            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolutePosition = new Point(offset.x + position.x, offset.y + position.y);
                if (absolutePosition.x >= 0 && absolutePosition.x < 8 && absolutePosition.y >= 0 && absolutePosition.y < 8)
                    seen[absolutePosition] = true;
            }
            return seen;
        }
    }
}