using Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        private static readonly Point[] mask = new Point[8]
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

        public override Type GetType()
        {
            return Type.King;
        }

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            BitBoard locked = new BitBoard();

            board[position] = null;

            board.ForEach(
            (Piece p) => p.GetColour() != GetColour(),
            (Point p) => { locked |= board[p].GetSeen(board, p); }
            );

            board[position] = this;

            IEnumerable<Point> kingMoves = GetSeen(board, position).GetAllSet().Where(p => !locked[p] && board[p]?.GetColour() != GetColour());
            foreach (Point p in kingMoves)
                ret.Add(new PieceMove(new Vector(position, p), board[p] == null? MoveType.Move : MoveType.Capture));

            if (!board.GetHistory().Any(x => x.p2 == position))
                foreach (CastelingMask mask in castelingMasks)
                    if (!board.GetHistory().Any(x => x.p1 == new Point(mask.rookInitial, position.y)) && Enumerate(position.x, mask.kingFinal).All(x => board.FindAttacks(GetColour(), new Point(x, position.y)).Count() == 0) && Enumerate(mask.rookInitial, position.x).SubArray(1, 1).All(x => board[x, position.y] == null))
                        ret.Add(new PieceMove(new Vector(position, new Point(mask.kingFinal, position.y)), new Vector[] { new Vector(new Point(mask.rookInitial, position.y), new Point(mask.rookFinal, position.y)) }, MoveType.Move));

            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard ret = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(position.x + offset.x, position.y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8)
                    ret[absolute] = true;
            }
            return ret;
        }
        private IEnumerable<int> Enumerate(int start, int stop) => Enumerable.Range(Math.Min(start, stop), Math.Abs(start - stop));
    }
}