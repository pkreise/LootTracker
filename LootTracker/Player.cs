using System;

namespace LootTracker
{
    [Serializable]
    public class Player
    {
        //Declare class vars.
        public string playername;
        public string charactername;
        public byte[] characterimage;
        public int equipmentvalue;
        public int wgtcarried;
        public int astral;
        public int platinum;
        public int gold;
        public int silver;
        public int copper;
        public double totalGP;

        //The default constructor for creating a new player object.
        public Player()
        {
            playername = null;
            charactername = null;
            equipmentvalue = 0;
            wgtcarried = 0;
            astral = 0;
            platinum = 0;
            gold = 0;
            silver = 0;
            copper = 0;
            CalculateGP();
        }

        //The constructor for creating a new player object.
        public Player(string PlayerName, string CharachterName)
        {
            playername = PlayerName;
            charactername = CharachterName;
            equipmentvalue = 0;
            wgtcarried = 0;
            astral = 0;
            platinum = 0;
            gold = 0;
            silver = 0;
            copper = 0;
            CalculateGP();
        }

        //The constructor for rehydrating an existing player.
        public Player(string PlayerName, string CharachterName, byte[] CharacterImage, int EquipmentValue, int WgtCarried, int Astral, int Platinum, int Gold, int Silver, int Copper)
        {
            playername = PlayerName;
            charactername = CharachterName;
            equipmentvalue = EquipmentValue;
            wgtcarried = WgtCarried;
            astral = Astral;
            platinum = Platinum;
            gold = Gold;
            silver = Silver;
            copper = Copper;
            characterimage = CharacterImage;
            CalculateGP();
        }

        //Currency add methods.
        public void AddAstral(int Astral)
        {
            astral += Astral;
            CalculateGP();
        }

        public void AddPlatinum(int Platinum)
        {
            platinum += Platinum;
            CalculateGP();
        }

        public void AddGold(int Gold)
        {
            gold += Gold;
            CalculateGP();
        }

        public void AddSilver(int Silver)
        {
            silver += Silver;
            CalculateGP();
        }

        public void AddCopper(int Copper)
        {
            copper += Copper;
            CalculateGP();
        }

        //Currency remove methods.
        public void RemoveAstral(int Astral)
        {
            astral -= Astral;
            CalculateGP();
        }

        public void RemovePlatinum(int Platinum)
        {
            platinum -= Platinum;
            CalculateGP();
        }

        public void RemoveGold(int Gold)
        {
            gold -= Gold;
            CalculateGP();
        }

        public void RemoveSilver(int Silver)
        {
            silver -= Silver;
            CalculateGP();
        }

        public void RemoveCopper(int Copper)
        {
            copper -= Copper;
            CalculateGP();
        }

        //TotalGP calculation method.
        public void CalculateGP()
        {
            totalGP = ((copper * .01) + (silver * .1) + gold + (platinum * 10) + (astral * 100));
        }

        //Update Image method.
        public void UpdateImage(byte[] Image)
        {
            characterimage = Image;
        }
    }
}
