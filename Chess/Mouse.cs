using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Mouse
    {
        Point lastClicked;

        public Mouse()
        {

        }

        public void SetLastClicked(Point position)
        {
            lastClicked = position;
        }

        public Point GetLastClicked() => lastClicked;
    }
}
