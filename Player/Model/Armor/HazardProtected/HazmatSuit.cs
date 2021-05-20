using Weapon.Enum;

namespace Player.Model.Armor
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