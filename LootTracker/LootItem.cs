using System.Collections.Generic;
using System;

namespace LootTracker
{
    [Serializable]
    public class LootItem
    {
        //Define class Fields.
        string _itemname = null;
        string _loottype = null;
        int _count = 0;
        int _unassignedcount = 0;
        int _assignedcount = 0;
        decimal _baseweight = 0;
        decimal _totalweight = 0;
        int _basevalue = 0;
        int _totalvalue = 0;
        Dictionary<string, int> _assignments = new Dictionary<string, int>();

        //Define properties.
        public string itemname { get {return _itemname; }  }
        public string loottype { get { return _loottype; } }
        public int count { get { return _count; } }
        public int unassignedcount { get { return _unassignedcount; } }
        public int assignedcount { get { return _assignedcount; } }
        public decimal baseweight { get { return _baseweight; } }
        public decimal totalweight { get { return _totalweight; } }
        public int basevalue { get { return _basevalue; } }
        public int totalvalue { get { return _totalvalue; } }
        public Dictionary<string, int> assignments { get { return _assignments; } }
        
        //Default constructor for initializing a new loot item.
        public LootItem()
        {
            _itemname = null;
            _loottype = null;
            _count = 0;
            _basevalue = 0;
            _unassignedcount = 0;
            _baseweight = 0;
            _assignments = null;
            CalculateTotalValue();
            CalculateTotalWeight();
        }

        //Constructor for initializing a new loot item.
        public LootItem(string ItemName, string LootType, int Count, int BaseValue, decimal BaseWeight)
        {
            _itemname = ItemName;
            _loottype = LootType;
            _count = Count;
            _basevalue = BaseValue;
            _unassignedcount = Count;
            _baseweight = BaseWeight;
            CalculateTotalValue();
            CalculateTotalWeight();
        }

        //Method to calculate total value of a loot item.
        public void CalculateTotalValue()
        {
            _totalvalue = _count * _basevalue;
        }

        //Method to calculate total weight of a loot item.
        public void CalculateTotalWeight()
        {
            _totalweight = _count * _baseweight;
        }

        //Method to increment the count of a loot item.
        public void IncrementCount(int Count)
        {
            _count += Count;
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
        }

        //Method to decrease the count of a loot item.
        public void DecrementCount(int Count)
        {
            if (_count <= Count)
            {
                _count = 0;
            }
            else
            {
                _count -= Count;
            }
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
        }

        //Method to modify ownership of the items.
        public void ModifiyAssignment(string PlayerName, int NewCount)
        {
            //If the player isn't in the dictionary, add the assignment.
            if (!_assignments.ContainsKey(PlayerName))
            {
                _assignments.Add(PlayerName, NewCount);
            }
            //Otherwise, remove them from the dictionary first then add.
            else
            {
                _assignments.Remove(PlayerName);
                _assignments.Add(PlayerName, NewCount);
            }
            
            //Re-calculate the unassigned count.
            CalculateUnassignedCount();
        }

        //Method to calculate the unassigned count after item assignment.
        public void CalculateUnassignedCount()
        {
            _assignedcount = 0;
            foreach (KeyValuePair<string, int> entry in _assignments)
            {
                _assignedcount += Convert.ToInt32(entry.Value);
            }

            _unassignedcount = _count - _assignedcount;
        }
    }
}
