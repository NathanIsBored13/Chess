using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class King : Piece
    {
        public King(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.King;
        }
    }
}
