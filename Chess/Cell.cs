using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess
{

    class Cell : Button
    {
        public bool Checkered
        {
            get { return (bool)Dispatcher.Invoke(new Func<object>(() => { return GetValue(CheckeredProperty); })); }
            set { Dispatcher.Invoke(() => SetValue(CheckeredProperty, value)); }
        }
        public static readonly DependencyProperty CheckeredProperty = DependencyProperty.Register("Checkered", typeof(bool), typeof(Cell));

        public int Highlighted
        {
            get { return (int)Dispatcher.Invoke(new Func<object>(() => { return GetValue(HighlightedProperty); })); }
            set { Dispatcher.Invoke(() => SetValue(HighlightedProperty, value)); }
        }
        public static readonly DependencyProperty HighlightedProperty = DependencyProperty.Register("Highlighted", typeof(int), typeof(Cell));

        private readonly Mouse mouse;
        private readonly Point position;
        private readonly Image image = new Image()
        {
            Stretch = Stretch.UniformToFill
        };

        public Cell(Point position, Mouse mouse) : base()
        {
            Grid.SetColumn(this, position.x);
            Grid.SetRow(this, position.y);
            Checkered = (((position.y % 2) + position.x) % 2) == 0;
            this.position = position;
            this.mouse = mouse;
            Click += Cell_Click;
            Content = image;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            mouse.SetLastClicked(position);
        }

        public void SetImage(BitmapImage image)
        {
            Dispatcher.Invoke(() => this.image.Source = image);
        }
    }
}
