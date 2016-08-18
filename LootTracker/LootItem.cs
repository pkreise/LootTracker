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
        int _unassignedvalue = 0;
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
        public string unassignedvalue { get { return (String.Format("{0} {1}", _unassignedvalue, "GP")); } }
        public string baseweight { get { return (String.Format("{0} {1}", _baseweight, "Lbs")); } }
        public string totalweight { get { return (String.Format("{0} {1}", _totalweight, "Lbs")); } }
        public string basevalue { get { return (String.Format("{0} {1}", _basevalue, "GP")); } }
        public string totalvalue { get { return (String.Format("{0} {1}", _totalvalue, "GP")); } }
        public Dictionary<string, int> assignments { get { return _assignments; } set { _assignments = value; } }
        public string assignmentsstring
        {
            get
            {
                int a_count = _assignments.Count;
                string s_out = "";
                int p_count = 0;
                foreach (KeyValuePair<string, int> d in _assignments)
                {
                    s_out += (String.Format("{0}:{1}", d.Key, d.Value));
                    p_count++;
                    if (p_count < a_count)
                    {
                        s_out += ", ";
                    }
                }
                return s_out;
            }
        }
        
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

        //Method to calculate total value of a loot item.
        public void CalculateUnassignedValue()
        {
            _unassignedvalue = _unassignedcount * _basevalue;
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
            CalculateUnassignedValue();
        }

        //Method to decrement the count of a loot item.
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
            CalculateUnassignedValue();
        }

        //Method to modify ownership of the items.
        public void ModifiyAssignment(string PlayerName, int Count)
        {
            //If the player isn't in the dictionary, add the assignment.
            if (!_assignments.ContainsKey(PlayerName))
            {
                _assignments.Add(PlayerName, Count);
            }
            //Otherwise, remove them from the dictionary first, then add.
            else
            {
                _assignments.Remove(PlayerName);
                _assignments.Add(PlayerName, Count);
            }
            
            //Re-calculate the unassigned count and value.
            CalculateUnassignedCount();
            CalculateUnassignedValue();
        }

        //Method to modify ownership of the items.
        public void RemoveAssignment(string PlayerName)
        {
            //If the player isn't in the dictionary, add the assignment.
            if (_assignments.ContainsKey(PlayerName))
            {
                _assignments.Remove(PlayerName);
            }
           
            //Re-calculate the unassigned count and value.
            CalculateUnassignedCount();
            CalculateUnassignedValue();
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
