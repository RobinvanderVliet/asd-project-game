using Weapon.Enum;

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