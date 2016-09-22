using System;
using System.ComponentModel;

namespace LootTracker
{
    [Serializable]
    public class Player : INotifyPropertyChanged
    {
        //Class fields.
        string _playername;
        string _charactername;
        byte[] _characterimage;
        bool _hasimage;
        int _equipmentvalue;
        int _wgtcarried;
        int _ast;
        int _plt;
        int _gld;
        int _sil;
        int _cop;
        int _strength;
        double _totalGP;
        bool _GPCarried;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        //Class properties.
        public string playername { get { return _playername; } }
        public string charactername { get { return _charactername; } }
        public byte[] characterimage { get { return _characterimage; } }
        public bool hasimage { get { return _hasimage; } set { _hasimage = value; } }
        public int equipmentvalue { get { return _equipmentvalue; } }
        public decimal wgtcarried { get { return _wgtcarried; } }
        public int ast { get { return _ast; } }
        public int gld { get { return _gld; } }
        public int plt { get { return _plt; } }
        public int sil { get { return _sil; } }
        public int cop { get { return _cop; } }
        public double totalGP { get { return _totalGP; } }
        public int strength { get { return _strength; } }
        public int LightLoadMax { get; }
        public int MedLoadMax { get; }
        public int HeavyLoadMax { get; }
        public int[] MaxLoads = new int[20] { 0, 4, 6, 10, 13, 16, 20, 23, 26, 30, 33, 38, 43, 50, 58, 66, 76, 86, 100, 116 };
        public bool GPCarried
        {
            get { return _GPCarried; }
            set
            {
                _GPCarried = value;
                NotifyPropertyChanged("GPCarried");
            }
        }

        //Constructor
        public Player()
        {
            _playername = null;
            _charactername = null;
            _equipmentvalue = 0;
            _wgtcarried = 0;
            _ast = 0;
            _plt = 0;
            _gld = 0;
            _sil = 0;
            _cop = 0;
            CalculateGP();
        }

        //Constructor
        public Player(string PlayerName, string CharachterName, int Strength)
        {
            _playername = PlayerName;
            _charactername = CharachterName;
            _strength = Strength;
            _equipmentvalue = 0;
            _wgtcarried = 0;
            _ast = 0;
            _plt = 0;
            _gld = 0;
            _sil = 0;
            _cop = 0;
            CalculateGP();
            
            
            // Calculate load based on strength value.
            if (_strength < 20)
            {
                LightLoadMax = MaxLoads[(_strength)];
                MedLoadMax = (2 * LightLoadMax);
                HeavyLoadMax = (3 * LightLoadMax);
            }
            else if (_strength >= 20)
            {
                LightLoadMax = (4 * MaxLoads[(_strength - 10)] + 1);
                MedLoadMax = (2 * LightLoadMax);
                HeavyLoadMax = (3 * LightLoadMax);
            }
            else if (_strength >= 30)
            {
                LightLoadMax = (16 * MaxLoads[(_strength - 20)] + 2);
                MedLoadMax = (2 * LightLoadMax);
                HeavyLoadMax = (3 * LightLoadMax);
            }
        }

        //NotifyPropertyChanged method.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        //Currency add methods.
        public void Addast(int Astral)
        {
            _ast += Astral;
            CalculateGP();
            NotifyPropertyChanged("ast");
        }
        public void Addplt(int Platinum)
        {
            _plt += Platinum;
            CalculateGP();
            NotifyPropertyChanged("plt");
        }
        public void Addgld(int Gold)
        {
            _gld += Gold;
            CalculateGP();
            NotifyPropertyChanged("gld");
        }
        public void Addsil(int Silver)
        {
            _sil += Silver;
            CalculateGP();
            NotifyPropertyChanged("sil");
        }
        public void Addcop(int Copper)
        {
            _cop += Copper;
            CalculateGP();
            NotifyPropertyChanged("cop");
        }

        //Currency remove methods.
        public void Removeast(int Astral)
        {
            _ast -= Astral;
            CalculateGP();
            NotifyPropertyChanged("ast");
        }
        public void Removeplt(int Platinum)
        {
            _plt -= Platinum;
            CalculateGP();
            NotifyPropertyChanged("plt");
        }
        public void Removegld(int Gold)
        {
            _gld -= Gold;
            CalculateGP();
            NotifyPropertyChanged("gld");
        }
        public void Removesil(int Silver)
        {
            _sil -= Silver;
            CalculateGP();
            NotifyPropertyChanged("sil");
        }
        public void Removecop(int Copper)
        {
            _cop -= Copper;
            CalculateGP();
            NotifyPropertyChanged("cop");
        }

        //TotalGP calculation method.
        private void CalculateGP()
        {
            _totalGP = ((_cop * .01) + (_sil * .1) + _gld + (_plt * 10) + (_ast * 100));
            NotifyPropertyChanged("totalGP");
        }

        //Update Image method.
        public void UpdateImage(byte[] Image)
        {
            _characterimage = Image;
            _hasimage = true;
            NotifyPropertyChanged("characterimage");
        }                    
    }
}
