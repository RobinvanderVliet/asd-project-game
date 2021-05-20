using Weapon.Enum;

namespace Player.Model.Armor.HazardProtected
{
    public class GasMask : HazardProtectedArmor
    {
        public GasMask()
        {
            Name = "Gas mask";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
            RPP = 40;
            SP = -20;
        }
    }
}