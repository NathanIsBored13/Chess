using System;
using System.Windows;

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
