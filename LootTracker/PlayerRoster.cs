using System.Collections.Generic;

namespace LootTracker
{
    public class PlayerRoster
    {
        //Method to add a player to a player roster object.
        public void AddPlayer(Player Player)
        {
            playerlist.Add(Player);
        }

        //Initialize a new list of player objects.
        public List<Player> playerlist = new List<Player>();
    }
}
