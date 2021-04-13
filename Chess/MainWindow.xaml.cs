using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Chess
{
    public struct Point
    {
        public int x;
        public int y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"[{x}, {y}]";
        }

        public static bool operator ==(Point l, Point r) => l.x == r.x && l.y == r.y;

        public static bool operator !=(Point l, Point r) => l.x != r.x || l.y != r.y;
    }

    public struct Vector
    {
        public Point p1;
        public Point p2;
        public Vector(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        public static bool operator ==(Vector l, Vector r) => l.p1 == r.p1 && l.p2 == r.p2;

        public static bool operator !=(Vector l, Vector r) => l.p1 != r.p1 || l.p2 != r.p2;

        public override string ToString()
        {
            return $"{p1} => {p2}";
        }
    }

    public struct Optional<T>
    {
        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;
                set = true;
            }
        }
        private T value;

        public bool Set { get { return set; } }
        private bool set;
    }

    public partial class MainWindow : Window
    {
        private Game game;
        private Thread gameThread = null;

        public MainWindow()
        {
            InitializeComponent();
            Icons.LoadImages("Set1");
            game = new Game(DrawTarget);
            Point[] vals = new Point[] { };
            Console.WriteLine(vals.FirstOrDefault());
        }

        private void MakeGame_Click(object sender, RoutedEventArgs e)
        {
            new SetupGame(MakeGame) {
                Owner = this
            }.Show();
        }

        private void MakeGame(Tuple<PlayerType, PlayerType> players)
        {
            if (gameThread != null)
            {
                gameThread.Abort();
                gameThread.Join();
                game = new Game(DrawTarget);
            }
            gameThread = new Thread(() => game.Begin(players.Item1, players.Item2));
            gameThread.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gameThread != null)
            {
                gameThread.Abort();
                gameThread.Join();
            }
        }
    }
}
