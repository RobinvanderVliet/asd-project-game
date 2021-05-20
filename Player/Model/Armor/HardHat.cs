using Weapon.Enum;

namespace Player.Model.Armor
{
    public class HardHat : Armor
    {
        public HardHat()
        {
            Name = "HardHat";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 10;
        }
    }
}