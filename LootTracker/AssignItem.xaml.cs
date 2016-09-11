using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for AssignItem.xaml
    /// </summary>
    public partial class AssignItem : Window
    {
        //Class fields.
        LootItem _loot;
        ObservableCollection<Player> _players;
        bool _isCancelled = false;

        //Class properties.
        public LootItem loot { get { return _loot; } }
        public ObservableCollection<Player> players { get { return _players; } }
        public bool isCancelled { get { return _isCancelled; } }

        //Constructor.
        public AssignItem(ObservableCollection<Player> players, LootItem l)
        {
            InitializeComponent();
            
            _players = players;
            _loot = l;
        }
        
        //Update Header method. Should just bind this.  To do.        
        private void UpdateHeader()
        {
            label_Item.Content = _loot.itemname;
            textBlock_AvailableVal.Text = _loot.unassignedcount.ToString();
            if (combobox_Player.SelectedIndex != -1)
            {
                if (_loot.assignments.ContainsKey((combobox_Player.SelectedItem as Player).playername))
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

        //Method for mouse click/drag to move window.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        //Event handler for window loaded event.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateHeader();
            combobox_Player.ItemsSource = _players;            
        }
        
        //Event handler for clicking the close button.
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            _isCancelled = true;
            Close();
        }

        //Event handler for clicking the OK button.
        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
           Close();
        }

        //Event handler to validate that the textbox has only interger input.
        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_Count.Text);
                if (i < 0 )
                {
                    textBox_Count.Text = "0";
                }
            }
            catch { textBox_Count.Text = "0"; }
        }

        //Event handler for when the player combobox changes.
        private void combobox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player player = combobox_Player.SelectedItem as Player;
                if (_loot.assignments.ContainsKey(player.playername))
                {
                    textBlock_AssignedVal.Text = (loot.assignments[player.playername]).ToString();
                    button_inc.IsEnabled = true;
                    button_dec.IsEnabled = true;
                }
                else if (player.playername == "Party")
                {
                    textBlock_AssignedVal.Text = loot.unassignedcount.ToString();
                    button_inc.IsEnabled = false;
                    button_dec.IsEnabled = false;
                }
                else
                {
                    textBlock_AssignedVal.Text = "0";
                    button_inc.IsEnabled = true;
                    button_dec.IsEnabled = true;
                }
            }
        }

        //Event handler for clicking the increment (+) button.
        private void button_inc_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player p = combobox_Player.SelectedItem as Player;
                Player p_party = combobox_Player.Items[0] as Player;

                int currentplayercount;
                if (_loot.assignments.ContainsKey(p.playername))
                {
                    currentplayercount = _loot.assignments[p.playername];
                }
                else
                {
                    currentplayercount = 0;
                }
                int modcount = Convert.ToInt32(textBox_Count.Text);

                if (modcount > 0)
                {
                    if ((_loot.unassignedcount - modcount) < 0)
                    {
                        _loot.ModifiyAssignment(p.playername, (_loot.unassignedcount + currentplayercount));
                        p.NotifyPropertyChanged("playername");
                        p_party.NotifyPropertyChanged("playername");
                    }
                    else
                    {
                        _loot.ModifiyAssignment(p.playername, (modcount + currentplayercount));
                        p.NotifyPropertyChanged("playername");
                        p_party.NotifyPropertyChanged("playername");
                    }
                    textBox_Count.Text = "0";
                }
                else
                {
                    if (_loot.unassignedcount > 0)
                    {
                        _loot.ModifiyAssignment(p.playername, (currentplayercount + 1));
                        p.NotifyPropertyChanged("playername");
                        p_party.NotifyPropertyChanged("playername");
                    }
                }
            }
            UpdateHeader();
        }

        //Event handler for clicking the decrement (-) button.
        private void button_dec_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_Player.SelectedIndex != -1)
            {
                Player p = combobox_Player.SelectedItem as Player;
                Player p_party = combobox_Player.Items[0] as Player;
                int currentplayercount;
                if (_loot.assignments.ContainsKey(p.playername))
                {
                    currentplayercount = _loot.assignments[p.playername];
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
                        _loot.assignments.Remove(p.playername);
                    }
                    else
                    {
                        _loot.ModifiyAssignment(p.playername, (currentplayercount - modcount));
                        p.NotifyPropertyChanged("playername");
                        p_party.NotifyPropertyChanged("playername");

                    }
                    textBox_Count.Text = "0";
                }
                else
                {
                    if (currentplayercount > 0)
                    {
                        if (currentplayercount == 1)
                        {
                            _loot.RemoveAssignment(p.playername);
                            p.NotifyPropertyChanged("playername");
                            p_party.NotifyPropertyChanged("playername");
                        }
                        else
                        {
                            _loot.ModifiyAssignment(p.playername, (currentplayercount - 1));
                            p.NotifyPropertyChanged("playername");
                            p_party.NotifyPropertyChanged("playername");
                        }
                    }
                }
            }
            UpdateHeader();
        }
    }
}
    
