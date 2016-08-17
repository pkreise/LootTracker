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
        bool _isCancelled = false;

        public LootItem loot { get { return _loot; } }
        public bool isCancelled { get { return _isCancelled; } }

        public Window1(ObservableCollection<Player> players, LootItem loot)
        {
            InitializeComponent();
            _players = players;
            _loot = loot;
        }


        private void UpdateHeader()
        {
            textBlock_Item.Text = loot.itemname;
            textBlock_AvailableVal.Text = loot.unassignedcount.ToString();
            if (combobox_Player.SelectedIndex != -1)
            {
                if (loot.assignments.ContainsKey((combobox_Player.SelectedItem as Player).playername))
                {
                    textBlock_AssignedVal.Text = (loot.assignments[(combobox_Player.SelectedItem as Player).playername]).ToString();                    
                }
                else
                {
                    textBlock_AssignedVal.Text = "0";
                }
            }
            else
            {
                textBlock_AssignedVal.Text = "0";

            }
            
        }

        /// <summary>
        /// Event handler to enable window dragging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            combobox_Player.ItemsSource = _players;
            UpdateHeader();
        }


        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            _isCancelled = true;
            Close();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
           Close();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void combobox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player player = combobox_Player.SelectedItem as Player;
                if (loot.assignments.ContainsKey(player.playername))
                {
                    textBlock_AssignedVal.Text = (loot.assignments[player.playername]).ToString();
                }
                else
                {
                    textBlock_AssignedVal.Text = "0";
                }
            }
        }

        private void button_inc_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player p = combobox_Player.SelectedItem as Player;
                int currentplayercount;
                if (loot.assignments.ContainsKey(p.playername))
                {
                    currentplayercount = loot.assignments[p.playername];
                }
                else
                {
                    currentplayercount = 0;
                }
                int modcount = Convert.ToInt32(textBox_Count.Text);

                if (modcount > 0)
                {
                    if ((loot.unassignedcount - modcount) < 0)
                    {
                        loot.ModifiyAssignment(p.playername, (loot.unassignedcount + currentplayercount));
                    }
                    else
                    {
                        loot.ModifiyAssignment(p.playername, (modcount + currentplayercount));
                    }
                    textBox_Count.Text = "0";
                }
                else
                {
                    if (loot.unassignedcount > 0)
                    {
                        loot.ModifiyAssignment(p.playername, (currentplayercount + 1));
                    }
                }
            }
            UpdateHeader();
        }
        private void button_dec_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player p = combobox_Player.SelectedItem as Player;
                int currentplayercount;
                if (loot.assignments.ContainsKey(p.playername))
                {
                    currentplayercount = loot.assignments[p.playername];
                }
                else
                {
                    currentplayercount = 0;
                }
                int modcount = Convert.ToInt32(textBox_Count.Text);

                if (modcount > 0)
                {
                    if ((currentplayercount - modcount) < 0)
                    {
                        loot.ModifiyAssignment(p.playername, 0);
                    }
                    else
                    {
                        loot.ModifiyAssignment(p.playername, (currentplayercount - modcount));
                    }
                    textBox_Count.Text = "0";
                }
                else
                {
                    if (currentplayercount > 0)
                    {
                        loot.ModifiyAssignment(p.playername, (currentplayercount - 1));
                    }
                }
            }
            UpdateHeader();
        }



        
    }
}
    
