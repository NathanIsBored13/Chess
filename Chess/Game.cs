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
        private Board board;
        private readonly Renderer renderer;
        private readonly int renderHandle;
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
            renderer = new Renderer(target);
            renderHandle = renderer.Register();
        }

        public void Begin(PlayerType white, PlayerType black)
        {
            board = new Board(template);
            renderer.SetSource(board);
            renderer.RenderIcons();
            playerQueue = new PlayerQueue(Player.MakePlayer(white, true, renderer), Player.MakePlayer(black, false, renderer));
            while (true)
            {
                Vector vec = playerQueue.PeekPlayer().Move(board);
                board.Move(vec);
                renderer.ResetHighlights(renderHandle);
                if (board.FindChecks(!playerQueue.PeekPlayer().GetColour()).Count() > 0)
                    renderer.SetHighlight(renderHandle, Highlight.InCheck, board.FindKing(!playerQueue.PeekPlayer().GetColour()));
                renderer.RenderIcons();
                playerQueue.Next();
            }
        }
    };
}