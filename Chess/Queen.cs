using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Queen : Piece
    {
        public Queen(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Queen;
        }
    }
}
