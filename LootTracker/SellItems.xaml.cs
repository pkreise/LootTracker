using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for SellItems.xaml
    /// </summary>
    public partial class SellItems : Window
    {
        //Class fields.
        bool _isCancelled;
        private event PropertyChangedEventHandler PropertyChanged;
        int _sellPercent = 50;
        ObservableCollection<LootItem> _items;

        //Class properties.
        public bool isCancelled { get { return _isCancelled; } }
        public int sellPercent
        {
            get { return _sellPercent; }
            set
            {
                _sellPercent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(sellPercent)));
            }
        }
        public ObservableCollection<LootItem> items
        {
            get { return _items; }
            set
            {
                _items = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(items)));
            }
        }

        //Constructor
        public SellItems(ObservableCollection<LootItem> i)
        {
            DataContext = this;
            items = i;
            InitializeComponent();            
        }

        //Event handler for clicking the OK button.
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            //We need to validate the value in the textBox is an interger b/w 0-100.
            int percent;
            bool isValid = false;
            try { percent = Convert.ToInt32(textBox_SellPercent.Text); if (percent > 0 && percent <= 100) { isValid = true; } }
            catch { isValid = false; }
            
            if (isValid)
            {
                _isCancelled = false;
                Close();
            }
           else
            {
                textBox_SellPercent.Text = "50";
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
