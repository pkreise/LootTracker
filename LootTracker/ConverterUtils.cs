using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LootTracker
{   

    /// <summary>
    /// Background color converter.
    /// </summary>
    public sealed class B_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListViewItem item = value as ListViewItem;
            
            ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            

            // Get the index of a ListViewItem
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);

            if (index % 2 == 0)
            {
                return "#FF464646";
            }
            else if (true)
            {
                return "#FFDEDEDE";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Foreground color converter.
    /// </summary>
    public sealed class F_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;

            // Get the index of a ListViewItem
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);

            if (index % 2 == 0)
            {
                return "#FFDEDEDE";
            }
            else
            {
                return "#FF464646";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
     }

    public sealed class D_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            LootItem item = (LootItem)values[0];
            Player player = (Player)values[1];
            if (item.assignments.ContainsKey(player.playername))
            {
                return item.assignments[(player.playername)];
            }
            else
            {
                return 0;
            } 
        }

        object[] ConvertBack(object values, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
