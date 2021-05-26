using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class Bandana : Armor
    {
        private const string ArmorDescription = "Default headwear, plain but good looking";
        public Bandana()
        {
            ItemName = "Bandana";
            Description = ArmorDescription;
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Common;
            ArmorProtectionPoints = 1;
        }
    }
}