using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Chess
{
    static class Mouse
    {
        private static readonly AutoResetEvent wait = new AutoResetEvent(false);
        private static Point lastClicked;

        public static void SetLastClicked(Point position)
        {
            lastClicked = position;
            wait.Set();
        }

        public static Point WaitForInput()
        {
            wait.WaitOne();
            return lastClicked;
        }

        public static Point GetLastClicked() => lastClicked;
    }
}
