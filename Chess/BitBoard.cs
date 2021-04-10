using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class BitBoard
    {
        private readonly bool[,] board = new bool[8, 8];

        public BitBoard()
        {
            
        }

        public bool this[int x, int y]
        {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }

        public bool this[Point p]
        {
            get { return board[p.x, p.y]; }
            set { board[p.x, p.y] = value; }
        }

        public static BitBoard operator &(BitBoard l, BitBoard r)
        {
            BitBoard ret = new BitBoard();
            Board.ForEach(point => { if (l[point] && r[point]) ret[point] = true; });
            return ret;
        }

        public static BitBoard operator |(BitBoard l, BitBoard r)
        {
            BitBoard ret = new BitBoard();
            Board.ForEach(point => { if (l[point] || r[point]) ret[point] = true; });
            return ret;
        }

        public override string ToString() => $"{{{string.Join(", ", GetAllSet().Select(x => x.ToString()))}}}";

        public List<Point> GetAllSet()
        {
            List<Point> ret = new List<Point>();
            Board.ForEach(
            (Point p) =>
            {
                if (this[p])
                    ret.Add(p);
            });
            return ret;
        }
    }
}
