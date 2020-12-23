using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class BitBoard
    {
        private readonly byte[] board = new byte[8];

        public BitBoard()
        {
            
        }

        public bool this[int x, int y]
        {
            get { return (board[y] & (1 << x)) > 0;}
        }

        public bool this[Point p]
        {
            get { return this[p.x, p.y]; }
        }

        public static BitBoard operator &(BitBoard l, BitBoard r)
        {
            BitBoard ret = new BitBoard();
            for (int i = 0; i < 8; i++)
                ret.board[i] = (byte)(l.board[i] & r.board[i]);
            return ret;
        }

        public static BitBoard operator |(BitBoard l, BitBoard r)
        {
            BitBoard ret = new BitBoard();
            for (int i = 0; i < 8; i++)
                ret.board[i] = (byte)(l.board[i] | r.board[i]);
            return ret;
        }

        public override string ToString() => $"{{{string.Join(", ", GetAllSet().Select(x => x.ToString()))}}}";

        public void Set(Point p)
        {
            board[p.y] = (byte)(board[p.y] | 1 << p.x);
        }

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
