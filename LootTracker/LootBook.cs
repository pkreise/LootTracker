﻿using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;


namespace LootTracker
{
    [Serializable]
    public class LootBook
    {
        //Add a player roster to the lootbook.
        public PlayerRoster roster = new PlayerRoster();

        //Add a list of loot items to the lootbook.
        public ObservableCollection<LootItem> lootlist = new ObservableCollection<LootItem>();
        

        //Method to add a loot item to the lootlist.
        public void AddLootItem(LootItem lootitem)
        {
            lootlist. Add(lootitem);
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
