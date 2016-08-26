using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
                return "#FFFFFFFF";
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
                return "#FF464646";
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

    /// <summary>
    /// Player equipped count converter.
    /// </summary>
    public sealed class C_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            Dictionary<string, int> d = values[0] as Dictionary<string, int>;
            Player p = values[1] as Player;

            if (d.ContainsKey(p.playername))
            {
                return (d[(p.playername)]);
            }
            else
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Player equipped weight converter.
    /// </summary>
    public sealed class W_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            Dictionary<string, int> d = values[0] as Dictionary<string, int>;
            Player p = values[1] as Player;
            decimal wgt;
            try { wgt = System.Convert.ToDecimal(values[2]); }
            catch { return "ERROR"; }
           
            
            if (d.ContainsKey(p.playername))
            {
                return (d[(p.playername)] * wgt);
            }
            else
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Player equipped value converter.
    /// </summary>
    public sealed class V_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {

            Dictionary<string, int> d = values[0] as Dictionary<string, int>;
            Player p = values[1] as Player;
            
            decimal val;
            try { val = System.Convert.ToDecimal(values[2]); }
            catch { return "ERROR"; }

            if (d.ContainsKey(p.playername))
            {
                return (d[(p.playername)] * val);
            }
            else
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class S_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<LootItem> items = value as ObservableCollection<LootItem>;
            decimal totalValue = 0;

            if (items != null)
            {
                foreach (LootItem i in items)
                {
                    if (i.unassignedcount > 0)
                    {
                        totalValue += i.unassignedvalue;
                    }
                }
            }

            return String.Format("Unequipped Value:  {0}GP", totalValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
