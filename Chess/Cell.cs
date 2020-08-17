﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess
{
    enum Highlight
    {
        None,
        CellSelected,
        MovePossible,
        AttackMovePossible,
        InCheck
    }

    class Cell : Button
    {
        public bool Checkered
        {
            get { return (bool)GetValue(CheckeredProperty); }
            set { SetValue(CheckeredProperty, value); }
        }
        public static readonly DependencyProperty CheckeredProperty = DependencyProperty.Register("Checkered", typeof(bool), typeof(Cell));

        public Highlight Highlighted
        {
            get { return (Highlight)GetValue(HighlightedProperty); }
            set { SetValue(HighlightedProperty, value); }
        }
        public static readonly DependencyProperty HighlightedProperty = DependencyProperty.Register("Highlighted", typeof(Highlight), typeof(Cell));

        private readonly Point position;
        private Image image = new Image()
        {
            Stretch = Stretch.UniformToFill
        };

        public Cell(Point position) : base()
        {
            Grid.SetColumn(this, position.x);
            Grid.SetRow(this, position.y);
            this.position = position;
            Checkered = (((position.y % 2) + position.x) % 2) == 0;
            Click += Cell_Click;
            Content = image;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"{position.x}, {position.y}");
            Mouse.SetLastClicked(position);
        }

        public void SetImage(BitmapImage image)
        {
            Dispatcher.Invoke(() => this.image.Source = image);
        }
    }
}
