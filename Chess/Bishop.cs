using System.Collections.Generic;

namespace Chess
{
    class Bishop : SliderPiece
    {
        private readonly Point[] moveDirections = { new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1) };

        public Bishop(bool colour) : base(colour)
        {

        }

        public override Type GetType() => Type.Bishop;

        public override List<PieceMove> GetMovesMask(Board board, Point position) => GetMovesMask(board, position, moveDirections);

        public override BitBoard GetSeen(Board board, Point position) => GetSeen(board, position, moveDirections);
    }
}
