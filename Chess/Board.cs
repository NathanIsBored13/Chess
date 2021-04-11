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
        private PieceMove[] cash = null;

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
            history = b.history.Select(v => new Vector(new Point(v.p1.x, v.p1.y), new Point(v.p2.x, v.p2.y))).ToList(); 
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

        public void Move(PieceMove vector)
        {
            cash = null;
            history.Add(vector.vector);
            ExecuteVector(vector.vector);
            foreach (Vector vec in vector.additionalMoves ?? new Vector[] { })
                ExecuteVector(vec);
        }

        public void ExecuteVector(Vector vector)
        {
            if (vector.p2 != new Point(-1, -1))
                this[vector.p2] = this[vector.p1];
            this[vector.p1] = null;
        }

        public List<Vector> GetHistory() => history;

        public PieceMove[] GetMoves(bool colour) => cash ?? (cash = GetMovesInternal(colour));

        private PieceMove[] GetMovesInternal(bool colour)
        {
            //List<Point> checkers = FindChecks(colour);
            //Vector[] ret = null;
            //Point k = FindKing(colour);
            //switch (checkers.Count())
            //{
            //    case 0:
            //        {
            //            List<Tuple<Point, BitBoard>> pins = CalculatePinRays(FindKing(colour), !colour);
            //            ret = PsudoGetMoves(colour).Where(x => !pins.Any(p => p.Item2[x.p1] && !GetCritPath(k, p.Item1)[x.p2])).ToArray();
            //        }
            //    break;
            //    case 1:
            //        {
            //            BitBoard crit = GetCritPath(k, checkers[0]);
            //            ret = PsudoGetMoves(colour).Where(x => crit[x.p2] || (x.p1.x == k.x && x.p1.y == k.y)).ToArray();
            //        }
            //    break;
            //    default:
            //        {
            //            PieceMovesMask mask = this[k].GetMovesMask(this, k);
            //            ret = (mask.attacks | mask.moves).GetAllSet().Select(p => new Vector(k, p)).ToArray();
            //        }
            //    break;
            //}
            return PsudoGetMoves(colour).Where(x => !WouldCheck(x)).ToArray();
        }

        private bool WouldCheck(PieceMove v)
        {
            Board b = new Board(this);
            b.Move(v);
            return b.FindChecks(this[v.vector.p1].GetColour()).Count() > 0;
        }

        //private List<Tuple<Point, BitBoard>> CalculatePinRays(Point p, bool colour)
        //{
        //    BitBoard seen = new Queen(colour).GetMovesMask(this, p).attacks;
        //    List<Tuple<Point, BitBoard>> ret = new List<Tuple<Point, BitBoard>>();
        //    ForEach(
        //    (piece) => (piece.GetType() == Type.Bishop || piece.GetType() == Type.Rook),
        //    (point) =>
        //    {
        //        Type type = this[point].GetType();
        //        if (point)
        //            ret.Add(new Tuple<Point, BitBoard>(point, this[point].GetMovesMask(this, point).attacks & seen));
        //    });
        //    return ret;
        //}

        //private BitBoard GetCritPath(Point s, Point f)
        //{
        //    BitBoard ret = new BitBoard();

        //    int[] dir = new int[] { Math.Sign(f.x - s.x), Math.Sign(f.y - s.y) };

        //    do
        //    {
        //        s.x += dir[0];
        //        s.y += dir[1];
        //        ret.Set(s);
        //    }
        //    while (s.x != f.x && s.y != f.y);
        //    return ret;
        //}
        
        private List<PieceMove> PsudoGetMoves(bool colour)
        {
            List<PieceMove> moves = new List<PieceMove>();
            ForEach(
            (piece) => piece.GetColour() == colour,
            (point) => moves.AddRange(this[point].GetMovesMask(this, point)));
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
