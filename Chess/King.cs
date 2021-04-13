using Extentions;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    class King : Piece
    {
        private struct CastelingMask
        {
            public int kingFinal, rookInitial, rookFinal;
            public CastelingMask(int kingFinal, int rookInitial, int rookFinal)
            {
                this.kingFinal = kingFinal;
                this.rookInitial = rookInitial;
                this.rookFinal = rookFinal;
            }
        }

        private static readonly Point[] mask = new Point[]
        {
            new Point(1, 1),
            new Point(0, 1),
            new Point(-1, 1),
            new Point(-1, 0),
            new Point(-1, -1),
            new Point(0, -1),
            new Point(1, -1),
            new Point(1, 0),
        };

        private static readonly CastelingMask[] castelingMasks = new CastelingMask[]
        {
            new CastelingMask(1, 0, 2),
            new CastelingMask(5, 7, 4)
        };

        public King(bool colour) : base(colour)
        {

        }

        public override Type GetType() => Type.King;

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            BitBoard locked = new BitBoard();

            board[position] = null;

            board.ForEach(
            (Piece piece) => piece.GetColour() != GetColour(),
            (Point point) => locked |= board[point].GetSeen(board, point));

            board[position] = this;

            IEnumerable<Point> kingMoves = GetSeen(board, position).GetAllSet().Where(kingMove => !locked[kingMove] && board[kingMove]?.GetColour() != GetColour());
            foreach (Point kingMove in kingMoves)
                ret.Add(new PieceMove(new Vector(position, kingMove), board[kingMove] == null? MoveType.Move : MoveType.Capture));

            if (!board.GetHistory().Any(historicalMove => historicalMove.p2 == position))
                foreach (CastelingMask mask in castelingMasks)
                    if (!board.GetHistory().Any(historicalMove => historicalMove.p1 == new Point(mask.rookInitial, position.y)) && Extentions.Extentions.Enumerate(position.x, mask.kingFinal).All(kingMoveX => board.FindAttacks(GetColour(), new Point(kingMoveX, position.y)).Count() == 0) && Extentions.Extentions.Enumerate(mask.rookInitial, position.x).SubArray(1, 1).All(movedThrough => board[movedThrough, position.y] == null))
                        ret.Add(new PieceMove(new Vector(position, new Point(mask.kingFinal, position.y)), new Vector[] { new Vector(new Point(mask.rookInitial, position.y), new Point(mask.rookFinal, position.y)) }, MoveType.Move));

            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard ret = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolutePosition = new Point(position.x + offset.x, position.y + offset.y);
                if (absolutePosition.x >= 0 && absolutePosition.x < 8 && absolutePosition.y >= 0 && absolutePosition.y < 8)
                    ret[absolutePosition] = true;
            }
            return ret;
        }
    }
}