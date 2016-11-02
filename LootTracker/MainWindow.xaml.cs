using System.Windows;
using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Class fields.
        private ViewModel viewModel = new ViewModel();
        private bool canclick = true;
        
        //MainWindow entry point.
        public MainWindow()
        {
            DataContext = viewModel;            
            InitializeComponent();
            comboBox_Player.SelectedIndex = 0;      
        }


        /// <summary>
        /// LootBook control event handlers.
        /// </summary>
        //Event Handler for creating a new, or opening an existing, lootbook.
        private void OpenOrNewLootBook(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Button).Name == "button_Open")
            {
                viewModel.OpenLootBook();
            }
            else
            {
                viewModel.NewLootBook();
            }
            
            listView_Master.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
            listView_Player.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
            comboBox_Player.GetBindingExpression(System.Windows.Controls.ComboBox.ItemsSourceProperty).UpdateTarget();
            label_SellValue.GetBindingExpression(System.Windows.Controls.Label.ContentProperty).UpdateTarget();
            comboBox_Player.SelectedIndex = 0;
        }

        //Event Handler for saving the open LootBook.
        private void SaveLootBook(object sender, RoutedEventArgs e)
        {
            viewModel.SaveLootBook();
        }


        /// <summary>
        /// Item control event handlers.
        /// </summary>
        //Event handler for clicking the new item menu item.
        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            
            //Instantiate a new AddItem window.
            AddItem window = new AddItem();

            //Show the window.
            window.ShowDialog();

            if (!window.canceled)
            {
                LootItem item = new LootItem(window.textBox_Name.Text, window.comboBox_Type.Text, (Convert.ToInt32(window.textBox_Count.Text)), (Convert.ToInt32(window.textBox_BaseValue.Text)), (Convert.ToDecimal(window.textBox_BaseWeight.Text)), window.textBox_Notes.Text);
                viewModel.AddItemToModel(item);
            }

            viewModel.NotifyPropertyChanged("LootList");
            viewModel.RefreshView();
        }
                
        //Event handler for clicking the item delete button.
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            //TODO:  Need a confirmation prompt window here.
            if (listView_Master.SelectedItems.Count > 1)
            {
                List<LootItem> itemsToDelete = new List<LootItem>();
                foreach (LootItem i in listView_Master.SelectedItems)
                {
                    itemsToDelete.Add(i);
                }

                foreach (LootItem i in itemsToDelete)
                {
                    viewModel.RemoveItemFromModel(i);
                }
            }
            else
            {
                viewModel.RemoveItemFromModel(listView_Master.SelectedItem as LootItem);
            }
                        
            button_Delete.IsEnabled = false;
            button_Assignments.IsEnabled = false;

            viewModel.NotifyPropertyChanged("LootList");
            viewModel.RefreshView();
         }

        //Event handler for clicking the edit button.
        private void button_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (listView_Master.SelectedItems.Count == 1 && listView_Master.SelectedIndex != -1)
            {
                //Instantiate a new AddItem window.
                LootItem i = listView_Master.SelectedItem as LootItem;
                AddItem window = new AddItem(i);

                //Show the window.
                window.ShowDialog();

                if (!window.canceled)
                {
                    //Set the properties on the lootitem.
                    i.itemname = window.textBox_Name.Text;
                    i.loottype = window.comboBox_Type.Text;
                    int tempCount = Convert.ToInt32(window.textBox_Count.Text);
                    if (tempCount < i.assignedcount && tempCount < i.count)
                    {
                        i.count = i.assignedcount;
                    }
                    else
                    {
                        i.count = (Convert.ToInt32(window.textBox_Count.Text));
                    }

                    i.basevalue = (Convert.ToInt32(window.textBox_BaseValue.Text));
                    i.baseweight = (Convert.ToDecimal(window.textBox_BaseWeight.Text));
                    i.notes = window.textBox_Notes.Text;

                    //Refresh the values and notify the UI that the lootlist has updated.
                    i.CalculateAllValues();
                }

                viewModel.NotifyPropertyChanged("LootList");
                viewModel.RefreshView();
            }
        }

        //Event handler for clicking the equip button (also when double clicking an item).
        private void button_Assignments_Click(object sender, RoutedEventArgs e)
        {
            if (listView_Master.SelectedItems.Count == 1 && listView_Master.SelectedIndex != -1)
            {
                LootItem l = (listView_Master.SelectedItem as LootItem);
                LootItem l_temp = l.Clone();
                AssignItem window_AssignItem = new AssignItem(viewModel.PlayerList, l_temp);
                window_AssignItem.ShowDialog();

                if (window_AssignItem.isCancelled == false)
                {
                    //Update the assignments.
                    l.assignments = l_temp.assignments;

                    //Refresh the values and notify the UI that the lootlist has updated.
                    l.CalculateAllValues();

                    //Refresh the view.
                    viewModel.RefreshView();
                    viewModel.NotifyPropertyChanged("LootList");
                }
            }
        }

        //Event handler for clicking the Sell items button.
        private void button_Sell_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<LootItem> items = new ObservableCollection<LootItem>();
            foreach (LootItem i in listView_Master.SelectedItems)
            {
                items.Add(i);
            }
            SellItems sellItemWindow = new SellItems(items);
            sellItemWindow.ShowDialog();

            decimal totalvalue = 0;
            decimal sellvalue = 0;
            Player party = viewModel.PlayerList[0] as Player;

            if (!sellItemWindow.isCancelled)
            {
                foreach (LootItem i in items)
                {
                    if (i.unassignedcount > 0)
                    {
                        totalvalue += (i.unassignedcount * i.basevalue);
                        i.DecrementCount(i.unassignedcount);
                    }
                }

                if (totalvalue > 0)
                {
                    sellvalue = Math.Round((totalvalue * (Convert.ToDecimal(sellItemWindow.textBox_SellPercent.Text) / 100)), 2);
                    int gld = 0;
                    int sil = 0;
                    int cop = 0;

                    gld = Convert.ToInt32(Math.Floor(sellvalue));
                    if (gld > 0)
                    {
                        sellvalue = sellvalue - gld;
                        viewModel.ModifyCurrency(party, CurrencyType.Gld, gld);
                    }

                    if (sellvalue > 0)
                    {
                        sil = Convert.ToInt32(Math.Floor(Decimal.Divide(sellvalue, 0.1M)));
                        sellvalue = sellvalue - (sil * .1M);
                        viewModel.ModifyCurrency(party, CurrencyType.Sil, sil);
                    }

                    if (sellvalue > 0)
                    {
                        cop = Convert.ToInt32(Math.Floor(Decimal.Divide(sellvalue, 0.01M)));
                        sellvalue = sellvalue - (cop * .01M);
                        viewModel.ModifyCurrency(party, CurrencyType.Cop, cop);
                    }
                }

                viewModel.NotifyPropertyChanged("LootList");
                viewModel.RefreshView();
            }
        }


        /// <summary>
        /// ListView control event handlers.
        /// </summary>
        //Event handler for sorting the listview by column when the header is clicked.
        private void GridViewColumnHeader_Clicked(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    /*if (headerClicked != _lastHeaderClicked)
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
                    _lastDirection = direction;*/
                }
            }
        }
                
        //Event handler for when the listview selection changes.
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         if (listView_Master.SelectedIndex != -1)
            {
               if (listView_Master.SelectedItems.Count > 1)
                {
                    button_Assignments.IsEnabled = false;
                    button_Edit.IsEnabled = false;

                }
                else if (listView_Master.SelectedItems.Count == 1)
                {
                    button_Assignments.IsEnabled = true;
                    button_Edit.IsEnabled = true;
                }

                button_Delete.IsEnabled = true;
                button_Sell.IsEnabled = true;
            }
            else
            {
                button_Assignments.IsEnabled = false;
                button_Edit.IsEnabled = false;
                button_Delete.IsEnabled = false;
                button_Sell.IsEnabled = false;
            }
        }


        /// <summary>
        /// Player control event handlers.
        /// </summary>
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
                Player p = new Player(window.textBox_Player.Text, window.textBox_Character.Text, 10); //10 = temp int strength

                //Update the image if one was uploaded at player creation.
                if (window.hasimage == true)
                {
                    p.UpdateImage(window.imagearray);
                }
                viewModel.AddPlayerToModel(p);
            }
        }

        //Event handler for clicking the remove player button.
        private void button_RemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            RemovePlayer_Conf confwindow = new RemovePlayer_Conf();
            confwindow.ShowDialog();

            if (!confwindow.isCancelled)
            {                
                viewModel.RemovePlayerFromModel(viewModel.selectedPlayer);
                comboBox_Player.SelectedIndex = 0;
            }

            viewModel.NotifyPropertyChanged("LootList");
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
                p.NotifyPropertyChanged("characterimage");
            }
        }

        //Event handler for when the player combobox selection is changed.
        private void comboBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((comboBox_Player.SelectedIndex == 0))
            {
                button_RemovePlayer.IsEnabled = false;
                button_Browse.IsEnabled = false;
            }
            else
            {
                button_RemovePlayer.IsEnabled = true;
                button_Browse.IsEnabled = true;
            }
        }


        /// <summary>
        /// Currency control event handlers.
        /// </summary>
        //Event Hander for the LostFocus event on Currency Text fields (ALL).
        private void textBox_Currency_LostFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox element;

            switch ((sender as System.Windows.Controls.TextBox).Name)
            {
                case "textBox_ast_int":
                    element = textBox_ast_int; break;

                case "textBox_plt_int":
                    element = textBox_plt_int; break;

                case "textBox_gld_int":
                    element = textBox_gld_int; break;

                case "textBox_sil_int":
                    element = textBox_sil_int; break;

                case "textBox_cop_int":
                    element = textBox_cop_int; break;

                default:
                    element = textBox_gld_int; break;
            }

            try
            {
                int i = Convert.ToInt32(element.Text);
                if (i < 0)
                {
                    element.Text = "0";
                }
            }
            catch { element.Text = "0"; canclick = false; }

        }

        //Event Handler for left Click of Currency modification buttons.
        private void button_CurrencyModification_Click(object sender, RoutedEventArgs e)
        {
            //Setup element abstraction variables to handle a variety of sending events.
            System.Windows.Controls.TextBox element;
            System.Windows.Controls.TextBlock element_2;
            CurrencyType t;
            TranctionType tr;
            EventType et;
            int i3;

            //A massive switch to determine which button sent the event (1 of 10).
            switch ((sender as System.Windows.Controls.Button).Name)
            {
                case "button_ast_inc": element = textBox_ast_int; element_2 = AstVal; t = CurrencyType.Ast; tr = TranctionType.Inc; break;
                case "button_plt_inc": element = textBox_plt_int; element_2 = PltVal; t = CurrencyType.Plt; tr = TranctionType.Inc; break;
                case "button_gld_inc": element = textBox_gld_int; element_2 = GldVal; t = CurrencyType.Gld; tr = TranctionType.Inc; break;
                case "button_sil_inc": element = textBox_sil_int; element_2 = CopVal; t = CurrencyType.Sil; tr = TranctionType.Inc; break;
                case "button_cop_inc": element = textBox_cop_int; element_2 = CopVal; t = CurrencyType.Cop; tr = TranctionType.Inc; break;
                case "button_ast_dec": element = textBox_ast_int; element_2 = AstVal; t = CurrencyType.Ast; tr = TranctionType.Dec; break;
                case "button_plt_dec": element = textBox_plt_int; element_2 = PltVal; t = CurrencyType.Plt; tr = TranctionType.Dec; break;
                case "button_gld_dec": element = textBox_gld_int; element_2 = GldVal; t = CurrencyType.Gld; tr = TranctionType.Dec; break;
                case "button_sil_dec": element = textBox_sil_int; element_2 = SilVal; t = CurrencyType.Sil; tr = TranctionType.Dec; break;
                case "button_cop_dec": element = textBox_cop_int; element_2 = CopVal; t = CurrencyType.Cop; tr = TranctionType.Dec; break;
                default: element = textBox_gld_int; element_2 = GldVal; t = CurrencyType.Gld; tr = TranctionType.Inc; break;
            }

            //Another switch to determine the event that got us here (right/left click).
            switch (e.RoutedEvent.Name)
            {
                case "MouseRightButtonUp": et = EventType.Right; break;
                default: et = EventType.Left; break;
            }

            if (comboBox_Player.SelectedIndex != -1)
            {
                //Further absraction of element values to support generic event handling.
                Player p = comboBox_Player.SelectedItem as Player;
                int i = Convert.ToInt32(element.Text);
                int i2 = Convert.ToInt32(element_2.Text);
                

                //Datasource should be contrained enough that we don't get a negative value, but just in case...
                if (i < 0)
                {
                    return;
                }
                else if (i > 0)
                {
                    //Decrement logic operations for i.
                    if (tr == TranctionType.Dec)
                    {
                        //If what we wish to remove is less than what we have, flip i to neg.
                        if (i < i2)
                        {
                            i = i * -1;
                        }
                        //If what we want to remove is more (or equal to) what we have, just remove what we have.
                        else
                        {
                            i = i2 * -1;
                        }
                    }
                }
                else
                {
                    //Set the increment interger per the event type (Left = 1, Right = 10).
                    if (et == EventType.Left)
                    {
                        i3 = 1;
                    }
                    else
                    {
                        i3 = 10; 
                    }

                    if (tr == TranctionType.Dec)
                    {
                        //If what we wish to remove is less than what we have, flip i to neg.
                        if (i3 < i2)
                        {
                            i3 = i3 * -1;
                        }
                        //If what we want to remove is more (or equal to) what we have, just remove what we have.
                        else
                        {
                            i3 = i2 * -1;
                        }
                    }
                    if (canclick)
                    {
                        i = i3;
                    }
                    else
                    {
                        canclick = true;
                    }
                }

                //Pass the currency modification parameters to the viewModel for processing.
                viewModel.ModifyCurrency(p, t, i);

                //Set the textBox element back to 0.
                element.Text = "0";
            }
        }

        //Event Handler for creating a new lootitem from highlighted notes text.
        private void NewItemFromNotes_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Notes.SelectedText.Length > 0)
            {
                //Instantiate a new AddItem window.
                AddItem window = new AddItem(textBox_Notes.SelectedText);

                //Show the window.
                window.ShowDialog();

                if (!window.canceled)
                {
                    LootItem item = new LootItem(window.textBox_Name.Text, window.comboBox_Type.Text, (Convert.ToInt32(window.textBox_Count.Text)), (Convert.ToInt32(window.textBox_BaseValue.Text)), (Convert.ToDecimal(window.textBox_BaseWeight.Text)), window.textBox_Notes.Text);
                    viewModel.AddItemToModel(item);
                }
            }
        }


        /// <summary>
        /// Window control event handlers.
        /// </summary>
        //Event handler for clicking the close button.
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //Event handler for clicking the minimize button.
        private void button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        //Event handler for dragging window.
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
