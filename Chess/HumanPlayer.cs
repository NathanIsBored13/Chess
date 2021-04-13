using System;
using System.Linq;
using System.Windows;

namespace Chess
{
    class HumanPlayer : Player
    {
        private readonly int renderHandle;
        private readonly Renderer renderer;
        private readonly Mouse mouse;

        private Optional<Type> returnBuffer;

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
                            foreach (PieceMove selectedMove in moves.Where(move => move.vector.p1 == mouse.GetLastClicked()))
                            {
                                highlighted[selectedMove.vector.p2] = true;
                                renderer.SetHighlight(renderHandle, selectedMove.type == MoveType.Move ? Highlight.MovePossible : Highlight.AttackMovePossible, selectedMove.vector.p2);
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
                            foreach (PieceMove selectedMove in moves.Where(move => move.vector.p1 == mouse.GetLastClicked()))
                            {
                                highlighted[selectedMove.vector.p2] = true;
                                renderer.SetHighlight(renderHandle, selectedMove.type == MoveType.Move ? Highlight.MovePossible : Highlight.AttackMovePossible, selectedMove.vector.p2);
                            }
                        },
                        ptr = 1
                    },

                    new TransitionExpressioin
                    {
                        function = () => buildVector.p2 = mouse.GetLastClicked(),
                        ptr = -1
                    }
                },

                () =>
                {
                    mouse.WaitForInput();
                    return highlighted[mouse.GetLastClicked()] ? 2 : (board[mouse.GetLastClicked()]?.GetColour() == GetColour() ? 1 : 0 );
                })
            }).Run();

            renderer.ResetHighlights(renderHandle);

            PieceMove ret = moves.First(x => x.vector == buildVector);
            if (board[buildVector.p1]?.GetType() == Type.Pawn && (buildVector.p2.y == 0 || buildVector.p2.y == 7))
                ret.additionalMoves = new Vector[] { new Vector(new Point((int)RequestPawnPromo().Value, GetColour() ? -1 : -2), buildVector.p2) };

            return ret;
        }

        private Optional<Type> RequestPawnPromo()
        {
            Application.Current.Dispatcher.Invoke(() => new PawnPromotionWindow(Callback).ShowDialog());
            return returnBuffer;
        }

        private void Callback(Optional<Type> value)
        {
            returnBuffer = value;
        }
    }
}