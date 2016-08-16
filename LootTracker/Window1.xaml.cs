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
using System.Collections.ObjectModel;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        ObservableCollection<Player> _players;
        LootItem _loot;

        public LootItem loot { get { return _loot; } }

        public Window1(ObservableCollection<Player> players, LootItem loot)
        {
            InitializeComponent();
            _players = players;
            _loot = loot;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            combobox_Player.ItemsSource = _players;
        }
    }
}
