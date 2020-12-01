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

            board.RemovePiece(this);
            foreach (Piece piece in board.GetPieces(!GetColour()))
            {
                Console.Write($"{piece.GetType()} at position {piece.GetPoition()} locks cells ");
                BitBoard b;
                b = piece.GetSeen(board);
                Console.WriteLine(b);
                locked.Merge(b);
            }
            board.AddPiece(this);

            IEnumerable<Point> kingMoves = GetSeen(board).GetAllSet().Where(p => !locked.Get(p) && board.GetPiece(p.x, p.y)?.GetColour() != GetColour());
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

        public override BitBoard GetSeen(Board board)
        {
            BitBoard ret = new BitBoard();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(GetPoition().x + offset.x, GetPoition().y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8)
                {
                    ret.Set(absolute);
                }
            }
            return ret;
        }
    }
}