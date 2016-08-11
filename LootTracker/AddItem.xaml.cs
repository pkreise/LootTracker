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

        public AddItem()
        {
            InitializeComponent();
        }


        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            bool namevalid = false;
            bool typevalid = false;
            bool countvalid = false;
            bool wgtvalid = false;
            bool valvalid = false;

            if (textBox_Name.Text == "")
            {
                textBlock_Name.Foreground = Brushes.Red;
            }
            else
            {
                textBlock_Name.Foreground = Brushes.Black;
                namevalid = true;
            }

            //Field Validation for Type.
            if (comboBox_Type.Text == "")
            {
                textBlock_Type.Foreground = Brushes.Red;
            }
            else
            {
                textBlock_Type.Foreground = Brushes.Black;
                typevalid = true;
            }

            //Field Validation for Count.
            if (textBox_Count.Text == "")
            {
                textBlock_Count.Foreground = Brushes.Red;
                textBlock_Count.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToInt32(textBox_Count.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    textBlock_Count.Foreground = Brushes.Black;
                    textBlock_Count.ToolTip = null;
                    countvalid = true;
                }
                else
                {
                    textBlock_Count.Foreground = Brushes.Red;
                    textBlock_Count.ToolTip = "Please enter a valid interger.";
                    textBox_Count.Text = "";
                }
            }

            //Field Validation for BaseWeight.
            if (textBox_BaseWeight.Text == "")
            {
                textBlock_BaseWeight.Foreground = Brushes.Red;
                textBlock_BaseWeight.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToDecimal(textBox_BaseWeight.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    textBlock_BaseWeight.Foreground = Brushes.Black;
                    textBlock_BaseWeight.ToolTip = null;
                    wgtvalid = true;
                }
                else
                {
                    textBlock_BaseWeight.Foreground = Brushes.Red;
                    textBlock_BaseWeight.ToolTip = "Please enter a valid interger.";
                    textBox_BaseWeight.Text = "";
                }
            }

            //Field Validation for BaseValue.
            if (textBox_BaseValue.Text == "")
            {
                textBlock_BaseValue.Foreground = Brushes.Red;
                textBlock_BaseValue.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToInt32(textBox_BaseValue.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    textBlock_BaseValue.Foreground = Brushes.Black;
                    textBlock_BaseValue.ToolTip = null;
                    valvalid = true;
                }
                else
                {
                    textBlock_BaseValue.Foreground = Brushes.Red;
                    textBlock_BaseValue.ToolTip = "Please enter a valid interger.";
                    textBox_BaseValue.Text = "";
                }
            }

            if (namevalid && typevalid && countvalid && wgtvalid && valvalid)
            {
                canceled = false;
                Close();
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
