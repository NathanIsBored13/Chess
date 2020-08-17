using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Chess
{
    class Game
    {
        private Board board;
        private ChessRenderer renderer;
        private PlayerQueue playerQueue;

        private Piece[,] template = new Piece[,]
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

        public Game(Board board, ChessRenderer renderer, PlayerType white, PlayerType black)
        {
            this.board = board;
            this.renderer = renderer;
            board.SetBoard(template);
            playerQueue = new PlayerQueue(Player.MakePlayer(white, true), Player.MakePlayer(black, false));
            renderer.Render();
        }

        public void Begin()
        {
            Vector vec = playerQueue.PeekPlayer().Move(board);
            Console.WriteLine($"[{vec.p1.x}, {vec.p1.y}] -> [{vec.p2.x}, {vec.p2.y}]");
            renderer.Render();
        }
    };
}