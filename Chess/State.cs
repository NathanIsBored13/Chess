using System;

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
