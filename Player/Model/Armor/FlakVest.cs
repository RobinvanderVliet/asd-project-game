using Weapon.Enum;

namespace Player.Model.Armor
{
    public class FlakVest : Armor
    {
        public FlakVest()
        {
            Name = "FlakVest";
            ArmorType = ArmorType.Body;
            Rarity = Rarity.Uncommon;
            ArmorProtectionPoints = 20;
        }
    }
}