using System.Windows;
using System.Collections;
using System.IO;
using System;

namespace LootTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Initialize the XML handler class.
            XmlHandler xmlhandler = new XmlHandler();

            //Read the XML and initialize our lootbook.
            //LootBook book = xmlhandler.ReadXML();

            LootBook book = new LootBook();

            //Define test player.
            Player player = new Player("Dean", "Frederick");

            byte[] playerimage = File.ReadAllBytes("C:\\Users\\ddenson\\Desktop\\IC848627.png");
            player.UpdateImage(playerimage);

            //Give the test player 10000 gold.
            player.AddGold(10000);

            //Add player to PlayerRoster.
            book.roster.AddPlayer(player);

            //Define test loot item.
            LootItem item = new LootItem("TestItem1", "Weapon", 10, 1000, 12);

            //Add test loot item to lootbook.
            if (!book.lootlist.Contains(item))
            {
                book.AddLootItem(item);
            }
            else
            {
                Console.WriteLine("Unable to add item.  {0} is already present in the collection.", item.itemname);
            }
        }
    }
}
