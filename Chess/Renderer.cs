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
        none,
        CellSelected,
        MovePossible,
        AttackMovePossible,
        InCheck
    }

    class Renderer
    {
        private readonly Cell[,] cells = new Cell[8, 8];
        private Board source;
        private readonly List<List<Tuple<Point, Highlight>>> deltas = new List<List<Tuple<Point, Highlight>>>();
        private readonly List<int> free = new List<int>();

        public Renderer(Grid grid, Mouse mouse)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            grid.Children.Clear();
            for (int i = 0; i < 8; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Board.ForEach(
            (p) =>
            {
                cells[p.x, p.y] = new Cell(new Point(p.x, p.y), mouse);
                grid.Children.Add(cells[p.x, p.y]);
            });
            Icons.RegisterListener(RenderIcons);
        }

        public void SetSource(Board source)
        {
            this.source = source;
        }

        public void RenderIcons()
        {
            Board.ForEach(
            (point) =>
            {
                if (source[point] is Piece piece)
                    cells[point.x, point.y].SetImage(Icons.GetImage(piece.GetType(), piece.GetColour()));
                else
                    cells[point.x, point.y].SetImage(null);
            });
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
                SetHighlight(handle, highlight.Item1, highlight.Item2);
        }

        public void ResetHighlights(int handle)
        {
            foreach(Tuple<Point, Highlight> pair in deltas[handle])
                cells[pair.Item1.x, pair.Item1.y].Highlighted = cells[pair.Item1.x, pair.Item1.y].Highlighted & (~(int)pair.Item2);
        }
    }
}