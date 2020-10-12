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
        public Vector[] attacks;
        public Vector[] moves;
        public PieceMovesMask(Vector[] attacks, Vector[] moves)
        {
            this.attacks = attacks;
            this.moves = moves;
        }
    }

    abstract class Piece
    {
        private readonly bool colour;

        public Piece(bool colour)
        {
            this.colour = colour;
        }

        public bool GetColour() => colour;

        public abstract new Type GetType();

        public abstract PieceMovesMask GetMovesMask(Board board, Point point);
    }
}
