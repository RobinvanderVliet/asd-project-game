using Weapon.Enum;

namespace Player.Model.Armor
{
    public class TacticalVest : Armor
    {
        public TacticalVest()
        {
            Name = "TacticalVest";
            ArmorType = ArmorType.Helmet;
            Rarity = Rarity.Rare;
            ArmorProtectionPoints = 40;
        }
    }
}