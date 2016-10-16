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
        int _strength;
        bool _GPCarried;
        int Ast;
        int Plt;
        int Gld;
        int Sil;
        int Cop;
        double TotalGP;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        //Class properties.
        public string playername { get { return _playername; } }
        public string charactername { get { return _charactername; } }
        public byte[] characterimage { get { return _characterimage; } }
        public bool hasimage { get { return _hasimage; } set { _hasimage = value; } }
        public int equipmentvalue { get { return _equipmentvalue; } }
        public decimal wgtcarried { get { return _wgtcarried; } }
        public int ast { get { return Ast; } set { Ast = value; NotifyPropertyChanged("ast"); } }
        public int plt { get { return Plt; } set { Plt = value; NotifyPropertyChanged("plt"); } }
        public int gld { get { return Gld; } set { Gld = value; NotifyPropertyChanged("gld"); } }
        public int sil { get { return Sil; } set { Sil = value; NotifyPropertyChanged("sil"); } }
        public int cop { get { return Cop; } set { Cop = value; NotifyPropertyChanged("cop"); } }
        public double totalGP { get { return TotalGP; } set { TotalGP = value; NotifyPropertyChanged("totalGP"); } }
        public int strength { get { return _strength; } }
        public int LightLoadMax { get; set; }
        public int MedLoadMax { get; set; }
        public int HeavyLoadMax { get; set; }
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
            ast = 0;
            plt = 0;
            gld = 0;
            sil = 0;
            cop = 0;
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
            ast = 0;
            plt = 0;
            gld = 0;
            sil = 0;
            cop = 0;
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
        public void modifyCurrency(CurrencyType t, int i)
        {
            switch (t)
            {
                case CurrencyType.Ast: ast += i; break;
                case CurrencyType.Plt: plt += i; break;
                case CurrencyType.Gld: gld += i; break;
                case CurrencyType.Sil: sil += i; break;
                case CurrencyType.Cop: cop += i; break;
            }            
            CalculateGP();            
        }

        //TotalGP calculation method.
        private void CalculateGP()
        {
            totalGP = ((cop * .01) + (sil * .1) + gld + (plt * 10) + (ast * 100));
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
