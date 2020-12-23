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

        public King(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.King;
        }

        public override PieceMovesMask GetMovesMask(Board board, Point position)
        {
            BitBoard moves = new BitBoard();
            BitBoard attacks = new BitBoard();
            BitBoard locked = new BitBoard();

            board[position] = null;

            board.ForEach(
            (Piece p) => p.GetColour() != GetColour(),
            (Point p) => { locked |= board[p].GetSeen(board, p); }
            );

            board[position] = this;

            IEnumerable<Point> kingMoves = GetSeen(board, position).GetAllSet().Where(p => !locked[p] && board[p]?.GetColour() != GetColour());
            foreach (Point p in kingMoves)
            {
                if (board[p] == null)
                    moves.Set(p);
                else
                    attacks.Set(p);
            }
            return new PieceMovesMask(attacks, moves);
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard ret = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(position.x + offset.x, position.y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8)
                    ret.Set(absolute);
            }
            return ret;
        }
    }
}