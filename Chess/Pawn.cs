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

        public Pawn(bool colour, Point position) : base(colour, position)
        {
            dir = colour ? -1 : 1;
        }

        public override Type GetType()
        {
            return Type.Pawn;
        }

        public override PieceMovesMask GetMovesMask(Board board)
        {
            BitBoard moves = new BitBoard();
            if (GetPosition().y > 0 && GetPosition().y < 8 && board.GetPiece(GetPosition().x, GetPosition().y + dir) == null)
            {
                moves.Set(new Point(GetPosition().x, GetPosition().y + dir));
                if (GetPosition().y == 1 || GetPosition().y == 6 && board.GetPiece(GetPosition().x, GetPosition().y + 2 * dir) == null)
                {
                    moves.Set(new Point(GetPosition().x, GetPosition().y + 2 * dir));
                }
            }
            BitBoard attacks = new BitBoard();
            if (GetPosition().y > 0 && GetPosition().y < 8 && GetPosition().x < 7 && board.GetPiece(GetPosition().x + 1, GetPosition().y + dir) is Piece p1 && p1.GetColour() != GetColour())
            {
                attacks.Set(new Point(GetPosition().x + 1, GetPosition().y + dir));
            }
            if (GetPosition().y > 0 && GetPosition().y < 8 && GetPosition().x > 1 && board.GetPiece(GetPosition().x - 1, GetPosition().y + dir) is Piece p2 && p2.GetColour() != GetColour())
            {
                attacks.Set(new Point(GetPosition().x - 1, GetPosition().y + dir));
            }
            return new PieceMovesMask(attacks, moves);
        }

        public override BitBoard GetSeen(Board board)
        {
            BitBoard seen = new BitBoard();
            if (GetColour() ? (GetPosition().y > 0) : (GetPosition().y < 7) && GetPosition().y < 7 && GetPosition().x > 0)
                seen.Set(new Point(GetPosition().x - 1, GetPosition().y + dir));
            if (GetColour() ? (GetPosition().y > 0) : (GetPosition().y < 7) && GetPosition().y > 0 && GetPosition().x > 0)
                seen.Set(new Point(GetPosition().x + 1, GetPosition().y + dir));
            return seen;
        }
    }
}