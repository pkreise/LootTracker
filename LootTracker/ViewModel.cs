using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace LootTracker
{
    public enum CurrencyType { Ast, Plt, Gld, Sil, Cop }
    public enum TranctionType { Inc, Dec }
    public enum EventType { Right, Left }

    class ViewModel : INotifyPropertyChanged
    {
        LootBook book = new LootBook();
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private CollectionView view_items;
        public event PropertyChangedEventHandler PropertyChanged;
        private string StringFilter = "";
        private int SelectedTabIndex;
        private Player SelectedPlayer;
        private ComboBoxItem SelectedItemFilter;
        
        public ObservableCollection<LootItem> LootList { get { return book.lootlist; } set { book.lootlist = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LootList))); } }
        public ObservableCollection<Player> PlayerList { get { return book.playerlist; } set { book.playerlist = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayerList))); } }
        public int selectedTabIndex { get { return SelectedTabIndex; } set { SelectedTabIndex = value; view_items.Refresh(); } }
        public Player selectedPlayer { get { return SelectedPlayer; } set { SelectedPlayer = value; view_items.Refresh(); } }
        public ComboBoxItem selectedItemFilter { get { return SelectedItemFilter; } set { SelectedItemFilter = value; view_items.Refresh(); } }
        public string stringFilter
        {
            get { return StringFilter; }
            set
            {
                if (value == StringFilter)
                {
                    return;
                }
                else
                {
                    StringFilter = value;

                    GCLatencyMode oldMode = GCSettings.LatencyMode;
                    GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                    view_items.Refresh();
                    GCSettings.LatencyMode = oldMode;

                    
                }                
            }
        }

        //cTor.
        public ViewModel()
        {
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(book.lootlist);
            view_items.Filter = LootFilter;
        }

        //Sorting method for the listview.
        private void Sort(string sortBy, ListSortDirection direction)
        {
            //Create a dictionary to lookup the binding values by the header passed in.
            Dictionary<string, string> headerlookup = new Dictionary<string, string>();
            headerlookup.Add("Name", "itemname");
            headerlookup.Add("Type", "loottype");
            headerlookup.Add("#", "count");
            headerlookup.Add("Unequipped", "unassignedcount");
            headerlookup.Add("Base Wgt", "baseweight");
            headerlookup.Add("Ttl Wgt", "totalweight");
            headerlookup.Add("Base Val", "basevalue");
            headerlookup.Add("Ttl Val", "totalvalue");
            headerlookup.Add("Assignments", "assignmentsstring");
            headerlookup.Add("Equipped", "assignmentsstring");
            headerlookup.Add("Equipped Wgt", "baseweight");
            headerlookup.Add("Equipped Val", "basevalue");

            //Create a dataview and clear.
            ICollectionView dataView = CollectionViewSource.GetDefaultView(book.lootlist);
            dataView.SortDescriptions.Clear();

            //Create a new sortdescription.
            SortDescription sd = new SortDescription((headerlookup[sortBy]), direction);

            //Add the sortdescription to the dataview and refresh.
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        //Player Loot Filter method.
        private bool LootFilter(object item)
        {
            //General
            LootItem i = item as LootItem;

            //StringFilter/ItemFilter part.
            if (SelectedTabIndex == 0)
            {
                string typeFilterValue = SelectedItemFilter.Content.ToString();
                //string NonCaseSensitiveFilter = StringFilter.ToLower();

                if (typeFilterValue == "All Items" && i.itemname.ToLower().Contains(StringFilter))
                {
                    return true;
                }
                else if (i.loottype == typeFilterValue && i.itemname.ToLower().Contains(StringFilter))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //PlayerFilterPart.
            else if (SelectedTabIndex == 1)
            {
                if (SelectedPlayer == null)
                {
                    return false;
                }
                else if (SelectedPlayer.playername == "Party")
                {
                    if (i.unassignedcount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    bool playerexists = false;
                    playerexists = i.assignments.ContainsKey(SelectedPlayer.playername);
                    if (playerexists)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        //Adds a player instance to the lootbook.
        public void AddPlayerToModel(Player p)
        {
            book.AddPlayer(p);
        }

        //Removes a player instace from the lootbook.
        public void RemovePlayerFromModel(Player p)
        {
            book.RemovePlayer(p);
        }

        //Adds an item instance to the lootbook.
        public void AddItemToModel(LootItem i)
        {
            book.AddLootItem(i);
        }

        //Removes an item instance from the lootbook.
        public void RemoveItemFromModel(LootItem i)
        {
            book.RemoveLootItem(i);
        }

        //Refreshes the view of the listView.
        public void RefreshView()
        {
            view_items.Refresh();
        }

        //Opens an existing lootbook.
        public void OpenLootBook()
        {
            DataHandler handler = new DataHandler();
            book = handler.ReadData();
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(book.lootlist);
            view_items.Filter = LootFilter;
        }

        //Creates a new lootbook.
        public void NewLootBook()
        {
            book = new LootBook();
            view_items = (CollectionView)CollectionViewSource.GetDefaultView(book.lootlist);
            view_items.Filter = LootFilter;
        }

        //Saves the existing lootbook.
        public void SaveLootBook()
        {
            DataHandler handler = new DataHandler();
            handler.WriteData(book);
        }

        //Implementation of the iNotifyPropertyChanged interface.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //Modifies the currecy of a given player instance.
        public void ModifyCurrency(Player p, CurrencyType t, int i)
        {
            p.modifyCurrency(t, i);                    
        }       
    }
}
