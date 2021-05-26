
using Player.Model.ItemStats;

namespace Player.Model.Consumable
{
    public class Consumable : IItem
    {
        public string Description { get; set; }
        public string ItemName { get; set; }
        protected Rarity Rarity { get; set; }
    }
}