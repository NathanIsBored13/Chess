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
        private bool status = true;
        private Point position;

        public Piece(bool colour, Point position)
        {
            this.colour = colour;
            this.position = position;
        }

        public Point GetPoition() => position;

        public void Move(Point p)
        {
            position = p;
        }

        public bool GetColour() => colour;

        public bool IsAlive() => status;

        public void IsDead()
        {
            status = false;
        }

        public abstract new Type GetType();

        public abstract PieceMovesMask GetMovesMask(Board board, Point point);
    }
}
