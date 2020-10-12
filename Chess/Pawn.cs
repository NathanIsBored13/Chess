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
        public Pawn(bool colour) : base(colour)
        {

        }

        public override Type GetType()
        {
            return Type.Pawn;
        }

        public override PieceMovesMask GetMovesMask(Board board, Point point)
        {
            List<Vector> moves = new List<Vector>();
            if (point.y > 0 && point.y < 8 && board.GetPiece(point.x, point.y + (GetColour() ? -1 : 1)) == null)
            {
                moves.Add(new Vector(point, new Point(point.x, point.y + (GetColour() ? -1 : 1))));
                if (point.y == 1 || point.y == 6)
                {
                    moves.Add(new Vector(point, new Point(point.x, point.y + (GetColour() ? -2 : 2))));
                }
            }
            List<Vector> attacks = new List<Vector>();
            if (point.y > 0 && point.y < 8 && point.x < 7 && board.GetPiece(point.x + 1, point.y + (GetColour() ? -1 : 1)) is Piece p1 && p1.GetColour() != GetColour())
            {
                attacks.Add(new Vector(point, new Point(point.x + 1, point.y + (GetColour() ? -1 : 1))));
            }
            if (point.y > 0 && point.y < 8 && point.x > 1 && board.GetPiece(point.x - 1, point.y + (GetColour() ? -1 : 1)) is Piece p2 && p2.GetColour() != GetColour())
            {
                attacks.Add(new Vector(point, new Point(point.x - 1, point.y + (GetColour() ? -1 : 1))));
            }
            return new PieceMovesMask(attacks.ToArray(), moves.ToArray());
        }
    }
}
