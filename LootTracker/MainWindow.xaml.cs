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
        //Declare class fields.
        string savefilepath;
        LootBook _book = new LootBook();
        bool windowLoaded;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        CollectionView view_items;

        
        //MainWindow entry point.
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            listView.ItemsSource = _book.lootlist;
            listView_Player.ItemsSource = _book.lootlist;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;

            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
            windowLoaded = true;
        }



        private bool PlayerLootFilter(object item)
        {
            bool playerexists;
            if (item == null)
            {
                return false;
            }

            try
            {
                playerexists = (item as LootItem).assignments.ContainsKey((comboBox_Player.SelectedItem as Player).playername);
            }
            catch
            {
                return false;
            }

            if (playerexists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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
            AstVal.Text = p.ast.ToString();
            PltVal.Text = p.plt.ToString();
            GldVal.Text = p.gld.ToString();
            SilVal.Text = p.sil.ToString();
            CopVal.Text = p.cop.ToString();
            TtlVal.Text = p.totalGP.ToString();
        }
        
        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            _book = handler.ReadData();
            savefilepath = handler.filepath;
            listView.ItemsSource = _book.lootlist;
            listView_Player.ItemsSource = _book.lootlist;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
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
            listView_Player.ItemsSource = _book.lootlist;
            comboBox_Player.SelectedIndex = -1;
            comboBox_Player.ItemsSource = _book.playerlist;
            comboBox_Player.SelectedIndex = 0;
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);   
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
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            _book.RemoveLootItem((listView.SelectedItem as LootItem));
            button_Delete.IsEnabled = false;
            button_Assignments.IsEnabled = false;
            button_Increment.IsEnabled = false;
            button_Decrement.IsEnabled = false;
        }

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
                
        private void ModifiyAssignment_Click(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if (!(_book.lootlist.Count == 0) && !(index == -1))
            {
                _book.lootlist[index].ModifiyAssignment("Dean", 2);
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_Delete.IsEnabled = true;
            button_Assignments.IsEnabled = true;
            button_Increment.IsEnabled = true;
            button_Decrement.IsEnabled = true;
        }

        private BitmapImage GetBitmap(byte[] Image)
        {
            MemoryStream ms = new MemoryStream(Image);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(comboBox_Player.SelectedIndex == -1))
            {
                Player player = comboBox_Player.SelectedItem as Player;
                UpdateGP(player);

                //If the player has an image, we should display it.
                if (player.hasimage)
                {
                    image_PlayerImage.Source = GetBitmap(player.characterimage);
                }
                else
                {
                    image_PlayerImage.Source = null;
                }
            }
        }

        private void button_ast_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_ast_int.Text) > 0))
                {
                    p.Addast(Convert.ToInt32(textBox_ast_int.Text));
                    UpdateGP(p);
                    textBox_ast_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_ast_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    p.Addast(1);
                    UpdateGP(p);
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
                        UpdateGP(p);
                        textBox_ast_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_ast_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removeast(Convert.ToInt32(AstVal.Text));
                        UpdateGP(p);
                        textBox_ast_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(AstVal.Text)) > 0)
                    {
                        p.Removeast(1);
                        UpdateGP(p);
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
                UpdateGP(p);
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
                    UpdateGP(p);
                }
                else
                {
                    p.Removeast(Convert.ToInt32(AstVal.Text));
                    UpdateGP(p);
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
            catch { textBox_ast_int.Text = "0"; }

        }
        private void textBox_ast_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        private void button_plt_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_plt_int.Text) > 0))
                {
                    p.Addplt(Convert.ToInt32(textBox_plt_int.Text));
                    UpdateGP(p);
                    textBox_plt_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_plt_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    p.Addplt(1);
                    UpdateGP(p);
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
                        UpdateGP(p);
                        textBox_plt_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_plt_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removeplt(Convert.ToInt32(PltVal.Text));
                        UpdateGP(p);
                        textBox_plt_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(PltVal.Text)) > 0)
                    {
                        p.Removeplt(1);
                        UpdateGP(p);
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
                UpdateGP(p);
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
                    UpdateGP(p);
                }
                else
                {
                    p.Removeplt(Convert.ToInt32(PltVal.Text));
                    UpdateGP(p);
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
            catch { textBox_plt_int.Text = "0"; }

        }
        private void textBox_plt_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        private void button_gld_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_gld_int.Text) > 0))
                {
                    p.Addgld(Convert.ToInt32(textBox_gld_int.Text));
                    UpdateGP(p);
                    textBox_gld_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_gld_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    p.Addgld(1);
                    UpdateGP(p);
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
                        UpdateGP(p);
                        textBox_gld_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_gld_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removegld(Convert.ToInt32(GldVal.Text));
                        UpdateGP(p);
                        textBox_gld_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(GldVal.Text)) > 0)
                    {
                        p.Removegld(1);
                        UpdateGP(p);
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
                UpdateGP(p);
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
                    UpdateGP(p);
                }
                else
                {
                    p.Removegld(Convert.ToInt32(GldVal.Text));
                    UpdateGP(p);
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
            catch { textBox_gld_int.Text = "0"; }

        }
        private void textBox_gld_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        private void button_sil_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_sil_int.Text) > 0))
                {
                    p.Addsil(Convert.ToInt32(textBox_sil_int.Text));
                    UpdateGP(p);
                    textBox_sil_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_sil_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    p.Addsil(1);
                    UpdateGP(p);
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
                        UpdateGP(p);
                        textBox_sil_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_sil_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removesil(Convert.ToInt32(SilVal.Text));
                        UpdateGP(p);
                        textBox_sil_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(SilVal.Text)) > 0)
                    {
                        p.Removesil(1);
                        UpdateGP(p);
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
                UpdateGP(p);
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
                    UpdateGP(p);
                }
                else
                {
                    p.Removesil(Convert.ToInt32(SilVal.Text));
                    UpdateGP(p);
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
            catch { textBox_sil_int.Text = "0"; }

        }
        private void textBox_sil_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }

        private void button_cop_inc_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Player.SelectedIndex != -1)
            {
                Player p = comboBox_Player.SelectedItem as Player;
                if ((Convert.ToInt32(textBox_cop_int.Text) > 0))
                {
                    p.Addcop(Convert.ToInt32(textBox_cop_int.Text));
                    UpdateGP(p);
                    textBox_cop_int.Text = "0";
                }
                else if ((Convert.ToInt32(textBox_cop_int.Text) < 0))
                {
                    return;
                }
                else
                {
                    p.Addcop(1);
                    UpdateGP(p);
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
                        UpdateGP(p);
                        textBox_cop_int.Text = "0";
                    }
                    else if ((Convert.ToInt32(textBox_cop_int.Text) < 0))
                    {
                        return;
                    }
                    else
                    {
                        p.Removecop(Convert.ToInt32(CopVal.Text));
                        UpdateGP(p);
                        textBox_cop_int.Text = "0";
                    }
                }
                else
                {
                    if ((Convert.ToInt32(CopVal.Text)) > 0)
                    {
                        p.Removecop(1);
                        UpdateGP(p);
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
                UpdateGP(p);
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
                    UpdateGP(p);
                }
                else
                {
                    p.Removecop(Convert.ToInt32(CopVal.Text));
                    UpdateGP(p);
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
            catch { textBox_cop_int.Text = "0"; }

        }
        private void textBox_cop_int_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as System.Windows.Controls.TextBox).SelectAll();
        }


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

        private void button_Assignments_Click(object sender, RoutedEventArgs e)
        {
            LootItem originalitem = listView.SelectedItem as LootItem;
            AssignItem window_AssignItem = new AssignItem(_book.playerlist, originalitem);
            window_AssignItem.ShowDialog();

            if (window_AssignItem.isCancelled == false)
            {
                originalitem.assignments = window_AssignItem.loot.assignments;
                originalitem.CalculateUnassignedCount();
                originalitem.CalculateUnassignedCount();
                view_items.Refresh();
            }
        }

        private void button_RemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            Player p = comboBox_Player.SelectedItem as Player;
            comboBox_Player.SelectedIndex = 0;
            _book.RemovePlayer(p);
            
            foreach (LootItem i in _book.lootlist)
            {
                i.RemoveAssignment(p.playername);
                i.CalculateUnassignedCount();
                i.CalculateUnassignedValue();
            }
            view_items.Refresh();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
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

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
    }
}
