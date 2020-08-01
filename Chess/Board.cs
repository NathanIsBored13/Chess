﻿using System;
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

        public Board(UniformGrid DrawTargert, Piece[,] template, Mouse mouse)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    board[x, y] = new Cell(new Point(x, y), template[x, y], mouse);
                    DrawTargert.Children.Add(board[x, y]);
                }
            }
        }
    }
}
