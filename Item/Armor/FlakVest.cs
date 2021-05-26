
using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class FlakVest : Armor
    {
        private const string ArmorDescription = "BOOM, wait it fiiiiiiine";

        public FlakVest()
        {
            ItemName = "Flak vest";
            Description = ArmorDescription;
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
        }
    }
}