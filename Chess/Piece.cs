using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        private readonly bool colour;

        public Piece(bool colour)
        {
            this.colour = colour;
        }

        public bool GetColour() => colour;

        public abstract new Type GetType();
    }
}
