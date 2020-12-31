using System;
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
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for SetupGame.xaml
    /// </summary>
    public partial class SetupGame : Window
    {
        readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public SetupGame()
        {
            InitializeComponent();
        }

        public Tuple<PlayerType, PlayerType> AwaitEntery()
        {

            autoResetEvent.WaitOne();
            return Dispatcher.Invoke(new Func<Tuple<PlayerType, PlayerType>>(() => new Tuple<PlayerType, PlayerType>((PlayerType)(WhiteComboBox.SelectedItem as Tuple<Enum, string>).Item1, (PlayerType)(BlackComboBox.SelectedItem as Tuple<Enum, string>).Item1)));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            autoResetEvent.Set();
        }
    }
}
