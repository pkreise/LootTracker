using System.Windows;
using System;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Class fields.
        string savefilepath;
        LootBook _book = new LootBook();
        bool windowLoaded;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        CollectionView view_items;
        bool canclick = true;

        //Class Properties.
        public LootBook book { get { return _book; } }
        
        //MainWindow entry point.
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _book;
        }
                
        //Player Loot Filter method.
        private bool PlayerLootFilter(object item)
        {
            bool playerexists;
            Player p = comboBox_Player.SelectedItem as Player;
            LootItem i = item as LootItem;

            if (item == null)
            {
                return false;
            }

            try
            {
                playerexists = i.assignments.ContainsKey(p.playername);
            }
            catch
            {
               return false;
            }

            if (p.playername == "Party" && i.unassignedcount > 0)
            {
                return true;
            }
            else if (playerexists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //Sorting method for the listview.
        private void Sort(string sortBy, ListSortDirection direction)
        {
            //Create a dictionary to lookup the binding values by the header passed in.
            Dictionary<string, string> headerlookup = new Dictionary<string, string>();
            headerlookup.Add("Name", "itemname");
            headerlookup.Add("Type", "loottype");
            headerlookup.Add("#", "count");
            headerlookup.Add("Unequipped", "unassignedcount");
            headerlookup.Add("Base Wgt", "baseweight");
            headerlookup.Add("Ttl Wgt", "totalweight");
            headerlookup.Add("Base Val", "basevalue");
            headerlookup.Add("Ttl Val", "totalvalue");
            headerlookup.Add("Assignments", "assignmentsstring");
            headerlookup.Add("Equipped", "assignmentsstring");
            headerlookup.Add("Equipped Wgt", "baseweight");
            headerlookup.Add("Equipped Val", "basevalue");

            //Create a dataview and clear.
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listView_Master.ItemsSource);
            dataView.SortDescriptions.Clear();

            //Create a new sortdescription.
            SortDescription sd = new SortDescription((headerlookup[sortBy]), direction);

            //Add the sortdescription to the dataview and refresh.
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        //Method to convert a byte array to a bitmap.
        private BitmapImage GetBitmap(byte[] Image)
        {
            MemoryStream ms = new MemoryStream(Image);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }

        //Event handler for dragging window.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            try
            {
                _book = handler.ReadData();
                int bookhash = _book.GetHashCode();
                DataContext = book;
                savefilepath = handler.filepath;
                listView_Master.ItemsSource = _book.lootlist;
                listView_Player.ItemsSource = _book.lootlist;
                comboBox_Player.ItemsSource = _book.playerlist;
                comboBox_Player.SelectedIndex = 0;
                view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView_Master.ItemsSource);
            }
            catch { }
            
        }

        //Event Handler for window loaded.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView_Master.ItemsSource = _book.lootlist;
            listView_Player.ItemsSource = _book.lootlist;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;

            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView_Master.ItemsSource);
            windowLoaded = true;
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
            listView_Master.ItemsSource = _book.lootlist;
            listView_Player.ItemsSource = _book.lootlist;
            comboBox_Player.SelectedIndex = -1;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView_Master.ItemsSource);   
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
            //Instantiate a new AddItem window.
            AddItem window = new AddItem();

            //Show the window.
            window.ShowDialog();

            if (!window.canceled)
            {
                LootItem item = new LootItem(window.textBox_Name.Text, window.comboBox_Type.Text, (Convert.ToInt32(window.textBox_Count.Text)), (Convert.ToInt32(window.textBox_BaseValue.Text)), (Convert.ToDecimal(window.textBox_BaseWeight.Text)));
                _book.AddLootItem(item);
            }

            view_items.Refresh();
        }
        
        //Event handler for clicking the close button.
        private void button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        
        //Event handler for clicking the item delete button.
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            _book.RemoveLootItem((listView_Master.SelectedItem as LootItem));
            button_Delete.IsEnabled = false;
            button_Assignments.IsEnabled = false;
            button_Increment.IsEnabled = false;
            button_Decrement.IsEnabled = false;
        }

        //Event handler for sorting the listview by column when the header is clicked.
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
                
        //Event handler for when the listview selection changes.
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_Delete.IsEnabled = true;
            button_Assignments.IsEnabled = true;
            button_Increment.IsEnabled = true;
            button_Decrement.IsEnabled = true;
        }

        //Event handler for clicking the browse button (player image).
        private void button_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filepicker = new OpenFileDialog();
            filepicker.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            filepicker.ShowDialog();

            //The user could cancel the file prompt, in which case
            //we don't want to try to read a null file.
            if (filepicker.FileName != "")
            {
                Player p = comboBox_Player.SelectedItem as Player;
                byte[] tempimage = File.ReadAllBytes(filepicker.FileName);
                p.UpdateImage(tempimage);
                p.hasimage = true;
                image_PlayerImage.Source = GetBitmap(p.characterimage);
            }
        }

        //Event handler for clicking the equip button (also when double clicking an item).
        private void button_Assignments_Click(object sender, RoutedEventArgs e)
        {
            LootItem l = (listView_Master.SelectedItem as LootItem);
            LootItem l_temp = l.Clone();
            AssignItem window_AssignItem = new AssignItem(_book.playerlist, l_temp);
            window_AssignItem.ShowDialog();

            if (window_AssignItem.isCancelled == false)
            {
                l.assignments = window_AssignItem.loot.assignments;
                l.CalculateUnassignedCount();
                view_items.Refresh();
                _book.NotifyPropertyChanged("lootlist");
            }
        }

        //Event handler for clicking the remove player button.
        private void button_RemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            RemovePlayer_Conf confwindow = new RemovePlayer_Conf();
            confwindow.ShowDialog();

            if (!confwindow.isCancelled)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                comboBox_Player.SelectedIndex = 0;
                _book.RemovePlayer(p);

                foreach (LootItem i in _book.lootlist)
                {
                    i.RemoveAssignment(p.playername);
                }
                view_items.Refresh();
                _book.NotifyPropertyChanged("lootlist");
            }
         }

        //Event handler to modify the listview filter when the tab selection changes.
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView_Master.ItemsSource);
            if (windowLoaded)
            {
                if (tabControl.SelectedIndex == 0)
                {
                    view_items.Filter = null;
                }
                else if (tabControl.SelectedIndex == 1)
                {
                    view_items.Filter = PlayerLootFilter;
                }
            }
        }

        //Event handler to maximize window with the top bar is double clicked.
        private void menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Regex.IsMatch(WindowState.ToString(), "Normal"))
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        //Event handler for clicking the minimize button.
        private void button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //Event handler for when the player combobox selection is changed.
        private void comboBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((comboBox_Player.SelectedIndex == 0))
            {
                button_RemovePlayer.IsEnabled = false;
            }
            else
            {
                button_RemovePlayer.IsEnabled = true;
            }
        }
        
        //Event Handlers for astral buttons.
        private void button_ast_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_ast_int.Text) > 0))
                {
                    p.Addast(Convert.ToInt32(textBox_ast_int.Text));
                   
                    textBox_ast_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_ast_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    if (canclick)
                    {
                        p.Addast(1);
                    }
                    else
                    {
                        canclick = true;
                    }
                    
                }
                
            }
                
        }
        private void button_ast_dec_Click(object sender, RoutedEventArgs e)
        {
             if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_ast_int.Text) > 0))
                {
                    if (((Convert.ToInt32(textBox_ast_int.Text)) - (Convert.ToInt32(AstVal.Text))) < 0)
                    {
                        p.Removeast(Convert.ToInt32(textBox_ast_int.Text));
                        
                        textBox_ast_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_ast_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removeast(Convert.ToInt32(AstVal.Text));
                        
                        textBox_ast_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(AstVal.Text)) > 0)
                    {
                        if (canclick)
                        {
                            p.Removeast(1);
                        }
                        else
                        {
                            canclick = true;
                        }

                    }
                    
                }

            }
        }
        private void button_ast_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                p.Addast(10);
                
            }
        }
        private void button_ast_dec_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                if ((10 - (Convert.ToInt32(AstVal.Text))) < 0)
                {
                    p.Removeast(10);
                    
                }
                else
                {
                    p.Removeast(Convert.ToInt32(AstVal.Text));
                    
                }
            }
        }
        private void textBox_ast_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_ast_int.Text);
                if (i < 0)
                {
                    textBox_ast_int.Text = "0";
                }
            }
            catch { textBox_ast_int.Text = "0";  canclick = false; }

        }
        private void textBox_ast_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        //Event Handlers for platinum buttons.
        private void button_plt_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_plt_int.Text) > 0))
                {
                    p.Addplt(Convert.ToInt32(textBox_plt_int.Text));
                    
                    textBox_plt_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_plt_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    if (canclick)
                    {
                        p.Addplt(1);
                    }
                    else
                    {
                        canclick = true;
                    }

                }

            }

        }
        private void button_plt_dec_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_plt_int.Text) > 0))
                {
                    if (((Convert.ToInt32(textBox_plt_int.Text)) - (Convert.ToInt32(PltVal.Text))) < 0)
                    {
                        p.Removeplt(Convert.ToInt32(textBox_plt_int.Text));
                        
                        textBox_plt_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_plt_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removeplt(Convert.ToInt32(PltVal.Text));
                        
                        textBox_plt_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(PltVal.Text)) > 0)
                    {
                        if (canclick)
                        {
                            p.Removeplt(1);
                        }
                        else
                        {
                            canclick = true;
                        }

                    }

                }

            }
        }
        private void button_plt_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                p.Addplt(10);
                
            }
        }
        private void button_plt_dec_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                if ((10 - (Convert.ToInt32(PltVal.Text))) < 0)
                {
                    p.Removeplt(10);
                    
                }
                else
                {
                    p.Removeplt(Convert.ToInt32(PltVal.Text));
                    
                }
            }
        }
        private void textBox_plt_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_plt_int.Text);
                if (i < 0)
                {
                    textBox_plt_int.Text = "0";
                }
            }
            catch { textBox_plt_int.Text = "0"; canclick = false; }

        }
        private void textBox_plt_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        //Event Handlers for gold buttons.
        private void button_gld_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_gld_int.Text) > 0))
                {
                    p.Addgld(Convert.ToInt32(textBox_gld_int.Text));
                    
                    textBox_gld_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_gld_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    if (canclick)
                    {
                        p.Addgld(1);
                    }
                    else
                    {
                        canclick = true;
                    }

                }

            }

        }
        private void button_gld_dec_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_gld_int.Text) > 0))
                {
                    if (((Convert.ToInt32(textBox_gld_int.Text)) - (Convert.ToInt32(GldVal.Text))) < 0)
                    {
                        p.Removegld(Convert.ToInt32(textBox_gld_int.Text));
                        
                        textBox_gld_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_gld_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removegld(Convert.ToInt32(GldVal.Text));
                        
                        textBox_gld_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(GldVal.Text)) > 0)
                    {
                        if (canclick)
                        {
                            p.Removegld(1);
                        }
                        else
                        {
                            canclick = true;
                        }

                    }

                }

            }
        }
        private void button_gld_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                p.Addgld(10);
                
            }
        }
        private void button_gld_dec_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                if ((10 - (Convert.ToInt32(GldVal.Text))) < 0)
                {
                    p.Removegld(10);
                    
                }
                else
                {
                    p.Removegld(Convert.ToInt32(GldVal.Text));
                    
                }
            }
        }
        private void textBox_gld_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_gld_int.Text);
                if (i < 0)
                {
                    textBox_gld_int.Text = "0";
                }
            }
            catch { textBox_gld_int.Text = "0"; canclick = false; }

        }
        private void textBox_gld_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        //Event Handlers for silver buttons.
        private void button_sil_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_sil_int.Text) > 0))
                {
                    p.Addsil(Convert.ToInt32(textBox_sil_int.Text));
                    
                    textBox_sil_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_sil_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    if (canclick)
                    {
                        p.Addsil(1);
                    }
                    else
                    {
                        canclick = true;
                    }

                }

            }

        }
        private void button_sil_dec_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_sil_int.Text) > 0))
                {
                    if (((Convert.ToInt32(textBox_sil_int.Text)) - (Convert.ToInt32(SilVal.Text))) < 0)
                    {
                        p.Removesil(Convert.ToInt32(textBox_sil_int.Text));
                        
                        textBox_sil_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_sil_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removesil(Convert.ToInt32(SilVal.Text));
                        
                        textBox_sil_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(SilVal.Text)) > 0)
                    {
                        if (canclick)
                        {
                            p.Removesil(1);
                        }
                        else
                        {
                            canclick = true;
                        }

                    }

                }

            }
        }
        private void button_sil_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                p.Addsil(10);
                
            }
        }
        private void button_sil_dec_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                if ((10 - (Convert.ToInt32(SilVal.Text))) < 0)
                {
                    p.Removesil(10);
                    
                }
                else
                {
                    p.Removesil(Convert.ToInt32(SilVal.Text));
                    
                }
            }
        }
        private void textBox_sil_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_sil_int.Text);
                if (i < 0)
                {
                    textBox_sil_int.Text = "0";
                }
            }
            catch { textBox_sil_int.Text = "0"; canclick = false; }

        }
        private void textBox_sil_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        //Event Handlers for copper buttons.
        private void button_cop_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_cop_int.Text) > 0))
                {
                    p.Addcop(Convert.ToInt32(textBox_cop_int.Text));
                    
                    textBox_cop_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_cop_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    if (canclick)
                    {
                        p.Addcop(1);
                    }
                    else
                    {
                        canclick = true;
                    }

                }

            }

        }
        private void button_cop_dec_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_cop_int.Text) > 0))
                {
                    if (((Convert.ToInt32(textBox_cop_int.Text)) - (Convert.ToInt32(CopVal.Text))) < 0)
                    {
                        p.Removecop(Convert.ToInt32(textBox_cop_int.Text));
                        
                        textBox_cop_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_cop_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removecop(Convert.ToInt32(CopVal.Text));
                        
                        textBox_cop_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(CopVal.Text)) > 0)
                    {
                        if (canclick)
                        {
                            p.Removecop(1);
                        }
                        else
                        {
                            canclick = true;
                        }

                    }

                }

            }
        }
        private void button_cop_inc_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                p.Addcop(10);
                
            }
        }
        private void button_cop_dec_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player p = (comboBox_Player.SelectedItem as Player);
                if ((10 - (Convert.ToInt32(CopVal.Text))) < 0)
                {
                    p.Removecop(10);
                    
                }
                else
                {
                    p.Removecop(Convert.ToInt32(CopVal.Text));
                    
                }
            }
        }
        private void textBox_cop_int_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(textBox_cop_int.Text);
                if (i < 0)
                {
                    textBox_cop_int.Text = "0";
                }
            }
            catch { textBox_cop_int.Text = "0"; canclick = false; }

        }
        private void textBox_cop_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }
    }
}
