using System;
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
            //We need to validate the value in the textBox is an interger b/w 0-100.
            int percent;
            bool isValid = false;
            try { percent = Convert.ToInt32(textBox.Text); if (percent > 0 && percent <= 100) { isValid = true; } }
            catch { isValid = false; }
            
            if (isValid)
            {
                _isCancelled = false;
                Close();
            }
           else
            {
                textBox.Text = "50";
            }
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
