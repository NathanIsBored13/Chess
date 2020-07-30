using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Chess
{
    abstract class Player
    {
        readonly bool colour;
        Board board;
        public Player(bool colour, Board board)
        {
            this.colour = colour;
            this.board = board;
        }

        public abstract Vector? Move();

        public bool GetColour() => colour;

        public Board GetBoard() => board;
    }
}
