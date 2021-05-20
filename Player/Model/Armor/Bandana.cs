using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class Bandana : Armor
    {
        public Bandana()
        {
            Name = "Bandana";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Common;
            ArmorProtectionPoints = 1;
        }
    }
}