using Weapon.Enum;

namespace Player.Model.Armor
{
    public class MilitaryHelmet : Armor
    {
        public MilitaryHelmet()
        {
            Name = "MilitaryHelmet";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 20;
        }
    }
}