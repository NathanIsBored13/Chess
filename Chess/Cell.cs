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
        private readonly Mouse mouse;
        private Piece piece;
        private Image image = new Image()
        {
            Stretch = Stretch.UniformToFill
        };

        public Cell(Point position, Piece piece, Mouse mouse) : base()
        {
            this.position = position;
            this.piece = piece;
            this.mouse = mouse;
            Checkered = (((position.y % 2) + position.x) % 2) == 0;
            Click += Cell_Click;
            Content = image;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            mouse.SetLastClicked(position);
        }
    }
}
