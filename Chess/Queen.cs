using System.Collections.Generic;

namespace Chess
{
    class Queen : SliderPiece
    {
        private readonly Point[] dirs = { new Point(0, 1), new Point(1, 0), new Point(-1, 0), new Point(0, -1), new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1) };
        
        public Queen(bool colour) : base(colour)
        {

        }

        public override Type GetType() => Type.Queen;

        public override List<PieceMove> GetMovesMask(Board board, Point position) => GetMovesMask(board, position, dirs);

        public override BitBoard GetSeen(Board board, Point position) => GetSeen(board, position, dirs);
    }
}
