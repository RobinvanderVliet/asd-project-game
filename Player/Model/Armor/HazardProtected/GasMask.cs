using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor.HazardProtected
{
    public class GasMask : HazardProtectedArmor
    {
        public GasMask()
        {
            ItemName = "Gas mask";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
            RadiationProtectionPoints = 40;
            StaminaPoints = -20;
        }
    }
}