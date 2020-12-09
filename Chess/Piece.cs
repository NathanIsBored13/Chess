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
        private Point position;

        public Piece(bool colour, Point position)
        {
            this.colour = colour;
            this.position = position;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public Point GetPoition() => position;

        public void Move(Point p)
        {
            position = p;
        }

        public bool GetColour() => colour;

        public abstract new Type GetType();

        public abstract PieceMovesMask GetMovesMask(Board board);

        public abstract BitBoard GetSeen(Board board);
    }
}
