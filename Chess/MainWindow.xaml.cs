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
    }

    public partial class MainWindow : Window
    {
        readonly ChessRenderer renderer;
        readonly Board board;
        Game game;
        Thread thread = null;

        public MainWindow()
        {
            InitializeComponent();
            Icons.LoadImages("Set1");
            board = new Board();
            renderer = new ChessRenderer(DrawTarget, board);
        }

        private void MakeGame_Click(object sender, RoutedEventArgs e)
        {
            game = new Game(board, renderer, PlayerType.HumanPlayer, PlayerType.HumanPlayer);
            thread = new Thread(new ThreadStart(game.Begin));
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
