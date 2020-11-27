using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Chess
{
    enum PlayerType
    {
        HumanPlayer
    }

    abstract class Player
    {
        readonly bool colour;
        public Player(bool colour)
        {
            this.colour = colour;
        }

        public abstract Vector Move(Board board);

        public bool GetColour() => colour;

        public static Player MakePlayer(PlayerType type, bool colour, Renderer renderer)
        {
            Player ret = null;
            switch (type)
            {
                case PlayerType.HumanPlayer:
                    ret = new HumanPlayer(colour, renderer);
                break;
            }
            return ret;
        }
    }
}
