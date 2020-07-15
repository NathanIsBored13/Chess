using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Chess
{
    class PlayerQueue
    {
        int pointer = 0;
        Player[] queue;

        public PlayerQueue(Player p1, Player p2)
        {
            queue = new Player[] { p1, p2 };
        }

        public void Next()
        {
            pointer = pointer++ % 2;
        }

        public Player PeekPlayer() => queue[pointer];
    }
}
