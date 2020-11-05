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

        public override PieceMovesMask GetMovesMask(Board board, Point position)
        {
            List<Vector> moves = new List<Vector>();
            List<Vector> attacks = new List<Vector>();
            List<Point> locked = new List<Point>();
            foreach (Piece piece in board.GetPieces(!GetColour()))
            {
                if (piece is King king)
                {
                    locked.AddRange(GetPsudoMoveMask(board, king.GetPoition(), king.GetColour()));
                }
                else
                {
                    foreach (Vector v in piece.GetMovesMask(board, piece.GetPoition()).moves)
                    {
                        locked.Add(v.p2);
                    }
                }
            }

            List<Point> kingMoves = GetPsudoMoveMask(board, position, GetColour()).Where(a => locked.All(b => a.x != b.x || a.y != b.y)).ToList();

            foreach (Point p in kingMoves)
            {
                if (board.GetPiece(p.x, p.y) == null)
                {
                    moves.Add(new Vector(position, p));
                }
                else
                {
                    attacks.Add(new Vector(position, p));
                }
            }
            return new PieceMovesMask(attacks.ToArray(), moves.ToArray());
        }

        private static List<Point> GetPsudoMoveMask(Board board, Point position, bool colour)
        {
            List<Point> ret = new List<Point>();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(position.x + offset.x, position.y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8 && board.GetPiece(absolute.x, absolute.y)?.GetColour() != colour)
                {
                    ret.Add(absolute);
                }
            }
            return ret;
        }
    }
}