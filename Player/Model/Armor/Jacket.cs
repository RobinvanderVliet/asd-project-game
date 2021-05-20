using Weapon.Enum;

namespace Player.Model.Armor
{
    public class Jacket : Armor
    {
        public Jacket()
        {
            Name = "Jacket";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Common;
            ArmorProtectionPoints = 10;
        }
    }
}