using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class Jacket : Armor
    {
        private const string ArmorDescription = "My new jacket is reversible, as it turns out";

        public Jacket()
        {
            ItemName = "Jacket";
            Description = ArmorDescription;
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Common;
            ArmorProtectionPoints = 10;
        }
    }
}