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
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        public bool canceled = true;

        public AddItem()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            //We need to validate the data types to ensure the data
            //source remains constrained.
            if (!(textBox_Name.Text == "") && !(comboBox_Type.Text == "") && !(textBox_Count.Text == "") && !(textBox_BaseWeight.Text == "") && !(textBox_BaseValue.Text == "") )
            {
                canceled = false;
                Close();
            }    
                    
        }
    }
}
