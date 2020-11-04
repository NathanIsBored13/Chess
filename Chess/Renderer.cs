using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chess
{
    class Renderer
    {
        Cell[,] cells = new Cell[8, 8];
        Board source;

        public Renderer(Grid grid, Board source)
        {
            this.source = source;
            for (int i = 0; i < 8; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[x, y] = new Cell(new Point(x, y));
                    grid.Children.Add(cells[x, y]);
                }
            }
            Icons.RegisterListener(RenderIcons);
            RenderIcons();
        }

        public void RenderIcons()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (source.GetPiece(x, y) is Piece piece)
                    {
                        cells[x, y].SetImage(Icons.GetImage(piece.GetType(), piece.GetColour()));
                    }
                    else
                    {
                        cells[x, y].SetImage(null);
                    }
                }
            }
        }

        public void SetHighlight(Highlight highlight, Point point)
        {
            cells[point.x, point.y].Highlighted = highlight;
        }

        public void SetHighlights(Tuple<Highlight, Point>[] highlights)
        {
            foreach (Tuple<Highlight, Point> highlight in highlights)
            {
                SetHighlight(highlight.Item1, highlight.Item2);
            }
        }

        public void ResetHighlights()
        {
            foreach (Cell cell in cells)
            {
                cell.Highlighted = Highlight.None;
            }
        }

        public void ResetHighlights(Point[] exceptions)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (!exceptions.Any(p => p.x == x && p.y == y))
                    {
                        cells[x, y].Highlighted = Highlight.None;
                    }
                }
            }
        }
    }
}