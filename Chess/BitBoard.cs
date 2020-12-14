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

        public static BitBoard operator &(BitBoard l, BitBoard r)
        {
            BitBoard ret = new BitBoard();
            for (int i = 0; i < 8; i++)
                ret.board[i] = (byte)(l.board[i] & r.board[i]);
            return ret;
        }

        public override string ToString() => $"{{{string.Join(", ", GetAllSet().Select(x => x.ToString()))}}}";

        public void Set(Point p)
        {
            board[p.y] = (byte)(board[p.y] | 1 << p.x);
        }
        
        public void UnSet(Point p)
        {
            board[p.y] = (byte)(board[p.y] & ~(1 << p.x));
        }

        public bool Get(Point p) => (board[p.y] & (1 << p.x)) > 0;

        public bool Get(int x, int y) => (board[y] & (1 << x)) > 0;

        public List<Point> GetAllSet()
        {
            List<Point> ret = new List<Point>();
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    if (Get(x, y))
                        ret.Add(new Point(x, y));
            return ret;
        }

        public void Merge(BitBoard b)
        {
            for (int i = 0; i < 8; i++)
            {
                board[i] |= b.board[i];
            }
        }

        public void Intersection(BitBoard b)
        {
            for (int i = 0; i < 8; i++)
            {
                board[i] &= b.board[i];
            }
        }
    }
}
