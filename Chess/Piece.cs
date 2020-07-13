using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    enum Type
    {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn
    }

    abstract class Piece
    {
        private bool colour;

        public Piece(bool colour)
        {
            this.colour = colour;
        }

        public abstract new Type GetType();
    }
}
