using System;
using System.Collections.Generic;
using System.Linq;

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
            get { return board[p.x, p.y]; }
            set { board[p.x, p.y] = value; }
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
            Point kingPos = new Point(-1, -1);
            ForEach(
            (piece) => piece.GetType() == Type.King && piece.GetColour() == colour,
            (point) => kingPos = point
            );
            return kingPos;
        }

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
            if (vector.p1.y < 0)
                this[vector.p2] = Piece.Initialise((Type)vector.p1.x, vector.p1.y == -1);
            else
            {
                if (vector.p2.y != -1)
                    this[vector.p2] = this[vector.p1];
                this[vector.p1] = null;
            }
        }

        public List<Vector> GetHistory() => history;

        public PieceMove[] GetMoves(bool colour) => cash ?? (cash = GetMovesInternal(colour));

        private PieceMove[] GetMovesInternal(bool colour) => PsudoGetMoves(colour).Where(x => !WouldCheck(x)).ToArray();

        private bool WouldCheck(PieceMove moveVector)
        {
            Board testBoard = Clone() as Board;
            testBoard.Move(moveVector);
            return testBoard.FindChecks(this[moveVector.vector.p1].GetColour()).Count() > 0;
        }
        
        private List<PieceMove> PsudoGetMoves(bool colour)
        {
            List<PieceMove> moves = new List<PieceMove>();
            ForEach(
            (piece) => piece.GetColour() == colour,
            (point) => moves.AddRange(this[point].GetMovesMask(this, point)));
            return moves;
        }

        public List<Point> FindChecks(bool colour) => FindAttacks(colour, FindKing(colour));

        public List<Point> FindAttacks(bool colour, Point position)
        {
            List<Point> attackList = new List<Point>();
            Piece[] pieceTemplates = new Piece[]
            {
                new King(colour),
                new Queen(colour),
                new Rook(colour),
                new Knight(colour),
                new Bishop(colour),
                new Pawn(colour)
            };
            Console.WriteLine($"pawn at {position} sees: {string.Join(", ", pieceTemplates.Last().GetSeen(this, position).GetAllSet().Select(p => p.ToString()))}");
            foreach (Piece piece in pieceTemplates)
                attackList.AddRange(piece.GetSeen(this, position).GetAllSet().Where(attack => !this[attack]?.GetColour() == colour && this[attack].GetType() == piece.GetType()));
            return attackList;
        }
    }
}
