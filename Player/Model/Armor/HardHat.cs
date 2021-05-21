using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class HardHat : Armor
    {
        private const string ArmorDescription = "Bob the builder, can we fix it";

        public HardHat()
        {
            ItemName = "Hard hat";
            Description = ArmorDescription;
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 10;
        }
    }
}