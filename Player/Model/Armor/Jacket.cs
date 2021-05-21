using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class Jacket : Armor
    {
        public Jacket()
        {
            ItemName = "Jacket";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Common;
            ArmorProtectionPoints = 10;
        }
    }
}