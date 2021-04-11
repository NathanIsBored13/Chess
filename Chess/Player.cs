using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Chess
{
    public enum PlayerType
    {
        [Description("Human (Local)")]
        HumanPlayerLOCAL,
        [Description("AI (Easy)")]
        AIPlayerEASY,
        [Description("AI (Medium)")]
        AIPlayerMEDIUM,
        [Description("AI (Hard)")]
        AIPlayerHARD,
    }

    abstract class Player
    {
        readonly bool colour;
        public Player(bool colour)
        {
            this.colour = colour;
        }

        public abstract PieceMove Move(Board board);

        public bool GetColour() => colour;

        public static Player MakePlayer(PlayerType type, bool colour, Renderer renderer, Mouse mouse)
        {
            Player ret = null;
            switch (type)
            {
                case PlayerType.HumanPlayerLOCAL:
                    ret = new HumanPlayer(colour, renderer, mouse);
                break;
            }
            return ret;
        }
    }
}
