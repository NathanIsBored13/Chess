namespace Chess
{
    class PlayerQueue
    {
        private int pointer = 0;
        private readonly Player[] queue;

        public PlayerQueue(Player p1, Player p2)
        {
            queue = new Player[] { p1, p2 };
        }

        public Player Next() => queue[pointer = (pointer + 1) % 2];

        public Player PeekPlayer() => queue[pointer];
    }
}
