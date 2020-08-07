using System;
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
        private Piece piece;
        private Image image = new Image()
        {
            Stretch = Stretch.UniformToFill
        };

        public Cell(Point position) : base()
        {
            this.position = position;
            Checkered = (((position.y % 2) + position.x) % 2) == 0;
            Click += Cell_Click;
            Content = image;
            Icons.RegisterListener(RefreshImage);
        }

        public void SetPiece(Piece piece)
        {
            this.piece = piece;
            RefreshImage();
        }

        public Piece GetPiece() => piece;

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Mouse.SetLastClicked(position);
        }

        private void RefreshImage()
        {
            image.Source = piece?.GetImage();
        }
    }
}
