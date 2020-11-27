using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chess
{
    enum Highlight
    {
        CellSelected = 1,
        MovePossible = 2,
        AttackMovePossible = 4,
        InCheck = 8
    }

    class Renderer
    {
        private readonly Cell[,] cells = new Cell[8, 8];
        private readonly Board source;
        private readonly List<List<Tuple<Point, Highlight>>> deltas = new List<List<Tuple<Point, Highlight>>>();
        private readonly List<int> free = new List<int>();

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

        public int Register()
        {
            int index;
            if (free.Count > 0)
            {
                index = free[0];
                free.RemoveAt(0);
                deltas[index] = new List<Tuple<Point, Highlight>>();
            }
            else
            {
                index = deltas.Count;
                deltas.Add(new List<Tuple<Point, Highlight>>());
            }
            return index;
        }

        public void SetHighlight(int handle, Highlight highlight, Point point)
        {
            deltas[handle].Add(new Tuple<Point, Highlight>(point, highlight));
            cells[point.x, point.y].Highlighted = cells[point.x, point.y].Highlighted | (int)highlight;
        }

        public void SetHighlights(int handle, Tuple<Highlight, Point>[] highlights)
        {
            foreach (Tuple<Highlight, Point> highlight in highlights)
            {
                SetHighlight(handle, highlight.Item1, highlight.Item2);
            }
        }

        public void ResetHighlights(int handle)
        {
            foreach(Tuple<Point, Highlight> pair in deltas[handle])
                cells[pair.Item1.x, pair.Item1.y].Highlighted = cells[pair.Item1.x, pair.Item1.y].Highlighted & (~(int)pair.Item2);
        }
    }
}