using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chess
{
    enum Type
    {
        King,
        Queen,
        Rook,
        Knight,
        Bishop,
        Pawn
    }

    struct PieceMovesMask
    {
        public BitBoard attacks;
        public BitBoard moves;
        public PieceMovesMask(BitBoard attacks, BitBoard moves)
        {
            this.attacks = attacks;
            this.moves = moves;
        }
    }

    abstract class Piece : ICloneable
    {
        private readonly bool colour;

        public Piece(bool colour)
        {
            this.colour = colour;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool GetColour() => colour;

        public abstract new Type GetType();

        public abstract PieceMovesMask GetMovesMask(Board board, Point position);

        public abstract BitBoard GetSeen(Board board, Point position);
    }
}
