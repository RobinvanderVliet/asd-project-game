using Items.Armor.ArmorStats;
using Player.Model;
using Player.Model.ItemStats;

namespace Items.Armor
{
    public class Armor : Item
    {
        public ArmorPartType ArmorPartType { get; set; }
        public int ArmorProtectionPoints { get; set; }
        public Rarity Rarity { get; set; }
    }
}