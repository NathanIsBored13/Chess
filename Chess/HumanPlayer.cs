using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        private Mouse mouse;

        public HumanPlayer(bool colour, Board board, Mouse mouse) : base(colour, board)
        {
            this.mouse = mouse;
        }

        public override Vector? Move()
        {
            return null;
        }
    }
}
