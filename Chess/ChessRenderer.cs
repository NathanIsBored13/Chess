using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chess
{
    class ChessRenderer
    {
        Cell[,] cells = new Cell[8, 8];
        Board source;

        public ChessRenderer(Grid grid, Board source)
        {
            this.source = source;
            for (int i = 0; i < 8; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[x, y] = new Cell(new Point(x, y));
                    grid.Children.Add(cells[x, y]);
                }
            }

            Render();
        }

        public void Render()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (source.GetPiece(x, y) is Piece piece)
                    {
                        cells[x, y].SetImage(Icons.GetImage(piece.GetType(), piece.GetColour()));
                    }
                }
            }
        }
    }
}