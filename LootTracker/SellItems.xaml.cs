using System.Windows;
using System.Windows.Input;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for SellItems.xaml
    /// </summary>
    public partial class SellItems : Window
    {
        //Class fields.
        bool _isCancelled;

        //Class properties.
        public bool isCancelled { get { return _isCancelled; } }

        //Constructor
        public SellItems()
        {
            InitializeComponent();
        }

        //Event handler for clicking the OK button.
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            _isCancelled = false;
            Close();
        }

        //Event handler for clicking the Cancel button.
        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _isCancelled = true;
            Close();
        }

        //Event handler for window drag.
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
