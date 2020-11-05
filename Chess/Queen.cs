using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Queen : Piece
    {
        public Queen(bool colour, Point position) : base(colour, position)
        {

        }

        public override Type GetType()
        {
            return Type.Queen;
        }

        public override PieceMovesMask GetMovesMask(Board board, Point position)
        {
            List<Vector> moves = new List<Vector>();
            List<Vector> attacks = new List<Vector>();
            int index = 0;
            int[,] dirs = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            do
            {
                bool hitPiece = false;
                Point pointer = new Point(position.x + dirs[index, 0], position.y + dirs[index, 1]);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    if (board.GetPiece(pointer.x, pointer.y) is Piece piece)
                    {
                        hitPiece = true;
                        if (piece.GetColour() != GetColour())
                        {
                            attacks.Add(new Vector(position, pointer));
                        }
                    }
                    else
                    {
                        moves.Add(new Vector(position, pointer));
                    }
                    pointer.x += dirs[index, 0];
                    pointer.y += dirs[index, 1];
                }
                index++;
            } while (index < 8);
            return new PieceMovesMask(attacks.ToArray(), moves.ToArray());
        }
    }
}
