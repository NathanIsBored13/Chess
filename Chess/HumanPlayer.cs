using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        public HumanPlayer(bool colour) : base(colour)
        {

        }

        public override Vector Move(Board board)
        {
            return new Vector(Mouse.WaitForInput(), Mouse.WaitForInput());
        }
    }
}
