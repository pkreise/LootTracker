using System;
using System.Collections.Generic;

namespace LootTracker
{
    [Serializable]
    public class PlayerRoster
    {
        //Initialize a new list of player objects.
        List<Player> _playerlist = new List<Player>();

        //Public property.
        public List<Player> playerlist { get { return _playerlist; } }

        //Method to add a player to a player roster object.
        public void AddPlayer(Player Player)
        {
            _playerlist.Add(Player);
        }
    }
}
