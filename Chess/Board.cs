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
        Cell[,] board = new Cell[8, 8];

        public Board(UniformGrid DrawTargert)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = new Cell(new Point(x, y));
                    DrawTargert.Children.Add(board[x, y]);
                }
            }
        }

        public void SetupBoard(Piece[,] template)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y].SetPiece(template[x, y]);
                }
            }
        }
    }
}
