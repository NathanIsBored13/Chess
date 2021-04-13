using System;
using System.Linq;
using System.Windows.Controls;

namespace Chess
{
    class Game
    {
        private Board board;
        private readonly Renderer renderer;
        private readonly int renderHandle;
        private readonly Mouse mouse = new Mouse();
        private PlayerQueue playerQueue;

        private readonly Piece[,] template = new Piece[,]
        {
            {new Rook(false),   new Pawn(false), null, null, null, null, new Pawn(true), new Rook(true)     },
            {new Knight(false), new Pawn(false), null, null, null, null, new Pawn(true), new Knight(true)   },
            {new Bishop(false), new Pawn(false), null, null, null, null, new Pawn(true), new Bishop(true)   },
            {new King(false),   new Pawn(false), null, null, null, null, new Pawn(true), new King(true)     },
            {new Queen(false),  new Pawn(false), null, null, null, null, new Pawn(true), new Queen(true)    },
            {new Bishop(false), new Pawn(false), null, null, null, null, new Pawn(true), new Bishop(true)   },
            {new Knight(false), new Pawn(false), null, null, null, null, new Pawn(true), new Knight(true)   },
            {new Rook(false),   new Pawn(false), null, null, null, null, new Pawn(true), new Rook(true)     }
        };

        public Game(Grid target)
        {
            renderer = new Renderer(target, mouse);
            renderHandle = renderer.Register();
        }

        public void Begin(PlayerType white, PlayerType black)
        {
            playerQueue = new PlayerQueue(Player.MakePlayer(white, true, renderer, mouse), Player.MakePlayer(black, false, renderer, mouse));
            board = new Board(template);
            renderer.SetSource(board);
            renderer.RenderIcons();
            Player player = playerQueue.PeekPlayer();
            
            while (board.GetMoves(player.GetColour()).Length > 0)
            {
                board.Move(player.Move(board));
                renderer.ResetHighlights(renderHandle);

                player = playerQueue.Next();

                if (board.FindChecks(player.GetColour()).Count() > 0)
                    renderer.SetHighlight(renderHandle, Highlight.InCheck, board.FindKing(player.GetColour()));
                renderer.RenderIcons();
            }
            Console.WriteLine("{0} player won", player.GetColour() ? "black" : "white");
        }
    };
}