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
        private readonly PieceHashTable blackPieces;
        private readonly PieceHashTable whitePieces;

        public Board(Piece[,] template)
        {
            blackPieces = new PieceHashTable();
            whitePieces = new PieceHashTable();
            history = new List<Vector>();
            board = new Piece[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = template[x, y];
                    PushToTable(x, y);
                }
            }
        }

        private Board(Board b)
        {
            blackPieces = new PieceHashTable();
            whitePieces = new PieceHashTable();
            history = b.history.Select(v => new Vector(new Point(v.p1.x, v.p1.y), new Point(v.p2.x, v.p2.y))).ToList(); ;
            board = new Piece[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = (Piece)b.board[x, y]?.Clone();
                    PushToTable(x, y);
                }
            }
        }

        public object Clone()
        {
            return new Board(this);
        }

        private void PushToTable(int x, int y)
        {
            switch (board[x, y]?.GetColour())
            {
                case true:
                    whitePieces.AddPiece(board[x, y]);
                    break;
                case false:
                    blackPieces.AddPiece(board[x, y]);
                    break;
            }
        }

        public Piece this[Point p]
        {
            get { return board[p.x, p.y]; }
        }

        public Piece[] this[bool colour, Type t]
        {
            get { return (colour ? whitePieces : blackPieces).GetPieces(t); }
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

        public Piece[] this[bool colour]
        {
            get { return (colour ? whitePieces : blackPieces).AsArray(); }
        }

        public Piece GetPiece(int x, int y) => board[x, y];

        public void RemovePiece(Piece p)
        {
            board[p.GetPosition().x, p.GetPosition().y] = null;
            (p.GetColour() ? whitePieces : blackPieces).RemovePiece(p);
        }

        public void AddPiece(Piece p)
        {
            board[p.GetPosition().x, p.GetPosition().y] = p;
            (p.GetColour() ? whitePieces : blackPieces).AddPiece(p);
        }

        public Piece[,] GetPieces() => board;

        public Piece[] GetPieces(bool colour, Type type) => colour ? whitePieces.GetPieces(type) : blackPieces.GetPieces(type);

        public Piece[] GetPieces(bool colour) => colour ? whitePieces.AsArray() : blackPieces.AsArray();

        public void Move(Vector vector)
        {
            history.Add(vector);
            if (board[vector.p2.x, vector.p2.y] is Piece p)
                (p.GetColour() ? whitePieces : blackPieces).RemovePiece(p);
            board[vector.p2.x, vector.p2.y] = board[vector.p1.x, vector.p1.y];
            board[vector.p1.x, vector.p1.y].Move(vector.p2);
            board[vector.p1.x, vector.p1.y] = null;
        }

        public List<Vector> GetHistory() => history;

        public Vector[] GetMoves(bool colour)
        {
            Console.WriteLine("\n\n---Move Begin---");
            Vector[] ret = new Vector[0];
            List<Piece> checkers = FindChecks(colour);
            switch (checkers.Count())
            {
                case 0:
                    {
                        BitBoard pins = CalculatePinRays(this[colour, Type.King][0].GetPosition(), !colour);
                        ret = PsudoGetMoves(colour).Where(x => !pins.Get(x.p1)).ToArray();
                    }
                break;
                case 1:
                    {
                        Point k = this[colour, Type.King][0].GetPosition();
                        BitBoard crit = GetCritPath(k, checkers[0].GetPosition());
                        ret = PsudoGetMoves(colour).Where(x => crit.Get(x.p2) || (x.p1.x == k.x && x.p1.y == k.y)).ToArray();
                    }
                break;
                case 2:
                    {
                        Piece k = this[colour, Type.King][0];
                        PieceMovesMask mask = k.GetMovesMask(this);
                        ret = Enumerable.Concat(mask.moves.GetAllSet(), mask.attacks.GetAllSet()).Select(p => new Vector(k.GetPosition(), p)).ToArray();
                    }
                break;
            }
            return ret;
        }

        private BitBoard CalculatePinRays(Point p, bool colour)
        {
            BitBoard bishop = new Bishop(colour, p).GetMovesMask(this).attacks;
            BitBoard rook = new Rook(colour, p).GetMovesMask(this).attacks;
            BitBoard ret = new BitBoard();
            foreach (Piece piece in this[colour, Type.Bishop])
                ret.Merge(piece.GetMovesMask(this).attacks & bishop);
            foreach (Piece piece in this[colour, Type.Rook])
                ret.Merge(piece.GetMovesMask(this).attacks & rook);
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
            foreach(Piece piece in this[colour])
            {
                PieceMovesMask mask = piece.GetMovesMask(this);
                moves.AddRange(Enumerable.Concat(mask.attacks.GetAllSet(), mask.moves.GetAllSet()).Select(p => new Vector(piece.GetPosition(), p)));
            }
            return moves;
        }

        public List<Piece> FindChecks(bool colour)
        {
            List<Piece> ret = new List<Piece>();
            Point pos = GetPieces(colour, Type.King)[0].GetPosition();
            Piece[] pieceTemplates = new Piece[6]
            {
                new King(colour, pos),
                new Queen(colour, pos),
                new Rook(colour, pos),
                new Knight(colour, pos),
                new Bishop(colour, pos),
                new Pawn(colour, pos)
            };
            foreach (Piece p in pieceTemplates)
                ret.AddRange(p.GetSeen(this).GetAllSet().Where(v => this[v] != null && this[v].GetColour() != colour && this[v].GetType() == p.GetType()).Select(v => this[v]));
            return ret;
        }
    }
}
