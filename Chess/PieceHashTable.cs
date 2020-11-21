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

        public PieceHashTable()
        {

        }

        public Piece[] GetPieces(Type type)
        {
            List<Piece> ret = (List<Piece>)pieces[type];

            return ret.Where(x => x.IsAlive()).ToArray();
        }

        public Piece[] AsArray()
        {
            List<Piece> ret = new List<Piece>();
            foreach (List<Piece> list in pieces.Values)
            {
                ret.AddRange(list);
            }
            return ret.ToArray();
        }

        public void AddPiece(Piece piece)
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

        public void RemovePiece(Piece piece)
        {
            ((List<Piece>)pieces[piece.GetType()]).Remove(piece);
        }
    }
}
