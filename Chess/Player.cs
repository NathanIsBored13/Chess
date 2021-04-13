using System.ComponentModel;

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
            switch (type)
            {
                case PlayerType.HumanPlayerLOCAL:
                    return new HumanPlayer(colour, renderer, mouse);
            }
            return null;
        }
    }
}
