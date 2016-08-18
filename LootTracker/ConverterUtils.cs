using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;

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

    public sealed class W_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            Dictionary<string, int> d = values[0] as Dictionary<string, int>;
            Player p = values[1] as Player;
            string w = values[2] as string;
            decimal wgt;

            w = (w.Split(" ".ToCharArray()))[0];
            try { wgt = System.Convert.ToDecimal(w); }
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

    public sealed class V_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {

            Dictionary<string, int> d = values[0] as Dictionary<string, int>;
            Player p = values[1] as Player;
            string v = values[2] as string;
            decimal val;

            v = (v.Split(" ".ToCharArray()))[0];
            try { val = System.Convert.ToDecimal(v); }
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
}
