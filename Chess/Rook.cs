using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Rook : Piece
    {
        public Rook(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Rook;
        }
    }
}
