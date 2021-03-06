﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    struct Point
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
    }

    struct Vector
    {
        public Point p1;
        public Point p2;
        public Vector(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public override string ToString()
        {
            return $"{p1} => {p2}";
        }
    }

    public partial class MainWindow : Window
    {
        private Game game;
        private Thread thread = null;

        public MainWindow()
        {
            InitializeComponent();
            Icons.LoadImages("Set1");
            game = new Game(DrawTarget);
        }

        private void MakeGame_Click(object sender, RoutedEventArgs e)
        {
            SetupGame wnd = new SetupGame(MakeGame)
            {
                Owner = this
            };
            wnd.Show();
        }

        private void MakeGame(Tuple<PlayerType, PlayerType> players)
        {
            if (thread != null)
            {
                thread.Abort();
                thread.Join();
                game = new Game(DrawTarget);
            }
            thread = new Thread(() => game.Begin(players.Item1, players.Item2));
            thread.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
                thread.Join();
            }
        }
    }
}
