using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;

namespace Chess
{
    class Board : ICloneable
    {
        private readonly Piece[,] board;
        private readonly List<Vector> history;

        public Board(Piece[,] template)
        {
            history = new List<Vector>();
            board = new Piece[8, 8];
            ForEach(
            (Point p) =>
            {
                this[p] = template[p.x, p.y];
            });
        }

        private Board(Board b)
        {
            history = b.history.Select(v => new Vector(new Point(v.p1.x, v.p1.y), new Point(v.p2.x, v.p2.y))).ToList(); ;
            board = new Piece[8, 8];
            ForEach(
            (Point p) =>
            {
                this[p] = (Piece)b.board[p.x, p.y]?.Clone();
            });
        }

        public object Clone()
        {
            return new Board(this);
        }

        public Piece this[int x, int y]
        {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }

        public Piece this[Point p]
        {
            get { return this[p.x, p.y]; }
            set { this[p.x, p.y] = value; }
        }

        public static void ForEach(Action<Point> method)
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    method(new Point(x, y));
        }

        public void ForEach(Func<Piece, bool> test, Action<Point> method)
        {
            ForEach(
            (Point p) =>
            {
                if (this[p] is Piece piece && test(piece))
                    method(p);
            });
        }

        public Point FindKing(bool colour)
        {
            Point ret = new Point(-1, -1);
            ForEach(
            (piece) => piece.GetType() == Type.King && piece.GetColour() == colour,
            (point) => ret = point
            );
            return ret;
        }

        public Piece[,] GetPieces() => board;

        public void Move(Vector vector)
        {
            history.Add(vector);
            board[vector.p2.x, vector.p2.y] = board[vector.p1.x, vector.p1.y];
            board[vector.p1.x, vector.p1.y] = null;
        }

        public List<Vector> GetHistory() => history;

        public Vector[] GetMoves(bool colour)
        {
            List<Point> checkers = FindChecks(colour);
            Vector[] ret = null;
            switch (checkers.Count())
            {
                case 0:
                    {
                        BitBoard pins = CalculatePinRays(FindKing(colour), !colour);
                        ret = PsudoGetMoves(colour).Where(x => !pins[x.p1]).ToArray();
                    }
                break;
                case 1:
                    {
                        Point k = FindKing(colour);
                        BitBoard crit = GetCritPath(k, checkers[0]);
                        ret = PsudoGetMoves(colour).Where(x => crit[x.p2] || (x.p1.x == k.x && x.p1.y == k.y)).ToArray();
                    }
                break;
                default:
                    {
                        Point pos = FindKing(colour);
                        PieceMovesMask mask = this[pos].GetMovesMask(this, pos);
                        ret = (mask.attacks | mask.moves).GetAllSet().Select(p => new Vector(pos, p)).ToArray();
                    }
                break;
            }
            return ret;
        }

        private BitBoard CalculatePinRays(Point p, bool colour)
        {
            BitBoard seen = new Queen(colour).GetMovesMask(this, p).attacks;
            BitBoard ret = new BitBoard();
            ForEach(
            (piece) => piece.GetType() == Type.Bishop || piece.GetType() == Type.Rook,
            (point) => { ret |= this[point].GetMovesMask(this, point).attacks & seen; }
            );
            return ret;
        }

        private BitBoard GetCritPath(Point s, Point f)
        {
            BitBoard ret = new BitBoard();

            int[] dir = new int[] { Math.Sign(f.x - s.x), Math.Sign(f.y - s.y) };

            do
            {
                s.x += dir[0];
                s.y += dir[1];
                ret.Set(s);
            }
            while (s.x != f.x && s.y != f.y);
            return ret;
        }

        private List<Vector> PsudoGetMoves(bool colour)
        {
            List<Vector> moves = new List<Vector>();
            ForEach(
            (piece) => piece.GetColour() == colour,
            (point) =>
            {
                PieceMovesMask mask = this[point].GetMovesMask(this, point);
                moves.AddRange((mask.attacks | mask.moves).GetAllSet().Select(p => new Vector(point, p)));
            });
            return moves;
        }

        public List<Point> FindChecks(bool colour)
        {
            List<Point> ret = new List<Point>();
            Point pos = FindKing(colour);
            Piece[] pieceTemplates = new Piece[6]
            {
                new King(colour),
                new Queen(colour),
                new Rook(colour),
                new Knight(colour),
                new Bishop(colour),
                new Pawn(colour)
            };
            foreach (Piece p in pieceTemplates)
                ret.AddRange(p.GetSeen(this, pos).GetAllSet().Where(v => this[v] != null && this[v].GetColour() != colour && this[v].GetType() == p.GetType()));
            return ret;
        }
    }
}
