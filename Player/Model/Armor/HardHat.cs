using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class HardHat : Armor
    {
        public HardHat()
        {
            Name = "Hard hat";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 10;
        }
    }
}