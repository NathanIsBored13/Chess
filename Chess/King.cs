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

        public King(bool colour, Point position) : base(colour, position)
        {

        }

        public override Type GetType()
        {
            return Type.King;
        }

        public override PieceMovesMask GetMovesMask(Board board)
        {
            BitBoard moves = new BitBoard();
            BitBoard attacks = new BitBoard();
            BitBoard locked = new BitBoard();
            foreach (Piece piece in board.GetPieces(!GetColour()))
            {
                if (piece is King king)
                {
                    locked.Merge(GetPsudoMoveMask(board, king.GetPoition(), king.GetColour()));
                }
                else if (piece is Pawn pawn)
                {
                    locked.Set(new Point(pawn.GetPoition().x + 1, pawn.GetPoition().y + (pawn.GetColour() ? -1 : 1)));
                    locked.Set(new Point(pawn.GetPoition().x - 1, pawn.GetPoition().y + (pawn.GetColour() ? -1 : 1)));
                }
                else
                {
                    locked.Merge(piece.GetMovesMask(board).moves);
                }
            }

            IEnumerable<Point> kingMoves = GetPsudoMoveMask(board, GetPoition(), GetColour()).GetAllSet().Where(p => !locked.Get(p));
            foreach (Point p in kingMoves)
            {
                if (board.GetPiece(p.x, p.y) == null)
                {
                    moves.Set(p);
                }
                else
                {
                    attacks.Set(p);
                }
            }
            return new PieceMovesMask(attacks, moves);
        }

        private static BitBoard GetPsudoMoveMask(Board board, Point position, bool colour)
        {
            BitBoard ret = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(position.x + offset.x, position.y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8 && board.GetPiece(absolute.x, absolute.y)?.GetColour() != colour)
                {
                    ret.Set(absolute);
                }
            }
            return ret;
        }
    }
}