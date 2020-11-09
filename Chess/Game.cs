using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Chess
{
    class Game
    {
        private readonly Board board = new Board();
        private readonly Renderer renderer;
        private PlayerQueue playerQueue;

        private readonly Piece[,] template = new Piece[,]
        {
            {new Rook(false, new Point(0, 0)),   new Pawn(false, new Point(0, 1)), null, null, null, null, new Pawn(true, new Point(0, 6)), new Rook(true, new Point(0, 7))     },
            {new Knight(false, new Point(1, 0)), new Pawn(false, new Point(1, 1)), null, null, null, null, new Pawn(true, new Point(1, 6)), new Knight(true, new Point(1, 7))   },
            {new Bishop(false, new Point(2, 0)), new Pawn(false, new Point(2, 1)), null, null, null, null, new Pawn(true, new Point(2, 6)), new Bishop(true, new Point(2, 7))   },
            {new King(false, new Point(3, 0)),   new Pawn(false, new Point(3, 1)), null, null, null, null, new Pawn(true, new Point(3, 6)), new King(true, new Point(3, 7))     },
            {new Queen(false, new Point(4, 0)),  new Pawn(false, new Point(4, 1)), null, null, null, null, new Pawn(true, new Point(4, 6)), new Queen(true, new Point(4, 7))    },
            {new Bishop(false, new Point(5, 0)), new Pawn(false, new Point(5, 1)), null, null, null, null, new Pawn(true, new Point(5, 6)), new Bishop(true, new Point(5, 7))   },
            {new Knight(false, new Point(6, 0)), new Pawn(false, new Point(6, 1)), null, null, null, null, new Pawn(true, new Point(6, 6)), new Knight(true, new Point(6, 7))   },
            {new Rook(false, new Point(7, 0)),   new Pawn(false, new Point(7, 1)), null, null, null, null, new Pawn(true, new Point(7, 6)), new Rook(true, new Point(7, 7))     }
        };

        public Game(Grid target)
        {
            renderer = new Renderer(target, board);
            renderer.RenderIcons();
        }

        public void Begin(PlayerType white, PlayerType black)
        {
            board.SetState(template);
            renderer.RenderIcons();
            playerQueue = new PlayerQueue(Player.MakePlayer(white, true, renderer), Player.MakePlayer(black, false, renderer));
            while (true)
            {
                Console.WriteLine(playerQueue.PeekPlayer().GetColour());
                Vector vec = playerQueue.PeekPlayer().Move(board);
                board.Move(vec);
                board.HighlightChecks(playerQueue.PeekPlayer().GetColour(), renderer);
                renderer.RenderIcons();
                playerQueue.Next();
            }
        }
    };
}