using System.Windows;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declare a var book of type LootBook.
        public LootBook book;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Event Handler for opening an existing LootBook.
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            book = handler.ReadData();
        }

        //Event Handler for saving the open LootBook.
        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(book);
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

                //Update the image if one was 
                if (window.hasimage == true)
                {
                    player.UpdateImage(window.imagearray);
                }
                book.roster.AddPlayer(player);
            }
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
