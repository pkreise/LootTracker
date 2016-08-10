using System.Collections.Generic;
using System;

namespace LootTracker
{
    [Serializable]
    public class LootItem
    {
        //Define class Fields.
        public string itemname = null;
        public string loottype = null;
        public int count = 0;
        public int unassignedcount = 0;
        public decimal baseweight = 0;
        public decimal totalweight = 0;
        public int basevalue = 0;
        public int totalvalue = 0;
        public Dictionary<string, int> assignments = new Dictionary<string, int>();
        //public Hashtable assignments = new Hashtable();
        public int assignedcount = 0;

        //Default constructor for initializing a new loot item.
        public LootItem()
        {
            itemname = null;
            loottype = null;
            count = 0;
            basevalue = 0;
            unassignedcount = 0;
            baseweight = 0;
            assignments = null;
            CalculateTotalValue();
            CalculateTotalWeight();
        }

        //Constructor for initializing a new loot item.
        public LootItem(string ItemName, string LootType, int Count, int BaseValue, decimal BaseWeight)
        {
            itemname = ItemName;
            loottype = LootType;
            count = Count;
            basevalue = BaseValue;
            unassignedcount = Count;
            baseweight = BaseWeight;
            CalculateTotalValue();
            CalculateTotalWeight();
        }

        //Method to calculate total value of a loot item.
        public void CalculateTotalValue()
        {
            totalvalue = count * basevalue;
        }

        //Method to calculate total weight of a loot item.
        public void CalculateTotalWeight()
        {
            totalweight = count * baseweight;
        }

        //Method to increment the count of a loot item.
        public void IncrementCount(int Count)
        {
            count += Count;
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
        }

        //Method to decrease the count of a loot item.
        public void DecrementCount(int Count)
        {
            if (count <= Count)
            {
                count = 0;
            }
            else
            {
                count -= Count;
            }
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
        }

        //Method to modify ownership of the items.
        public void ModifiyAssignment(string PlayerName, int NewCount)
        {
            //If the player isn't in the dictionary, add the assignment.
            if (!assignments.ContainsKey(PlayerName))
            {
                assignments.Add(PlayerName, NewCount);
            }
            //Otherwise, remove them from the dictionary first then add.
            else
            {
                assignments.Remove(PlayerName);
                assignments.Add(PlayerName, NewCount);
            }
            
            //Re-calculate the unassigned count.
            CalculateUnassignedCount();
        }

        //Method to calculate the unassigned count after item assignment.
        public void CalculateUnassignedCount()
        {
            assignedcount = 0;
            foreach (KeyValuePair<string, int> entry in assignments)
            {
                assignedcount += Convert.ToInt32(entry.Value);
            }

            unassignedcount = count - assignedcount;
        }

        
    }
}
