using System;

namespace LootTracker
{
    [Serializable]
    public class Player
    {
        //Declare class fields.
        string _playername;
        string _charactername;
        byte[] _characterimage;
        int _equipmentvalue;
        int _wgtcarried;
        int _astral;
        int _platinum;
        int _gold;
        int _silver;
        int _copper;
        double _totalGP;

        //Define public properties.
        public string playername { get { return _playername; } }
        public string charactername { get { return _charactername; } }
        public byte[] characterimage { get { return _characterimage; } }
        public int equipmentvalue { get { return _equipmentvalue; } }
        public decimal wgtcarried { get { return _wgtcarried; } }
        public int astral { get { return _astral; } }
        public int gold { get { return _gold; } }
        public int platinum { get { return _platinum; } }
        public int silver { get { return _silver; } }
        public int copper { get { return _copper; } }
        public double totalGP { get { return _totalGP; } }



        //The default constructor for creating a new player object.
        public Player()
        {
            _playername = null;
            _charactername = null;
            _equipmentvalue = 0;
            _wgtcarried = 0;
            _astral = 0;
            _platinum = 0;
            _gold = 0;
            _silver = 0;
            _copper = 0;
            CalculateGP();
        }

        //The constructor for creating a new player object.
        public Player(string PlayerName, string CharachterName)
        {
            _playername = PlayerName;
            _charactername = CharachterName;
            _equipmentvalue = 0;
            _wgtcarried = 0;
            _astral = 0;
            _platinum = 0;
            _gold = 0;
            _silver = 0;
            _copper = 0;
            CalculateGP();
        }

        //The constructor for rehydrating an existing player.
        public Player(string PlayerName, string CharachterName, byte[] CharacterImage, int EquipmentValue, int WgtCarried, int Astral, int Platinum, int Gold, int Silver, int Copper)
        {
            _playername = PlayerName;
            _charactername = CharachterName;
            _equipmentvalue = EquipmentValue;
            _wgtcarried = WgtCarried;
            _astral = Astral;
            _platinum = Platinum;
            _gold = Gold;
            _silver = Silver;
            _copper = Copper;
            _characterimage = CharacterImage;
            CalculateGP();
        }

        //Currency add methods.
        public void AddAstral(int Astral)
        {
            _astral += Astral;
            CalculateGP();
        }

        public void AddPlatinum(int Platinum)
        {
            _platinum += Platinum;
            CalculateGP();
        }

        public void AddGold(int Gold)
        {
            _gold += Gold;
            CalculateGP();
        }

        public void AddSilver(int Silver)
        {
            _silver += Silver;
            CalculateGP();
        }

        public void AddCopper(int Copper)
        {
            _copper += Copper;
            CalculateGP();
        }

        //Currency remove methods.
        public void RemoveAstral(int Astral)
        {
            _astral -= Astral;
            CalculateGP();
        }

        public void RemovePlatinum(int Platinum)
        {
            _platinum -= Platinum;
            CalculateGP();
        }

        public void RemoveGold(int Gold)
        {
            _gold -= Gold;
            CalculateGP();
        }

        public void RemoveSilver(int Silver)
        {
            _silver -= Silver;
            CalculateGP();
        }

        public void RemoveCopper(int Copper)
        {
            _copper -= Copper;
            CalculateGP();
        }

        //TotalGP calculation method.
        public void CalculateGP()
        {
            _totalGP = ((_copper * .01) + (_silver * .1) + _gold + (_platinum * 10) + (_astral * 100));
        }

        //Update Image method.
        public void UpdateImage(byte[] Image)
        {
            _characterimage = Image;
        }
    }
}
