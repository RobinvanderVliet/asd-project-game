using Weapon.Enum;

namespace Player.Model.Armor
{
    public class GasMask : HazardProtectedArmor
    {
        public GasMask()
        {
            Name = "FlakVest";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
            RPP = 40;
            SP = -20;
        }
    }
}