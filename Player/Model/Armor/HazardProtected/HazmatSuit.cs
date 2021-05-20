using Weapon.Enum;

namespace Player.Model.Armor.HazardProtected
{
    public class HazmatSuit : HazardProtectedArmor
    {
        public HazmatSuit()
        {
            Name = "Hazmat suit";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 20;
            RPP = 80;
            SP = -20;
        }
    }
}