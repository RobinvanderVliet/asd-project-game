using Items.ArmorStats;
using Items.ItemStats;

namespace Items
{
    public class Armor : Item
    {
        public ArmorPartType ArmorPartType { get; set; }
        public int ArmorProtectionPoints { get; set; }
        public Rarity Rarity { get; set; }
    }
}