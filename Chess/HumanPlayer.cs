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
        private readonly int renderHandle;
        private readonly Renderer renderer;
        private readonly Mouse mouse;

        public HumanPlayer(bool colour, Renderer renderer, Mouse mouse) : base(colour)
        {
            this.renderer = renderer;
            renderHandle = renderer.Register();
            this.mouse = mouse;
        }

        public override Vector Move(Board board)
        {
            BitBoard highlighted = new BitBoard();
            PieceMove[] moves = board.GetMoves(GetColour());
            Vector ret = new Vector();

            new StateMachine(
            new State[]
            {
                new State(
                new TransitionExpressioin[]
                {
                    new TransitionExpressioin
                    {
                        function = () => { },
                        ptr = 0
                    },

                    new TransitionExpressioin
                    {
                        function = () =>
                        {
                            ret.p1 = mouse.GetLastClicked();
                            foreach (PieceMove v in moves.Where(x => x.vector.p1.x == mouse.GetLastClicked().x && x.vector.p1.y == mouse.GetLastClicked().y))
                            {
                                highlighted[v.vector.p2] = true;
                                renderer.SetHighlight(renderHandle, v.type == MoveType.Move ? Highlight.MovePossible : Highlight.AttackMovePossible, v.vector.p2);
                            }
                        },
                        ptr = 1
                    }
                },

                () =>
                {
                    mouse.WaitForInput();
                    return board[mouse.GetLastClicked()]?.GetColour() == GetColour() ? 1 : 0;
                }),

                new State(
                new TransitionExpressioin[]
                {
                    new TransitionExpressioin
                    {
                        function = () =>
                        {
                            highlighted = new BitBoard();
                            renderer.ResetHighlights(renderHandle);
                        },
                        ptr = 0
                    },

                    new TransitionExpressioin
                    {
                        function = () =>
                        {
                            highlighted = new BitBoard();
                            renderer.ResetHighlights(renderHandle);
                            ret.p1 = mouse.GetLastClicked();
                            foreach (PieceMove v in moves.Where(x => x.vector.p1.x == mouse.GetLastClicked().x && x.vector.p1.y == mouse.GetLastClicked().y))
                            {
                                highlighted[v.vector.p2] = true;
                                renderer.SetHighlight(renderHandle, v.type == MoveType.Move ? Highlight.MovePossible : Highlight.AttackMovePossible, v.vector.p2);
                            }
                        },
                        ptr = 1
                    },

                    new TransitionExpressioin
                    {
                        function = () =>
                        {
                            renderer.ResetHighlights(renderHandle);
                            ret.p2 = mouse.GetLastClicked();
                        },
                        ptr = -1
                    }
                },

                () =>
                {
                    mouse.WaitForInput();
                    return highlighted[mouse.GetLastClicked()] ? 2 : (board[mouse.GetLastClicked()]?.GetColour() == GetColour() ? 1 : 0 );
                })
            }).Run();

            return ret;
        }
    }
}