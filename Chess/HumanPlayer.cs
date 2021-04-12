using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess
{
    class HumanPlayer : Player
    {
        private readonly int renderHandle;
        private readonly Renderer renderer;
        private readonly Mouse mouse;

        private readonly AutoResetEvent threadLatch = new AutoResetEvent(false);
        private Optional<Type> threadBuffer;

        public HumanPlayer(bool colour, Renderer renderer, Mouse mouse) : base(colour)
        {
            this.renderer = renderer;
            renderHandle = renderer.Register();
            this.mouse = mouse;
        }

        public override PieceMove Move(Board board)
        {
            BitBoard highlighted = new BitBoard();
            PieceMove[] moves = board.GetMoves(GetColour());
            Vector buildVector = new Vector();

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
                            buildVector.p1 = mouse.GetLastClicked();
                            renderer.SetHighlight(renderHandle, Highlight.CellSelected, buildVector.p1);
                            foreach (PieceMove v in moves.Where(x => x.vector.p1.x == mouse.GetLastClicked().x && x.vector.p1.y == mouse.GetLastClicked().y))
                            {
                                Console.WriteLine($"{v.vector}");
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
                            buildVector.p1 = mouse.GetLastClicked();
                            renderer.SetHighlight(renderHandle, Highlight.CellSelected, buildVector.p1);
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
                            buildVector.p2 = mouse.GetLastClicked();
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

            PieceMove ret = moves.First(x => x.vector == buildVector);
            if (board[buildVector.p1]?.GetType() == Type.Pawn && (buildVector.p2.y == 0 || buildVector.p2.y == 7))
                ret.additionalMoves = new Vector[] { new Vector(new Point((int)RequestPawnPromo().Value, GetColour() ? -1 : -2), buildVector.p2) };

            return ret;
        }

        private Optional<Type> RequestPawnPromo()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                PawnPromotionWindow wnd = new PawnPromotionWindow(Callback);
                wnd.ShowDialog();
            });
            return threadBuffer;
        }

        private void Callback(Optional<Type> value)
        {
            threadBuffer = value;
        }
    }
}