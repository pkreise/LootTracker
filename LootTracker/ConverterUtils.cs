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
    //Background color converter.
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

    //Foreground color converter.
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

   //Player item count converter.
    public sealed class C_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            LootItem i = values[0] as LootItem;
            Player p = values[1] as Player;

            if (p == null)
            {
                return 0;
            }

            if (i.assignments.ContainsKey(p.playername))
            {
                return (i.assignments[(p.playername)]);
            }
            else if (p.playername == "Party")
            {
                return i.unassignedcount;
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

   //Player item weight converter.
    public sealed class W_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            LootItem i = values[0] as LootItem;
            Player p = values[1] as Player;

            if (p == null)
            {
                return 0;
            }

            if (i.assignments.ContainsKey(p.playername))
            {
                return i.assignments[(p.playername)] * i.baseweight;
            }
            else if (p.playername == "Party")
            {
                return i.baseweight * i.unassignedcount;
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

    //Player item value converter.
    public sealed class V_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {

            LootItem i = values[0] as LootItem;
            Player p = values[1] as Player;
            if (p == null)
            {
                return 0;
            }

            if (i.assignments.ContainsKey(p.playername))
            {
                return i.assignments[(p.playername)] * i.basevalue;
            }
            else if (p.playername == "Party")
            {
                return i.basevalue * i.unassignedcount;
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

    //Sell Value converter.
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

            return String.Format("Unequipped Value:  {0} GP", totalValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Player total weight converter.
    public sealed class TtlWgt_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            ObservableCollection<LootItem> lootlist = values[0] as ObservableCollection<LootItem>;
            Player p = values[2] as Player;
            decimal ttlwgt = 0;

            if (p != null && lootlist != null)
            {
                foreach (LootItem l in lootlist)
                {
                    if (l.assignments.ContainsKey(p.playername))
                    {
                        ttlwgt += l.assignments[(p.playername)] * l.baseweight;
                    }
                    else if (p.playername == "Party")
                    {
                        ttlwgt += l.unassignedcount * l.baseweight;
                    }
                }
             }

            if (p != null)
            {
                if (p.GPCarried == true)
                {
                    double GPwgt = (p.ast * .02) + (p.plt * .02) + (p.gld * .02) + (p.sil * .02) + (p.cop * .02);
                    decimal GPwgt_dec = System.Convert.ToDecimal(GPwgt);
                    ttlwgt += GPwgt_dec;
                }
            }

            return ttlwgt;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Player total value converter.
    public sealed class TtlVal_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            ObservableCollection<LootItem> lootlist = values[0] as ObservableCollection<LootItem>;
            Player p = values[2] as Player;
            decimal ttlval = 0;

            if (p != null && lootlist != null)
            {
                foreach (LootItem l in lootlist)
                {
                    if (l.assignments.ContainsKey(p.playername))
                    {
                        ttlval += l.assignments[(p.playername)] * l.basevalue;
                    }
                    else if (p.playername == "Party")
                    {
                        ttlval += l.basevalue * l.unassignedcount;
                    }
                }
            }

           return ttlval;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Player equipped fontweight converter.
    public sealed class A_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            LootItem l = values[0] as LootItem;
            string p = values[1] as string;
            
            if (l.assignments.ContainsKey(p) || (p == "Party" && l.unassignedcount > 0))
            {
                return FontWeights.Bold;
            }
            else 
            {
                return FontWeights.Normal;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Item sell value converter.
    public sealed class ItemSellValue_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            LootItem l = values[0] as LootItem;
            int p;
            try { p = System.Convert.ToInt32(values[1]); }
            catch { p = 0; }
            
            if (l != null)
            {
                return l.unassignedvalue * (p * .01);
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

    //Total Item sell value converter.
    public sealed class TtlItemSellValue_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targettype, object parameter, CultureInfo culture)
        {
            ObservableCollection<LootItem> items = values[0] as ObservableCollection<LootItem>;
            int p;
            double ttlsellvalue = 0;
            
            try { p = System.Convert.ToInt32(values[1]); }
            catch { p = 0; }

            if (items.Count > 0)
            {
                foreach (LootItem i in items)
                {
                    ttlsellvalue += i.unassignedvalue;
                }
                return ttlsellvalue * (p * .01);
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
