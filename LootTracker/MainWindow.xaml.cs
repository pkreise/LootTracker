using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;

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
        }

        //Declare a var book of type LootBook.
        public LootBook book;

        //Event Handler for opening an existing LootBook.
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            book = handler.ReadData();
        }

        //Event Handler for saving the open LootBook.
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(book);
        }

        //Event Handler for creating a new LootBook.
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            book = new LootBook();
        }

        //Event Handler for adding a new player to the roster.
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Player player = new Player("Dean", "Frederick");
            book.roster.AddPlayer(player);
        }
    }
}
