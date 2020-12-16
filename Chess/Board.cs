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
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                        board[x, y] = template[x, y];
        }

        private Board(Board b)
        {
            history = b.history.Select(v => new Vector(new Point(v.p1.x, v.p1.y), new Point(v.p2.x, v.p2.y))).ToList(); ;
            board = new Piece[8, 8];
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    board[x, y] = (Piece)b.board[x, y]?.Clone();
        }

        public object Clone()
        {
            return new Board(this);
        }

        public Piece this[Point p]
        {
            get { return board[p.x, p.y]; }
        }

        public Point this[Piece p]
        {
            get
            {
                Point ret = new Point(-1, -1);
                for (int x = 0; x < 8; x++)
                    for (int y = 0; y < 8; y++)
                        if (board[x, y] == p)
                            ret = new Point(x, y);
                return ret;
            }
        }

        public void ForEach(Func<Piece, bool> test, Action<Point> method)
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    if (board[x, y] is Piece p && test(p))
                        method(new Point(x, y));
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

        public Piece GetPiece(int x, int y) => board[x, y];

        public void RemovePiece(Point point)
        {
            board[point.x, point.y] = null;
        }

        public void AddPiece(Piece piece, Point point)
        {
            board[point.x, point.y] = piece;
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
            List<Piece> checkers = FindChecks(colour);
            Vector[] ret = null;
            switch (checkers.Count())
            {
                case 0:
                    {
                        BitBoard pins = CalculatePinRays(FindKing(colour), !colour);
                        ret = PsudoGetMoves(colour).Where(x => !pins.Get(x.p1)).ToArray();
                    }
                break;
                case 1:
                    {
                        Point k = FindKing(colour);
                        BitBoard crit = GetCritPath(k, this[checkers[0]]);
                        ret = PsudoGetMoves(colour).Where(x => crit.Get(x.p2) || (x.p1.x == k.x && x.p1.y == k.y)).ToArray();
                    }
                break;
                default:
                    {
                        ForEach(
                        (piece) => piece.GetType() == Type.King && piece.GetColour() == colour,
                        (point) =>
                        {
                            PieceMovesMask mask = this[point].GetMovesMask(this, point);
                            ret = (mask.attacks | mask.moves).GetAllSet().Select(p => new Vector(point, p)).ToArray();
                        });
                    }
                break;
            }
            return ret;
        }

        private BitBoard CalculatePinRays(Point p, bool colour)
        {
            BitBoard bishop = new Bishop(colour).GetMovesMask(this, p).attacks;
            BitBoard rook = new Rook(colour).GetMovesMask(this, p).attacks;
            BitBoard ret = new BitBoard();
            ForEach(
            (piece) => piece.GetType() == Type.Bishop,
            (point) => { ret |= this[point].GetMovesMask(this, point).attacks & bishop; }
            );
            ForEach(
            (piece) => piece.GetType() == Type.Rook,
            (point) => { ret |= this[point].GetMovesMask(this, point).attacks & rook; }
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

        public List<Piece> FindChecks(bool colour)
        {
            List<Piece> ret = new List<Piece>();
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
                ret.AddRange(p.GetSeen(this, pos).GetAllSet().Where(v => this[v] != null && this[v].GetColour() != colour && this[v].GetType() == p.GetType()).Select(v => this[v]));
            return ret;
        }
    }
}
