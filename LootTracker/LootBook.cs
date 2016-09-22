using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace LootTracker
{
    [Serializable]
    public class LootBook : INotifyPropertyChanged
    {
        //Class Fields.
        ObservableCollection<Player> _playerlist = new ObservableCollection<Player>();
        ObservableCollection<LootItem> _lootlist = new ObservableCollection<LootItem>();
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        //Class properties.
        public ObservableCollection<Player> playerlist { get { return _playerlist; } }
        public ObservableCollection<LootItem> lootlist { get { return _lootlist; } }

        //Constructor
        public LootBook()
        {
            //Add a default "party" player to the lootbook.
            Player party = new Player("Party", "Party", 0);

            //Get the default party image.
            Uri uri = new Uri("pack://application:,,,/party2.jpg");
            var info = Application.GetResourceStream(uri);
            var memoryStream = new MemoryStream();
            info.Stream.CopyTo(memoryStream);
            byte[] image = memoryStream.ToArray();

            //Add the image to the party character.
            party.UpdateImage(image);

            //Add the player to the playerlist.
            AddPlayer(party);
        }

        //NotifyPropertyChanged method.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

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
            NotifyPropertyChanged("lootlist");
        }
        
        //Method to remove a loot item from the lootlist.
        public void RemoveLootItem(LootItem l)
        {
            if (_lootlist.Contains(l))
            {
                _lootlist.Remove(l);
                NotifyPropertyChanged("lootlist");

            }
        }

        public decimal CalculateTotalWeight()
        {
            decimal _totalPartyWeight = 0;
            foreach (LootItem loot in _lootlist)
            {
                _totalPartyWeight = _totalPartyWeight + (loot.count * loot.baseweight);
            }
            return _totalPartyWeight;
        }

        public int CalculateMaxLightLoad()
        {
            int _lightLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _lightLoadTotal += (Dude.LightLoadMax);
            }
            return _lightLoadTotal;
        }

        public int CalculateMaxMedLoad()
        {
            int _medLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _medLoadTotal += (Dude.MedLoadMax);
            }
            return _medLoadTotal;
        }

        public int CalculateMaxheavyLoad()
        {
            int _heavyLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _heavyLoadTotal += (Dude.HeavyLoadMax);
            }
            return _heavyLoadTotal;
        }

        public void lootlistChanged()
        {
            NotifyPropertyChanged("lootlist");
        }

        public void playerlistChanged()
        {
            NotifyPropertyChanged("playerlist");
        }


    }
}
