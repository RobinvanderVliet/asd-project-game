using Item.Armor.ArmorStats;
using Player.Model;
using Player.Model.ItemStats;

namespace Item.Armor
{
    public class Armor : IItem
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public ArmorPartType ArmorPartType { get; set; }
        public int ArmorProtectionPoints { get; set; }
        public Rarity Rarity { get; set; }
    }
}