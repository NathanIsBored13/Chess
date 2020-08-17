using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Pawn : Piece
    {
        public Pawn(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Pawn;
        }
    }
}
