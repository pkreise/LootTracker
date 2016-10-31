using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LootTracker
{
    [Serializable]
    public class LootItem : INotifyPropertyChanged
    {
        //Class Fields.
        private string _notes;
        Dictionary<string, int> _assignments = new Dictionary<string, int>();
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        //Class properties.
        public string itemname { get; set; }
        public string loottype { get; set; }
        public int count { get; set; }
        public int unassignedcount { get; set; }
        public int assignedcount { get; set; }
        public int unassignedvalue { get; set; }
        public decimal baseweight { get; set; }
        public decimal totalweight { get; set; }
        public int basevalue { get; set; }
        public int totalvalue { get; set; }
        public int charges { get; set; }
        public Dictionary<string, int> assignments { get { return _assignments; } set { _assignments = value; NotifyPropertyChanged("assignments"); } }
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

        //Constructor
        public LootItem(string ItemName, string LootType, int Count, int BaseValue, decimal BaseWeight, string Notes)
        {
            itemname = ItemName;
            loottype = LootType;
            count = Count;
            basevalue = BaseValue;
            unassignedcount = Count;
            baseweight = BaseWeight;
            notes = Notes;
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
            CalculateUnassignedValue();
        }

        //Method for cloning a loot item (deeeeeeeep clone).
        public LootItem Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, this);

            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();

            return obj as LootItem;
        }

        //NotifyPropertyChanged method.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        //Method to calculate total value of a loot item.
        public void CalculateTotalValue()
        {
            totalvalue = count * basevalue;
        }

        //Method to calculate total value of a loot item.
        public void CalculateUnassignedValue()
        {
            unassignedvalue = unassignedcount * basevalue;
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
            CalculateUnassignedValue();
            NotifyPropertyChanged("_count");
        }

        //Method to decrement the count of a loot item.
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
            CalculateUnassignedValue();
            NotifyPropertyChanged("_count");
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
            NotifyPropertyChanged("_unassignedcount");
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
            NotifyPropertyChanged("_unassignedcount");
        }

        //Method to calculate the unassigned count after item assignment.
        public void CalculateUnassignedCount()
        {
            assignedcount = 0;
            foreach (KeyValuePair<string, int> entry in _assignments)
            {
                assignedcount += Convert.ToInt32(entry.Value);
            }

            unassignedcount = count - assignedcount;
        }

        //Combo method to perform all calculations.
        public void CalculateAllValues()
        {
            CalculateTotalValue();
            CalculateTotalWeight();
            CalculateUnassignedCount();
            CalculateUnassignedValue();
        }
    }
}
