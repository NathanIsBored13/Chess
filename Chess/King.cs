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
        private static Point[] mask = new Point[8]
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
            List<Vector> moves = new List<Vector>();
            List<Vector> attacks = new List<Vector>();
            List<Point> locked = new List<Point>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board.GetPiece(x, y) is Piece piece && piece.GetColour() != GetColour())
                    {
                        if (piece is King)
                        {
                            locked.AddRange(GetPsudoMoveMask(board, new Point(x, y)));
                        }
                        else
                        {
                            foreach (Vector v in piece.GetMovesMask(board, new Point(x, y)).moves)
                            {
                                locked.Add(v.p2);
                            }
                        }
                    }
                }
            }

            List<Point> kingMoves = GetPsudoMoveMask(board, position).Where(a => locked.All(b => a.x != b.x || a.y != b.y)).ToList();

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

        private List<Point> GetPsudoMoveMask(Board board, Point position)
        {
            List<Point> ret = new List<Point>();
            foreach (Point offset in mask)
            {
                Point absolute = new Point(position.x + offset.x, position.y + offset.y);
                if (absolute.x >= 0 && absolute.x < 8 && absolute.y >= 0 && absolute.y < 8 && board.GetPiece(absolute.x, absolute.y)?.GetColour() != GetColour())
                {
                    ret.Add(absolute);
                }
            }
            return ret;
        }
    }
}