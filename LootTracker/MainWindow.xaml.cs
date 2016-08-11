using System.Windows;
using System;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declare vars.
        public string savefilepath;
        public LootBook book = new LootBook();        
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        //MainWindow entry point.
        public MainWindow()
        {
            InitializeComponent();
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
            //headerlookup.Add("Item Assignments", "assignments");

            //Create a dataview and clear.
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
            dataView.SortDescriptions.Clear();

            //Create a new sortdescription.
            SortDescription sd = new SortDescription((headerlookup[sortBy]), direction);

            //Add the sortdescription to the dataview and refresh.
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            book = handler.ReadData();
            savefilepath = handler.filepath;
            listView.ItemsSource = book.lootlist;
        }

        //Event Handler for saving the open LootBook.
        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(book);
            savefilepath = handler.filepath;
        }

        //Event Handler for creating a new LootBook.
        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            book = new LootBook();
        }

        //Event Handler for adding a new player to the roster.
        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
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
                book.roster.AddPlayer(player);
            }
        }

        //Event handler for clicking the save menu item (known filepath).
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(true, book, savefilepath);
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
                book.AddLootItem(item);
            }
        }

        //Event handler for main window onload event.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = book.lootlist;
        }

        //Event handler to enable window dragging.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        //Event for clicking the close button.
        private void button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //Event handler for deleting an item from the list view.
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            if (!(book.lootlist.Count == 0) && !(index == -1))
            {
                book.RemoveLootItem(book.lootlist[index]);
            }

        }
        
        //Event handler for clicking listview headers (sort)
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
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
            if (!(book.lootlist.Count == 0) && !(index == -1))
            {
                book.lootlist[index].ModifiyAssignment("Dean", 2);
            }
        }
    }
}
