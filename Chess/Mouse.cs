using System.Threading;


namespace Chess
{
    class Mouse
    {
        private readonly AutoResetEvent thredLock = new AutoResetEvent(false);
        private Point lastClicked;

        public Mouse()
        {

        }

        public void SetLastClicked(Point position)
        {
            lastClicked = position;
            thredLock.Set();
        }

        public Point WaitForInput()
        {
            thredLock.Reset();
            thredLock.WaitOne();
            return lastClicked;
        }

        public Point GetLastClicked() => lastClicked;
    }
}