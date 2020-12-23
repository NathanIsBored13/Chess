using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    struct TransitionExpressioin
    {
        public int ptr;
        public Action function;
    };

    class State
    {
        private readonly Func<int> parse;
        private readonly TransitionExpressioin[] transitions;
        
        public State (TransitionExpressioin[] transitions, Func<int> parse)
        {
            this.transitions = transitions;
            this.parse = parse;
        }

        public int Next()
        {
            int index = parse();
            transitions[index].function();
            return transitions[index].ptr;
        }
    }
}
