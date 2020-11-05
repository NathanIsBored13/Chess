using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class PieceHashTable
    {
        private readonly Hashtable pieces = new Hashtable();

        public PieceHashTable(List<Piece> initial)
        {
            foreach (Piece piece in initial)
            {
                if (pieces.ContainsKey(piece.GetType()))
                {
                    ((List<Piece>)pieces[piece.GetType()]).Add(piece);
                }
                else
                {
                    pieces.Add(piece.GetType(), new List<Piece>());
                    ((List<Piece>)pieces[piece.GetType()]).Add(piece);
                }
            }
        }

        public Piece[] GetPieces(Type type)
        {
            List<Piece> ret = (List<Piece>)pieces[type];

            return ret.Where(x => x.IsAlive()).ToArray();
        }

        public Piece[] AsArray()
        {
            Piece[] ret = new Piece[pieces.Count];
            pieces.CopyTo(ret, 0);
            return ret;
        }

        public void AddPiece(Piece piece)
        {
            ((List<Piece>)pieces[piece.GetType()]).Add(piece);
        }
    }
}
