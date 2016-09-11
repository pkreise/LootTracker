using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for RemovePlayer_Conf.xaml
    /// </summary>
    public partial class RemovePlayer_Conf : Window
    {
        //Class fields.
        bool _isCancelled;

        //Class properties.
        public bool isCancelled { get { return _isCancelled; } }

        //Constructor.
        public RemovePlayer_Conf()
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
