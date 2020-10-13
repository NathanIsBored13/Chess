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
                renderer.RenderIcons();
                playerQueue.Next();
            }
        }
    };
}