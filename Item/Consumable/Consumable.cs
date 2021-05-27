
using Player.Model;
using Player.Model.ItemStats;

namespace Item.Consumable
{
    public class Consumable : IItem
    {
        public string Description { get; set; }
        public string ItemName { get; set; }
        public Rarity Rarity { get; set; }
    }
}