using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class HumanPlayer : Player
    {
        public HumanPlayer(bool colour, Renderer renderer) : base(colour, renderer)
        {

        }

        public override Vector Move(Board board)
        {
            Vector? ret = null;
            Vector[] moves = board.GetMoves(GetColour());
            do
            {
                GetRenderer().ResetHighlights();
                Console.WriteLine("--MOVES--");
                foreach (Vector vec in moves)
                {
                    Console.WriteLine("[{0}, {1}] -> [{2}, {3}]", vec.p1.x, vec.p1.y, vec.p2.x, vec.p2.y);
                }
                Point p1 = Mouse.WaitForInput();
                Vector[] PossibleMoves = moves.Where(v => v.p1.x == p1.x && v.p1.y == p1.y).ToArray();
                foreach (Vector vec in PossibleMoves)
                {
                    Console.WriteLine("--Filtered Moves--");
                    Console.WriteLine("[{0}, {1}] -> [{2}, {3}]", vec.p1.x, vec.p1.y, vec.p2.x, vec.p2.y);
                    if (board.GetPiece(vec.p2.x, vec.p2.y) == null)
                    {
                        GetRenderer().SetHighlight(Highlight.MovePossible, vec.p2);
                    }
                    else
                    {
                        GetRenderer().SetHighlight(Highlight.AttackMovePossible, vec.p2);
                    }
                }
                Point p2 = Mouse.WaitForInput();
                foreach (Vector vec in PossibleMoves)
                {
                    if (vec.p2.x == p2.x && vec.p2.y == p2.y)
                    {
                        ret = new Vector(p1, p2);
                    }
                }
            } while (ret == null);
            return ret.Value;
        }
    }
}
