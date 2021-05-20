using Weapon.Enum;

namespace Player.Model.Armor
{
    public class Armor
    {
        protected string Name { get; set; }
        protected ArmorType ArmorType { get; set; }
        protected int ArmorProtectionPoints { get; set; }
        protected Rarity Rarity { get; set; }
    }
}