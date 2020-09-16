using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Chess
{
    class Board
    {
        readonly Piece[,] board = new Piece[8, 8];
        readonly List<Vector> history = new List<Vector>();

        public Board() { }

        public void SetState(Piece[,] template)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = template[x, y];
                }
            }
        }

        public Piece GetPiece(int x, int y) => board[x, y];

        public void Move(Vector vector)
        {
            history.Add(vector);
            Console.WriteLine($"[{vector.p1.x}, {vector.p1.y}] => [{vector.p2.x}, {vector.p2.y}]");
            board[vector.p2.x, vector.p2.y] = board[vector.p1.x, vector.p1.y];
            board[vector.p1.x, vector.p1.y] = null;
        }

        public List<Vector> GetHistory() => history;
    }
}
