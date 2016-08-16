using System.Windows;
using System;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declare class fields.
        string savefilepath;
        LootBook _book = new LootBook();
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        //Class properties.
        public LootBook book { get { return _book; } set { _book = value; } }
        //MainWindow entry point.
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Event handler for main window onload event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = book.lootlist;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
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

        //Sorting method for the listview.
        private void Sort(string sortBy, ListSortDirection direction)
        {
            //Create a dictionary to lookup the binding values by the header passed in.
            Dictionary<string, string> headerlookup = new Dictionary<string, string>();
            headerlookup.Add("Item Name", "itemname");
            headerlookup.Add("Item Type", "loottype");
            headerlookup.Add("Item Count", "count");
            headerlookup.Add("Unassigned Count", "unassignedcount");
            headerlookup.Add("Base Weight", "baseweight");
            headerlookup.Add("Total Weight", "totalweight");
            headerlookup.Add("Base Value", "basevalue");
            headerlookup.Add("Total Value", "totalvalue");
            
            //Create a dataview and clear.
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            dataView.SortDescriptions.Clear();

            //Create a new sortdescription.
            SortDescription sd = new SortDescription((headerlookup[sortBy]), direction);

            //Add the sortdescription to the dataview and refresh.
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        //Method to update player statistic elements.
        private void UpdateGP(Player p)
        {
            AstVal.Text = p.astral.ToString();
            PltVal.Text = p.platinum.ToString();
            GldVal.Text = p.gold.ToString();
            SilVal.Text = p.silver.ToString();
            CopVal.Text = p.copper.ToString();
            TtlVal.Text = p.totalGP.ToString();
        }
        
        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            _book = handler.ReadData();
            savefilepath = handler.filepath;
            listView.ItemsSource = _book.lootlist;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
        }

        //Event Handler for saving the open LootBook.
        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(_book);
            savefilepath = handler.filepath;
        }

        //Event Handler for creating a new LootBook.
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            _book = new LootBook();
            listView.ItemsSource = _book.lootlist;
            comboBox_Player.SelectedIndex = -1;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
        }

        //Event Handler for adding a new player to the roster.
        private void button_AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            //Instantiate a new AddPlayer window.
            AddPlayer window = new AddPlayer();

            //Show the window.
            window.ShowDialog();

            //If the AddPlayer window wasn't cancelled, let's add
            //the player to the roster using the values entered.
            if (!window.cancelled)
            {
                //Instantiate the player object
                Player player = new Player(window.textBox_Player.Text, window.textBox_Character.Text);

                //Update the image if one was uploaded at player creation.
                if (window.hasimage == true)
                {
                    player.UpdateImage(window.imagearray);
                }
                _book.AddPlayer(player);
            }
        }

        //Event handler for clicking the save menu item (known filepath).
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(true, _book, savefilepath);
        }

        //Event handler for clicking the exit menu item.
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //Event handler for clicking the new item menu item.
        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            //Instantiate a new AddPlayer window.
            AddItem window = new AddItem();

            //Show the window.
            window.ShowDialog();

            if (!window.canceled)
            {
                LootItem item = new LootItem(window.textBox_Name.Text, window.comboBox_Type.Text, (Convert.ToInt32(window.textBox_Count.Text)), (Convert.ToInt32(window.textBox_BaseValue.Text)), (Convert.ToDecimal(window.textBox_BaseWeight.Text)));
                _book.AddLootItem(item);
            }
        }
        
        /// <summary>
        /// Event handler for clicking the mainwindow close button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        
        /// <summary>
        /// Event handler for deleting an item from the list view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            _book.RemoveLootItem((listView.SelectedItem as LootItem));
            button_Delete.IsEnabled = false;
            button_Assignments.IsEnabled = false;
            button_Increment.IsEnabled = false;
            button_Decrement.IsEnabled = false;

        }

        /// <summary>
        /// Event handler for clicking listview headers (sort)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewColumnHeader_Clicked(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
                
        /// <summary>
        /// Event Handler for clicking the assignement context menu item or tool bar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifiyAssignment_Click(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if (!(_book.lootlist.Count == 0) && !(index == -1))
            {
                _book.lootlist[index].ModifiyAssignment("Dean", 2);
            }
        }

        /// <summary>
        /// Event handler for enabling items when an item is selected from the list view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_Delete.IsEnabled = true;
            button_Assignments.IsEnabled = true;
            button_Increment.IsEnabled = true;
            button_Decrement.IsEnabled = true;
        }

        /// <summary>
        /// Event handler for populating player data when a player is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player player = comboBox_Player.SelectedItem as Player;
                UpdateGP(player);

                //If the player has an image, we should display it.
                if (player.hasimage)
                {
                    MemoryStream ms = new MemoryStream(player.characterimage);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    image_PlayerImage.Source = bitmap;
                }
                else
                {
                    image_PlayerImage.Source = null;
                }
            }
        }

        private void button_ast_inc_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).AddAstral(Convert.ToInt32(textBox_ast_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_ast_int.Text = "0";
        }
        private void button_ast_dec_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).RemoveAstral(Convert.ToInt32(textBox_ast_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_ast_int.Text = "0";
        }
        private void textBox_ast_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try { Convert.ToInt32(textBox_ast_int.Text); }
            catch { textBox_ast_int.Text = "0"; }

        }
        private void textBox_ast_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void button_ast_inc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddAstral(1);
                UpdateGP(p);
            }
        }
        private void button_ast_dec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1) && (p.astral > 0))
            {
                p.RemoveAstral(1);
                UpdateGP(p);
            }
        }

        private void button_plt_inc_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).AddPlatinum(Convert.ToInt32(textBox_plt_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_plt_int.Text = "0";
        }
        private void button_plt_dec_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).RemovePlatinum(Convert.ToInt32(textBox_plt_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_plt_int.Text = "0";
        }
        private void textBox_plt_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try { Convert.ToInt32(textBox_plt_int.Text); }
            catch { textBox_plt_int.Text = "0"; }

        }
        private void textBox_plt_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void button_plt_inc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddPlatinum(1);
                UpdateGP(p);
            }
        }
        private void button_plt_dec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1) && (p.platinum > 0))
            {
                p.RemovePlatinum(1);
                UpdateGP(p);
            }
        }

        private void button_gld_inc_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).AddGold(Convert.ToInt32(textBox_gld_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_gld_int.Text = "0";
        }
        private void button_gld_dec_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).RemoveGold(Convert.ToInt32(textBox_gld_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_gld_int.Text = "0";
        }
        private void textBox_gld_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try { Convert.ToInt32(textBox_gld_int.Text); }
            catch { textBox_gld_int.Text = "0"; }

        }
        private void textBox_gld_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void button_gld_inc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddGold(1);
                UpdateGP(p);
            }
        }
        private void button_gld_dec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1) && (p.gold > 0))
            {
                p.RemoveGold(1);
                UpdateGP(p);
            }
        }

        private void button_sil_inc_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).AddSilver(Convert.ToInt32(textBox_sil_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_sil_int.Text = "0";
        }
        private void button_sil_dec_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).RemoveSilver(Convert.ToInt32(textBox_sil_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_sil_int.Text = "0";
        }
        private void textBox_sil_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try { Convert.ToInt32(textBox_sil_int.Text); }
            catch { textBox_sil_int.Text = "0"; }

        }
        private void textBox_sil_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void button_sil_inc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddSilver(1);
                UpdateGP(p);
            }
        }
        private void button_sil_dec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1) && (p.silver > 0))
            {
                p.RemoveSilver(1);
                UpdateGP(p);
            }
        }

        private void button_cop_inc_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).AddCopper(Convert.ToInt32(textBox_cop_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_cop_int.Text = "0";
        }
        private void button_cop_dec_Click(object sender, RoutedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                (comboBox_Player.SelectedItem as Player).RemoveCopper(Convert.ToInt32(textBox_cop_int.Text));
                UpdateGP(comboBox_Player.SelectedItem as Player);
            }
            textBox_cop_int.Text = "0";
        }
        private void textBox_cop_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try { Convert.ToInt32(textBox_cop_int.Text); }
            catch { textBox_cop_int.Text = "0"; }

        }
        private void textBox_cop_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        private void button_cop_inc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddCopper(1);
                UpdateGP(p);
            }
        }
        private void button_cop_dec_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1) && (p.copper > 0))
            {
                p.RemoveCopper(1);
                UpdateGP(p);
            }
        }




        private void button_Assignments_Click(object sender, RoutedEventArgs e)
        {

            Window1 window_Assign = new Window1(_book.playerlist,(listView.SelectedItem as LootItem));
            window_Assign.ShowDialog();
        }

        private void button_RemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            book.RemovePlayer(comboBox_Player.SelectedItem as Player);
            comboBox_Player.SelectedIndex = 0;
        }

        private void button_ast_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Player p = (comboBox_Player.SelectedItem as Player);
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                p.AddAstral(10);
                UpdateGP(p);
            }
        }
    
    }
}
