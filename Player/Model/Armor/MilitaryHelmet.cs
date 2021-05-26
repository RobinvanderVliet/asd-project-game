using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class MilitaryHelmet : Armor
    {
        private const string ArmorDescription = "A shell-met!";

        public MilitaryHelmet()
        {
            ItemName = "Military helmet";
            Description = ArmorDescription;
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 20;
        }
    }
}