using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace LootTracker
{
    [Serializable]
    public class LootBook
    {

        //Initialize a new list of players and loot items.
        ObservableCollection<Player> _playerlist = new ObservableCollection<Player>();
        ObservableCollection<LootItem> _lootlist = new ObservableCollection<LootItem>();

        //Public properties.
        public ObservableCollection<Player> playerlist { get { return _playerlist; } }
        public ObservableCollection<LootItem> lootlist { get { return _lootlist; } }

        public LootBook()
        {
            //Add a default "party" player to the lootbook.
            Player party = new Player("Party", "Party");

            //Get the default party image.
            Uri uri = new Uri("pack://application:,,,/party2.jpg");
            var info = Application.GetResourceStream(uri);
            var memoryStream = new MemoryStream();
            info.Stream.CopyTo(memoryStream);
            byte[] image =  memoryStream.ToArray();

            //Add the image to the party character.
            party.UpdateImage(image);

            //Add the player to the playerlist.
            AddPlayer(party);
        }

        //Method to add a player to a player roster object.
        public void AddPlayer(Player p)
        {
            _playerlist.Add(p);
        }

        //Method to remove a loot item from the lootlist.
        public void RemovePlayer(Player p)
        {
            if (_playerlist.Contains(p))
            {
                _playerlist.Remove(p);

            }
        }
        
        //Method to add a loot item to the lootlist.
        public void AddLootItem(LootItem l)
        {
            _lootlist. Add(l);
        }
        
        //Method to remove a loot item from the lootlist.
        public void RemoveLootItem(LootItem l)
        {
            if (_lootlist.Contains(l))
            {
                _lootlist.Remove(l);
                
            }
        }
    }
}
