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
        private Hashtable pieces = new Hashtable();

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
            return ((List<Piece>)pieces[type]).ToArray();
        }
    }
}
