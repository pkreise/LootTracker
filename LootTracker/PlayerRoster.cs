using System;
using System.Collections.ObjectModel;

namespace LootTracker
{
    [Serializable]
    public class PlayerRoster
    {
        //Initialize a new list of player objects.
        ObservableCollection<Player> _playerlist = new ObservableCollection<Player>();

        //Public property.
        public ObservableCollection<Player> playerlist { get { return _playerlist; } }

        //Method to add a player to a player roster object.
        public void AddPlayer(Player Player)
        {
            _playerlist.Add(Player);
        }
    }
}
