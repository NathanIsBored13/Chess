using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Chess
{
    class Bishop : Piece
    {
        public Bishop(bool colour, Point GetPoition) : base(colour, GetPoition)
        {

        }

        public override Type GetType()
        {
            return Type.Bishop;
        }

        public override PieceMovesMask GetMovesMask(Board board)
        {
            List<Vector> moves = new List<Vector>();
            List<Vector> attacks = new List<Vector>();
            int index = 0;
            int[,] dirs = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            do
            {
                bool hitPiece = false;
                Point pointer = new Point(GetPoition().x + dirs[index, 0], GetPoition().y + dirs[index, 1]);
                while (pointer.x <= 7 && pointer.x >= 0 && pointer.y <= 7 && pointer.y >= 0 && !hitPiece)
                {
                    if (board.GetPiece(pointer.x, pointer.y) is Piece piece)
                    {
                        hitPiece = true;
                        if (piece.GetColour() != GetColour())
                        {
                            attacks.Add(new Vector(GetPoition(), pointer));
                        }
                    }
                    else
                    {
                        moves.Add(new Vector(GetPoition(), pointer));
                    }
                    pointer.x += dirs[index, 0];
                    pointer.y += dirs[index, 1];
                }
                index++;
            } while (index < 4);
            return new PieceMovesMask(attacks.ToArray(), moves.ToArray());
        }
    }
}
