using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

namespace Player.Model.Armor.HazardProtected
{
    public class HazmatSuit : HazardProtectedArmor
    {
        public HazmatSuit()
        {
            ItemName = "Hazmat suit";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 20;
            RadiationProtectionPoints = 80;
            StaminaPoints = -20;
        }
    }
}