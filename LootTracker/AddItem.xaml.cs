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
using System.Text.RegularExpressions;
using System.IO;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        public bool canceled = true;
        byte[] imagearray;


        public AddItem()
        {
            InitializeComponent();
        }


        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            //We need to validate the data types to ensure the data
            //source remains constrained.
            if (!(textBox_Name.Text == "") && !(comboBox_Type.Text == "") && !(textBox_Count.Text == "") && !(textBox_BaseWeight.Text == "") && !(textBox_BaseValue.Text == ""))
            {
                canceled = false;
                Close();
            }
            else
            {
                if (textBox_Name.Text == "")
                {
                    textBlock_Name.Foreground = Brushes.Red;
                    if (!Regex.IsMatch(textBlock_Name.Text, @"^\*"))
                    {
                        textBlock_Name.Text = ("*" + textBlock_Name.Text);
                    }
                }
                else
                {
                    textBlock_Name.Foreground = Brushes.Black;
                    textBlock_Name.Text = "Item Name";
                }
            }
                    
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            DragMove();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
