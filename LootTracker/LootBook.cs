using System.Collections.Generic;

namespace LootTracker
{
    public class LootBook
    {
        //Add a player roster to the lootbook.
        public PlayerRoster roster = new PlayerRoster();

        //Add a list of loot items to the lootbook.
        public List<LootItem> lootlist = new List<LootItem>();

        //Method to add a loot item to the lootlist.
        public void AddLootItem(LootItem lootitem)
        {
            lootlist.Add(lootitem);
        }

        //Method to remove a loot item from the lootlist.
        public void RemoveLootItem(LootItem lootitem)
        {
            if (lootlist.Contains(lootitem))
            {
                lootlist.Remove(lootitem);
            }
        }
    }
}
