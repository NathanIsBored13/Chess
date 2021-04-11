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
    /// Interaction logic for PawnPromotionWindow.xaml
    /// </summary>

    public partial class PawnPromotionWindow : Window
    {
        private readonly Action<Optional<Type>> callback;

        public PawnPromotionWindow(Action<Optional<Type>> callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void Promote_Button_Click(object sender, RoutedEventArgs e)
        {
            callback(new Optional<Type> { Value = (Type)(PromotionSelection.SelectedItem as Enum) });
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            callback(new Optional<Type>());
            Close();
        }
    }
}
