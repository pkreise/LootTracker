using System.Windows;
using System;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declare vars.
        public LootBook book = new LootBook();
        public string savefilepath;
        
        

        public MainWindow()
        {
            InitializeComponent();
        }

        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            book = handler.ReadData();
            savefilepath = handler.filepath;
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
            if (!window.canceled)
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

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(true, book, savefilepath);
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Instantiate a new AddPlayer window.
            AddItem window = new AddItem();

            //Show the window.
            window.ShowDialog();

            if (!window.canceled)
            {
                LootItem item = new LootItem(window.textBox_Name.Text, window.comboBox_Type.Text, (Convert.ToInt32(window.textBox_Count.Text)), (Convert.ToInt32(window.textBox_BaseValue.Text)), (Convert.ToInt64(window.textBox_BaseWeight.Text)));
                book.AddLootItem(item);
                listView.ItemsSource = book.lootlist.ToArray();

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = book.lootlist.ToArray();
        }
    }
}
