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

        public override List<PieceMove> GetMovesMask(Board board, Point position)
        {
            List<PieceMove> ret = new List<PieceMove>();
            if (position.y > 0 && position.y < 8 && board[position.x, position.y + dir] == null)
            {
                ret.Add(new PieceMove(new Vector(position, new Point(position.x, position.y + dir)), MoveType.Move));
                if ((dir == 1 ? position.y == 1 : position.y == 6) && board[position.x, position.y + 2 * dir] == null)
                    ret.Add(new PieceMove(new Vector(position, new Point(position.x, position.y + 2 * dir)), MoveType.Move));
            }
            if (position.y > 0 && position.y < 8 && position.x < 7 && !board[position.x + 1, position.y + dir]?.GetColour() == GetColour())
                ret.Add(new PieceMove(new Vector(position, new Point(position.x + 1, position.y + dir)), MoveType.Capture));
            if (position.y > 0 && position.y < 8 && position.x > 0 && !board[position.x - 1, position.y + dir]?.GetColour() == GetColour())
                ret.Add(new PieceMove(new Vector(position, new Point(position.x - 1, position.y + dir)), MoveType.Capture));

            List<Vector> moveHistory = board.GetHistory();
            if (moveHistory.Count > 0)
            {
                Vector lastMove = moveHistory.Last();
                if (board[lastMove.p2].GetType() == Type.Pawn && Math.Abs(lastMove.p1.y - lastMove.p2.y) == 2)
                    foreach (Point p in GetSeen(board, position).GetAllSet().Where(x => x.x == lastMove.p1.x && x.x == lastMove.p2.x && Math.Sign(x.y - lastMove.p2.y) + Math.Sign(x.y - lastMove.p1.y) == 0))
                        ret.Add(new PieceMove(new Vector(position, p), new Vector[] { new Vector(lastMove.p2, new Point(-1, -1)) }, MoveType.Capture));
            }

            return ret;
        }

        public override BitBoard GetSeen(Board board, Point position)
        {
            BitBoard seen = new BitBoard();
            if (position.y > 0 && position.y < 8 && position.x > 0)
                seen[position.x - 1, position.y + dir] = true;
            if (position.y > 0 && position.y < 8 && position.x < 7)
                seen[position.x + 1, position.y + dir] = true;
            return seen;
        }
    }
}