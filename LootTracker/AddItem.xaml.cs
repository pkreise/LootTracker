using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            SolidColorBrush lgtgry = new SolidColorBrush(Color.FromRgb(222, 222, 222));
                   

            //Field validation for Name.
            if (textBox_Name.Text == "")
            {
                label_Name.Foreground = Brushes.Red;
            }
            else
            {
                label_Name.Foreground = lgtgry;
                namevalid = true;
            }

            //Field Validation for Type.
            if (comboBox_Type.Text == "")
            {
                label_Type.Foreground = Brushes.Red;
            }
            else
            {
                label_Type.Foreground = lgtgry;
                typevalid = true;
            }

            //Field Validation for Count.
            if (textBox_Count.Text == "")
            {
                label_Count.Foreground = Brushes.Red;
                label_Count.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToInt32(textBox_Count.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    label_Count.Foreground = lgtgry;
                    label_Count.ToolTip = null;
                    countvalid = true;
                }
                else
                {
                    label_Count.Foreground = Brushes.Red;
                    label_Count.ToolTip = "Please enter a valid interger.";
                    textBox_Count.Text = "";
                }
            }

            //Field Validation for BaseWeight.
            if (textBox_BaseWeight.Text == "")
            {
                label_BaseWgt.Foreground = Brushes.Red;
                label_BaseWgt.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToDecimal(textBox_BaseWeight.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    label_BaseWgt.Foreground = lgtgry;
                    label_BaseWgt.ToolTip = null;
                    wgtvalid = true;
                }
                else
                {
                    label_BaseWgt.Foreground = Brushes.Red;
                    label_BaseWgt.ToolTip = "Please enter a valid decimal.";
                    textBox_BaseWeight.Text = "";
                }
            }

            //Field Validation for BaseValue.
            if (textBox_BaseValue.Text == "")
            {
                label_BaseVal.Foreground = Brushes.Red;
                label_BaseVal.ToolTip = "Please enter a value.";
            }
            else
            {
                bool isvalidint = true;
                try { Convert.ToInt32(textBox_BaseValue.Text); }
                catch { isvalidint = false; }
                if (isvalidint)
                {
                    label_BaseVal.Foreground = lgtgry;
                    label_BaseVal.ToolTip = null;
                    valvalid = true;
                }
                else
                {
                    label_BaseVal.Foreground = Brushes.Red;
                    label_BaseVal.ToolTip = "Please enter a valid interger.";
                    textBox_BaseValue.Text = "";
                }
            }

            if (namevalid && typevalid && countvalid && wgtvalid && valvalid)
            {
                canceled = false;
                Close();
            }
        }

        //Event handler for dragging window.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            DragMove();
        }

        //Event handler for clicking close button.
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
