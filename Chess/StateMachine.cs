using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    class StateMachine
    {
        private int ptr = 0;
        private readonly State[] states;

        public StateMachine(State[] states)
        {
            this.states = states;
        }

        public void Run()
        {
            while (ptr != -1)
                ptr = states[ptr].Next();
        }
    }
}
