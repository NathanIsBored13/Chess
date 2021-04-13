using System;
using System.Windows;

namespace Chess
{
    public partial class SetupGame : Window
    {
        readonly Action<Tuple<PlayerType, PlayerType>> callback;

        public SetupGame(Action<Tuple<PlayerType, PlayerType>> callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            callback(Dispatcher.Invoke(new Func<Tuple<PlayerType, PlayerType>>(() => new Tuple<PlayerType, PlayerType>((PlayerType)(WhiteComboBox.SelectedItem as Tuple<Enum, string>).Item1, (PlayerType)(BlackComboBox.SelectedItem as Tuple<Enum, string>).Item1))));
            Close();
        }
    }
}
