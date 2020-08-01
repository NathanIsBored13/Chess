using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Chess
{
    class Mouse
    {
        private readonly AutoResetEvent wait = new AutoResetEvent(false);
        Point lastClicked;

        public Mouse()
        {
 
        }

        public void SetLastClicked(Point position)
        {
            lastClicked = position;
            wait.Set();
        }

        public Point WaitForInput()
        {
            wait.WaitOne();
            return lastClicked;
        }

        public Point GetLastClicked() => lastClicked;
    }
}
