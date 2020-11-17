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
        public Pawn(bool colour, Point position) : base(colour, position)
        {

        }

        public override Type GetType()
        {
            return Type.Pawn;
        }

        public override PieceMovesMask GetMovesMask(Board board)
        {
            BitBoard moves = new BitBoard();
            if (GetPoition().y > 0 && GetPoition().y < 8 && board.GetPiece(GetPoition().x, GetPoition().y + (GetColour() ? -1 : 1)) == null)
            {
                moves.Set(new Point(GetPoition().x, GetPoition().y + (GetColour() ? -1 : 1)));
                if (GetPoition().y == 1 || GetPoition().y == 6 && board.GetPiece(GetPoition().x, GetPoition().y + (GetColour() ? -2 : 2)) == null)
                {
                    moves.Set(new Point(GetPoition().x, GetPoition().y + (GetColour() ? -2 : 2)));
                }
            }
            BitBoard attacks = new BitBoard();
            if (GetPoition().y > 0 && GetPoition().y < 8 && GetPoition().x < 7 && board.GetPiece(GetPoition().x + 1, GetPoition().y + (GetColour() ? -1 : 1)) is Piece p1 && p1.GetColour() != GetColour())
            {
                attacks.Set(new Point(GetPoition().x + 1, GetPoition().y + (GetColour() ? -1 : 1)));
            }
            if (GetPoition().y > 0 && GetPoition().y < 8 && GetPoition().x > 1 && board.GetPiece(GetPoition().x - 1, GetPoition().y + (GetColour() ? -1 : 1)) is Piece p2 && p2.GetColour() != GetColour())
            {
                attacks.Set(new Point(GetPoition().x - 1, GetPoition().y + (GetColour() ? -1 : 1)));
            }
            return new PieceMovesMask(attacks, moves);
        }
    }
}
