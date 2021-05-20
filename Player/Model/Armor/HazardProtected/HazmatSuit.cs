using Weapon.Enum;

namespace Player.Model.Armor.HazardProtected
{
    public class HazmatSuit : HazardProtectedArmor
    {
        public HazmatSuit()
        {
            Name = "FlakVest";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
            RPP = 40;
            SP = -20;
        }
    }
}