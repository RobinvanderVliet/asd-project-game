using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor
{
    public class MilitaryHelmet : Armor
    {
        public MilitaryHelmet()
        {
            Name = "Military helmet";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 20;
        }
    }
}