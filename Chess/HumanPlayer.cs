using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chess
{
    class HumanPlayer : Player
    {
        public HumanPlayer(bool colour, Renderer renderer) : base(colour, renderer)
        {

        }

        public override Vector Move(Board board)
        {
            Point king = new Point();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board.GetPiece(x, y) is King k && k.GetColour() == GetColour())
                    {
                        king = new Point(x, y);
                    }
                }
            }
            GetRenderer().ResetHighlights(new Point[] { king });

            Vector? ret = null;
            Point? p1 = null;
            Point? p2 = null;
            Vector[] moves = board.GetMoves(GetColour());
            Vector[] filteredMoves = null;
            do
            {
                if (p1 == null)
                {
                    p1 = Mouse.WaitForInput();
                    filteredMoves = moves.Where(v => v.p1.x == p1.Value.x && v.p1.y == p1.Value.y).ToArray();
                    if (filteredMoves.Length == 0)
                    {
                        p1 = null;
                    }
                    else
                    {
                        foreach (Vector vec in filteredMoves)
                        {
                            if (board.GetPiece(vec.p2.x, vec.p2.y) is Piece)
                            {
                                GetRenderer().SetHighlight(Highlight.AttackMovePossible, new Point(vec.p2.x, vec.p2.y));
                            }
                            else
                            {
                                GetRenderer().SetHighlight(Highlight.MovePossible, new Point(vec.p2.x, vec.p2.y));
                            }
                        }
                    }

                }
                else if (p2 == null)
                {
                    Point buffer = Mouse.WaitForInput();
                    foreach (Vector vec in filteredMoves)
                    {
                        if (vec.p2.x == buffer.x && vec.p2.y == buffer.y)
                        {
                            p2 = buffer;
                        }
                    }
                    if (p2 == null)
                    {
                        p1 = buffer;
                        GetRenderer().ResetHighlights();
                        filteredMoves = moves.Where(v => v.p1.x == p1.Value.x && v.p1.y == p1.Value.y).ToArray();
                        if (filteredMoves.Length == 0)
                        {
                            p1 = null;
                        }
                        else
                        {
                            foreach (Vector vec in filteredMoves)
                            {
                                if (board.GetPiece(vec.p2.x, vec.p2.y) is Piece)
                                {
                                    GetRenderer().SetHighlight(Highlight.AttackMovePossible, new Point(vec.p2.x, vec.p2.y));
                                }
                                else
                                {
                                    GetRenderer().SetHighlight(Highlight.MovePossible, new Point(vec.p2.x, vec.p2.y));
                                }
                            }
                        }
                    }
                }
                else
                {
                    ret = new Vector(p1.Value, p2.Value);
                }
            } while (ret == null);
            return ret.Value;
        }
    }
}