using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Pawn : Piece
    {
        private readonly int dir;

        public Pawn(bool colour) : base(colour)
        {
            dir = colour ? -1 : 1;
        }

        public override Type GetType()
        {
            return Type.Pawn;
        }

        public override PieceMovesMask GetMovesMask(Board board, Point position)
        {
            BitBoard moves = new BitBoard();
            if (position.y > 0 && position.y < 8 && board.GetPiece(position.x, position.y + dir) == null)
            {
                moves.Set(new Point(position.x, position.y + dir));
                if (position.y == 1 || position.y == 6 && board.GetPiece(position.x, position.y + 2 * dir) == null)
                {
                    moves.Set(new Point(position.x, position.y + 2 * dir));
                }
            }
            BitBoard attacks = new BitBoard();
            if (position.y > 0 && position.y < 8 && position.x < 7 && board.GetPiece(position.x + 1, position.y + dir) is Piece p1 && p1.GetColour() != GetColour())
            {
                attacks.Set(new Point(position.x + 1, position.y + dir));
            }
            if (position.y > 0 && position.y < 8 && position.x > 1 && board.GetPiece(position.x - 1, position.y + dir) is Piece p2 && p2.GetColour() != GetColour())
            {
                attacks.Set(new Point(position.x - 1, position.y + dir));
            }
            return new PieceMovesMask(attacks, moves);
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            if (GetColour() ? (position.y > 0) : (position.y < 7) && position.y < 7 && position.x > 0)
                seen.Set(new Point(position.x - 1, position.y + dir));
            if (GetColour() ? (position.y > 0) : (position.y < 7) && position.y > 0 && position.x > 0)
                seen.Set(new Point(position.x + 1, position.y + dir));
            return seen;
        }
    }
}