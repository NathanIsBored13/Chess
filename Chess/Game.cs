using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Chess
{
    class Game
    {
        Board board;
        public Game(UniformGrid DrawTarget)
        {
            board = new Board(DrawTarget);
        }
    }
}
