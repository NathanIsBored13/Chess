using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        public HumanPlayer(bool colour, Board board) : base(colour, board)
        {

        }

        public override Vector Move()
        {
            return new Vector(Mouse.WaitForInput(), Mouse.WaitForInput());
        }
    }
}
