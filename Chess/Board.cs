using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;

namespace Chess
{
    class Board
    {
        private readonly Piece[,] board = new Piece[8, 8];
        private readonly List<Vector> history = new List<Vector>();
        private PieceHashTable blackPieces;
        private PieceHashTable whitePieces;

        public Board()
        {

        }

        public void SetState(Piece[,] template)
        {
            blackPieces = new PieceHashTable();
            whitePieces = new PieceHashTable();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = template[x, y];
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
            }
        }

        public Piece GetPiece(int x, int y) => board[x, y];

        public Piece[,] GetPieces() => board;

        public Piece[] GetPieces(bool colour, Type type) => colour ? whitePieces.GetPieces(type) : blackPieces.GetPieces(type);

        public Piece[] GetPieces(bool colour) => colour ? whitePieces.AsArray() : blackPieces.AsArray();

        public void Move(Vector vector)
        {
            history.Add(vector);
            if (board[vector.p1.x, vector.p1.y] is Piece p)
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
                        moves.AddRange(mask.attacks.GetAllSet().Select(p => new Vector(new Point(x, y), p)));
                        moves.AddRange(mask.moves.GetAllSet().Select(p => new Vector(new Point(x, y), p)));
                    }
                }
            }
            return moves.ToArray();
        }

        public void HighlightChecks(bool colour, Renderer renderer)
        {
            Vector[] attacks = GetMoves(colour);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[x, y] is King king)
                    {
                        if (king.GetColour() == colour)
                        {
                            renderer.SetHighlight(Highlight.None, new Point(x, y));
                        }
                        else if (attacks.Any(v => v.p2.x == x && v.p2.y == y))
                        {
                            renderer.SetHighlight(Highlight.InCheck, new Point(x, y));
                        }
                    }
                }
            }
        }
    }
}
