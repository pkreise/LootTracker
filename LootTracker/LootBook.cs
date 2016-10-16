using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace LootTracker
{
    [Serializable]
    public class LootBook : INotifyPropertyChanged
    {
        //Class Fields.
        private ObservableCollection<Player> _playerlist = new ObservableCollection<Player>();
        private ObservableCollection<LootItem> _lootlist = new ObservableCollection<LootItem>();
        private string _notes;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        //Class properties.
        public ObservableCollection<Player> playerlist { get { return _playerlist; } set { _playerlist = value; } }
        public ObservableCollection<LootItem> lootlist { get { return _lootlist; } set { _lootlist = value; } }
        public string notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(notes)));
            }
        }

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

        //Method for cloning a loot book (deeeeeeeep clone).
        public LootBook Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, this);

            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();

            return obj as LootBook;
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
                NotifyPropertyChanged("playerlist");

                foreach (LootItem i in _lootlist)
                {
                    i.RemoveAssignment(p.playername);
                }
                NotifyPropertyChanged("lootlist");
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

        //Method to calculate the total weight of items (# * basewgt)
        public decimal CalculateTotalWeight()
        {
            decimal _totalPartyWeight = 0;
            foreach (LootItem loot in _lootlist)
            {
                _totalPartyWeight = _totalPartyWeight + (loot.count * loot.baseweight);
            }
            return _totalPartyWeight;
        }

        //Method to calculate a players light load threshold.
        public int CalculateMaxLightLoad()
        {
            int _lightLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _lightLoadTotal += (Dude.LightLoadMax);
            }
            return _lightLoadTotal;
        }

        //Method to calculate a players medium load threshold.
        public int CalculateMaxMedLoad()
        {
            int _medLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _medLoadTotal += (Dude.MedLoadMax);
            }
            return _medLoadTotal;
        }

        //Method to calculate a players heavy load threshold.
        public int CalculateMaxheavyLoad()
        {
            int _heavyLoadTotal = 0;
            foreach (Player Dude in _playerlist)
            {
                _heavyLoadTotal += (Dude.HeavyLoadMax);
            }
            return _heavyLoadTotal;
        }
    }
}
