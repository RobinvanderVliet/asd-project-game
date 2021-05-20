using Player.Model.Armor.ArmorStats;
using Player.Model.ItemStats;

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