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

        public Piece[] this[bool colour]
        {
            get { return (colour ? whitePieces : blackPieces).AsArray(); }
        }

        public Piece GetPiece(int x, int y) => board[x, y];

        public void RemovePiece(Piece p)
        {
            board[p.GetPoition().x, p.GetPoition().y] = null;
            (p.GetColour() ? whitePieces : blackPieces).RemovePiece(p);
        }

        public void AddPiece(Piece p)
        {
            board[p.GetPoition().x, p.GetPoition().y] = p;
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
            List<Vector> moves = new List<Vector>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] is Piece piece && piece.GetColour() == colour)
                    {
                        PieceMovesMask mask = piece.GetMovesMask(this);
                        moves.AddRange(Enumerable.Concat(mask.attacks.GetAllSet(), mask.moves.GetAllSet()).Select(p => new Vector(new Point(x, y), p)));
                    }
                }
            }
            return moves.ToArray();
        }

        public Point? HighlightChecks(bool colour)
        {
            Point? ret = null;
            BitBoard attacks = new BitBoard();
            foreach (Piece p in (colour ? whitePieces : blackPieces).AsArray())
                attacks.Merge(p.GetSeen(this));
            King k = (King)(colour ? blackPieces : whitePieces).GetPieces(Type.King)[0];
            if (attacks.Get(k.GetPoition()))
                ret = k.GetPoition();
            return ret;
        }
    }
}
